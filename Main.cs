﻿using UnityModManagerNet;
using System;
using System.Reflection;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using System.Collections.Generic;
using Kingmaker.Blueprints.Items;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace KingmakerAI
{
    internal class Main
    {
        internal class Settings
        {
            [JsonProperty]
            internal string scripts_working_folder { get; set; }
            [JsonProperty]
            internal string party_info_file { get; set; }
            internal int dungeon_hard_encounters_per_stage { get; set; }
            internal int dungeon_corrupted_unit_chance { get; set; }
            internal float dungeon_experience_reward_per_stage_scaling { get; set; }
            internal float dungeon_cr_scaling { get; set; }

            internal float ai_chance_to_hit_weight { get; set; }
            internal float ai_distance_to_target_weight { get; set; }
            internal float ai_engaged_by_target_weight { get; set; }

            internal Settings()
            {

                using (StreamReader settings_file = File.OpenText(UnityModManager.modsPath + @"/KingmakerAi/settings.json"))
                using (JsonTextReader reader = new JsonTextReader(settings_file))
                {
                    JObject jo = (JObject)JToken.ReadFrom(reader);
                    scripts_working_folder = (string)jo["scripts_working_folder"];
                    party_info_file = (string)jo["party_info_file"];
                    dungeon_hard_encounters_per_stage = Math.Max(Math.Min(6, (int)jo["dungeon_hard_encounters_per_stage"]), 0);
                    dungeon_corrupted_unit_chance = Math.Max(Math.Min(100, (int)jo["dungeon_corrupted_unit_chance"]), 0);
                    dungeon_experience_reward_per_stage_scaling = Math.Max((float)jo["dungeon_experience_reward_per_stage_scaling"], 0.0f);
                    dungeon_cr_scaling = Math.Max((float)jo["dungeon_cr_scaling"], 0.0f);

                    ai_chance_to_hit_weight = Math.Min(Math.Max((float)jo["ai_chance_to_hit_weight"], 0.0f), 1.0f);
                    ai_distance_to_target_weight = Math.Min(Math.Max((float)jo["ai_distance_to_target_weight"], 0.0f), 1.0f);
                    ai_engaged_by_target_weight = Math.Min(Math.Max((float)jo["ai_engaged_by_target_weight"], 0.0f), 1.0f);
                }
            }
        }

        static internal Settings settings = new Settings();
        internal static UnityModManagerNet.UnityModManager.ModEntry.ModLogger logger;
        internal static Harmony12.HarmonyInstance harmony;
        internal static LibraryScriptableObject library;

        static readonly Dictionary<Type, bool> typesPatched = new Dictionary<Type, bool>();
        static readonly List<String> failedPatches = new List<String>();
        static readonly List<String> failedLoading = new List<String>();

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DebugLog(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        internal static void DebugError(Exception ex)
        {
            if (logger != null) logger.Log(ex.ToString() + "\n" + ex.StackTrace);
        }
        internal static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                modEntry.OnGUI = Scripting.UI.onGUI;
                logger = modEntry.Logger;
                harmony = Harmony12.HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());



            }
            catch (Exception ex)
            {
                DebugError(ex);
                throw ex;
            }
            return true;
        }
        [Harmony12.HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        [Harmony12.HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary", new Type[0])]
        static class LibraryScriptableObject_LoadDictionary_Patch
        {
            static void Postfix(LibraryScriptableObject __instance)
            {
                var self = __instance;
                if (Main.library != null) return;
                Main.library = self;
                try
                {
                    Main.logger.Log("Loading Kingmaker AI");

#if DEBUG                
                    bool allow_guid_generation = true;
#else
                    bool allow_guid_generation = false; //no guids should be ever generated in release
#endif
                    CallOfTheWild.Helpers.GuidStorage.load(Properties.Resources.blueprints, allow_guid_generation);

                    UpdateAi.load();
                    Core.load();
                    EndlessDungeon.load();

#if DEBUG
                    string guid_file_name = @"C:\Repositories\KingmakerAI\KingmakerAI\blueprints.txt";
                    CallOfTheWild.Helpers.GuidStorage.dump(guid_file_name);
#endif
                    CallOfTheWild.Helpers.GuidStorage.dump(@"./Mods/KingmakerAI/loaded_blueprints.txt");
                    Scripting.UI.initialize(Main.settings.scripts_working_folder, Main.settings.party_info_file);

                    Type TurnController = Type.GetType("TurnBased.Controllers.TurnController, TurnBased");
                    Type TurnControllerOwlcat = Type.GetType("TurnBased.Controllers.TurnController, Assembly-CSharp");
                    if (TurnController != null)
                    {
                        logger.Log("Found TurnBased mod, patching...");
                        harmony.Patch(Harmony12.AccessTools.Method(TurnController, "ContinueActing"),
                                       postfix: new Harmony12.HarmonyMethod(typeof(TurnBasedPatches.TurnController_ContinueActing_Patch), "Postfix")
                                     );
                        harmony.Patch(Harmony12.AccessTools.Method(TurnController, "ContinueWaiting"),
                                        postfix: new Harmony12.HarmonyMethod(typeof(TurnBasedPatches.TurnController_ContinueWaiting_Patch), "Postfix")
                                    );
                    }
                    else if (TurnControllerOwlcat != null)
                    {
                        logger.Log("Found Official TurnBased, patching...");
                        harmony.Patch(Harmony12.AccessTools.Method(TurnControllerOwlcat, "ContinueActing"),
                                    postfix: new Harmony12.HarmonyMethod(typeof(TurnBasedOwlcatsPatches.TurnController_ContinueActing_Patch), "Postfix")
                                );

                        harmony.Patch(Harmony12.AccessTools.Method(TurnControllerOwlcat, "ContinueWaiting"),
                                     prefix: new Harmony12.HarmonyMethod(typeof(TurnBasedOwlcatsPatches.TurnController_ContinueWaiting_Patch), "Prefix")
                        );
                    }
                    else
                    {
                        logger.Log("TurnBased mod not found.");
                    }

                }
                catch (Exception ex)
                {
                    Main.DebugError(ex);
                }
            }
        }

        internal static Exception Error(String message)
        {
            logger?.Log(message);
            return new InvalidOperationException(message);
        }
    }
}

