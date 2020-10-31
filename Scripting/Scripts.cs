using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Classes.Spells;
using CallOfTheWild;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Threading;
using System.Text.RegularExpressions;
using Kingmaker.Blueprints;

/*Possible commands
 * 
 * cast ability_name[.variant] target_descriptor
 * move_to target_descriptor
 * activate ability_name
 * deactivate ability_name
 * 
 * target_descriptor = [1][2][3][4][5][6][7][8][9][A][B][C][D][E][F][s][p]
 * [1 - 9, A - F] - correspond to unit tags (i.e action will be applied to all unit that have at least one tag, so 123 will be applied to anyone who has tag 1, 2 or 3)
 * s - apply on self
 * p - apply on own pet
 * 
 * 
 * 
 * 
 */



namespace KingmakerAI.Scripting
{
    public class UnitDeactivateAbility : UnitCommand
    {
        ActivatableAbility activatable_ability;

        public UnitDeactivateAbility(ActivatableAbility ability)
            :base(CommandType.Free, null)
        {
            activatable_ability = ability;
        }

        protected override ResultType OnAction()
        {
            if (!activatable_ability.IsTurnedOn)
            {
                return ResultType.Fail;
            }
            else
            {
                activatable_ability.IsOn  = false;
                return ResultType.Success;
            }
        }
    }


    public interface CommandProvider
    {
        UnitCommand getCommand();
    }


    public class ActivateAbilityProvider: CommandProvider
    {
        UnitEntityData unit;
        BlueprintActivatableAbility activatable_ability;


        public ActivateAbilityProvider(UnitEntityData executor, BlueprintActivatableAbility ability)
        {
            unit = executor;
            activatable_ability = ability;
        }

        public UnitCommand getCommand()
        {
            var aa = unit.ActivatableAbilities.Enumerable.Where(a => a.Blueprint == activatable_ability && !a.IsOn).FirstOrDefault();
            if (aa == null)
            {
                return null;
            }
            else
            {
                aa.IsOn = true;
                return new UnitActivateAbility(aa);
            }
        }
    }


    public class DeactivateAbilityProvider : CommandProvider
    {
        UnitEntityData unit;
        BlueprintActivatableAbility activatable_ability;

        public DeactivateAbilityProvider(UnitEntityData executor, BlueprintActivatableAbility ability)
        {
            unit = executor;
            activatable_ability = ability;
        }

        public UnitCommand getCommand()
        {
            var aa = unit.ActivatableAbilities.Enumerable.Where(a => a.Blueprint == activatable_ability && a.IsOn).FirstOrDefault();
            if (aa == null)
            {
                return null;
            }
            else
            {
                return new UnitDeactivateAbility(aa);
            }
        }
    }




    public class MoveToTargetProvider : CommandProvider
    {
        UnitEntityData unit;
        TargetWrapper target;

        public MoveToTargetProvider(UnitEntityData executor, TargetWrapper move_target)
        {
            unit = executor;
            target = move_target;
        }

        public UnitCommand getCommand()
        {
            return new UnitMoveTo(target.Point,2.Feet().Meters);
        }
    }


    public class ScriptParser
    {
        private static string USE_ABILITY_COMMAND => "cast";
        private static string ACTIVATE_ABILITY_COMMAND => "activate";
        private static string DEACTIVATE_ABILITY_COMMAND => "deactivate";
        private static string MOVE_TO_COMMAND => "move_to";

        public static List<CommandProvider> parse(string code, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            List<CommandProvider> providers = new List<CommandProvider>();
            error = "";
            var lines = Regex.Split(code, "\r\n|\r|\n");

            for (int i = 0; i < lines.Length; i++)
            {
                var new_command_providers = parseLine(lines[i], unit, party_mask_map, out error);

                if (new_command_providers == null)
                {
                    error = "Error on line " + (i + 1).ToString() + ": " + lines[i] + " >> " + error;
                    return null;
                }
                else
                {
                    providers.AddRange(new_command_providers);
                }
            }

            return providers;
        }


        static List<CommandProvider> parseLine(string line, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            error = "";

            var entries = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (entries.Empty())
            {
                return null;
            }

            if (entries[0] == USE_ABILITY_COMMAND)
            {
                return parseUseAbilityCommand(entries, unit, party_mask_map, out error);
            }
            else if (entries[0] == MOVE_TO_COMMAND)
            {
                return parseMoveToCommand(entries, unit, party_mask_map, out error);
            }
            else if (entries[0] == ACTIVATE_ABILITY_COMMAND)
            {
                return parseActivateAbilityCommand(entries, unit, party_mask_map, out error);
            }
            else if (entries[0] == DEACTIVATE_ABILITY_COMMAND)
            {
                return parseDeactivateAbilityCommand(entries, unit, party_mask_map, out error);
            }

            return null;
        }


        static List<CommandProvider> parseDeactivateAbilityCommand(string[] entries, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            error = "";
            if (entries.Length != 2)
            {
                error = DEACTIVATE_ABILITY_COMMAND + " " + " must have 1 argument";
            }

            var ability = tryGetBlueprint<BlueprintActivatableAbility>(entries[1]);
            if (ability == null)
            {
                error = "Can not find activatable ability <" + entries[1] + ">";
                return null;
            }

            List<CommandProvider> providers = new List<CommandProvider>();

            providers.Add(new DeactivateAbilityProvider(unit, ability));

            return providers;
        }


        static List<CommandProvider> parseActivateAbilityCommand(string[] entries, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            error = "";
            if (entries.Length != 2)
            {
                error = ACTIVATE_ABILITY_COMMAND + " " + " must have 1 argument";
            }

            var ability = tryGetBlueprint<BlueprintActivatableAbility>(entries[1]);
            if (ability == null)
            {
                error = "Can not find activatable ability <" + entries[1] + ">";
                return null;
            }

            List<CommandProvider> providers = new List<CommandProvider>();

            providers.Add(new ActivateAbilityProvider(unit,  ability));

            return providers;
        }


        static List<CommandProvider> parseMoveToCommand(string[] entries, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            error = "";
            if (entries.Length != 3)
            {
                error = MOVE_TO_COMMAND + " " + " must have 1 argument";
            }
  
            var mask = getTargetMask(entries[1]);
            List<UnitEntityData> units = new List<UnitEntityData>();

            foreach (var p in party_mask_map)
            {
                if (isUnitinMask(unit, p.Key, p.Value, mask))
                {
                    units.Add(p.Key);
                }
            }

            List<CommandProvider> providers = new List<CommandProvider>();

            foreach (var u in units)
            {
                providers.Add(new MoveToTargetProvider(unit, u));
            }

            return providers;
        }


        static List<CommandProvider> parseUseAbilityCommand(string[] entries, UnitEntityData unit, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map, out string error)
        {
            error = "";
            if (entries.Length != 3)
            {
                error = USE_ABILITY_COMMAND + " " + " must have 2 arguments";
            }

            var spells = entries[1].Split('.');

            if (spells.Length > 2)
            {
                error = USE_ABILITY_COMMAND + " " + " spell argument must be specified as ability[.variant]";
            }

            var ability = tryGetBlueprint<BlueprintAbility>(spells[0]);
            BlueprintAbility variant = null;
            if (ability == null)
            {
                error = "Can not find ability <" + spells[0] +">";
                return null;
            }

            if (spells.Length == 2)
            {
                variant = tryGetBlueprint<BlueprintAbility>(spells[1]);
                if (variant == null)
                {
                    error = "Can not find ability variant <" + spells[1] + ">";
                    return null;
                }
            }

            var mask = getTargetMask(entries[2]);
            List<UnitEntityData> units = new List<UnitEntityData>();

            foreach (var p in party_mask_map)
            {
                if (isUnitinMask(unit, p.Key, p.Value, mask))
                {
                    units.Add(p.Key);
                }
            }

            List<CommandProvider> providers = new List<CommandProvider>();

            foreach (var u in units)
            {
                providers.Add(new UseAbilityProvider(unit, u, ability, variant));
            }

            return providers;
        }


        static bool isUnitinMask(UnitEntityData caster, UnitEntityData unit, ScriptController.UnitTags unit_tags, ScriptController.TargetMask target_mask)
        {
            if (target_mask.self && unit == caster)
            {
                return true;
            }
            if (target_mask.pet && unit == caster.Descriptor.Pet)
            {
                return true;
            }

            if ((unit_tags & target_mask.tags) != 0)
            {
                return true;
            }

            return false;
        }

        static ScriptController.TargetMask getTargetMask(string s)
        {
            var all_tags = Enum.GetValues(typeof(ScriptController.UnitTags)).Cast<ScriptController.UnitTags>().ToArray();
            var mask = new ScriptController.TargetMask();
            foreach (var c in s)
            {
                if (c == 's')
                {
                    mask.self = true;
                }
                else if (c == 'p')
                {
                    mask.pet = true;
                }
                else
                {
                    int num = Array.FindIndex(ScriptController.unit_tags_char, val => val == c);
                    if (num > 0 && num < all_tags.Length )
                    {
                        mask.tags = mask.tags | all_tags[num];
                    }
                }
            }

            return mask;
        }


        static T tryGetBlueprint<T>(string name) where T: BlueprintScriptableObject
        {
            return Main.library.GetAllBlueprints().OfType<T>().Where(b => b.name == name).FirstOrDefault();
        }
    }


    public class ScriptExecutor
    {
        private UnitCommand current_command_ = null;
        private object current_command_lock = new object();


        public UnitCommand current_command
        {
            get
            {
                lock (current_command_lock)
                {
                    return current_command_;
                }
            }
            set
            {
                lock (current_command_lock)
                {
                    current_command_ = value;
                }
            }
        }

        UnitEntityData unit;
        List<CommandProvider> command_providers = new List<CommandProvider>();
        bool is_ok;
        public string error;

        public bool isOk() { return is_ok; }
        public string getError() { return error; }

        public ScriptExecutor(string code, UnitEntityData executor, Dictionary<UnitEntityData, ScriptController.UnitTags> party_mask_map)
        {
            unit = executor;
            command_providers = ScriptParser.parse(code, executor, party_mask_map, out error);
            if (command_providers == null)
            {
                is_ok = false;
                return;
            }
            is_ok = true;

        }

        public void run()
        {
            tryRunNextCommand();
        }

        private void tryRunNextCommand()
        {
            current_command = null;
            if (command_providers.Empty())
            {
                return;
            }

            current_command = command_providers.First().getCommand();
            command_providers.RemoveAt(0);

            if (current_command == null)
            {
                tryRunNextCommand();
            }

            unit.Commands.InterruptAll();
            unit.Commands.AddToQueue(current_command);
        }

        public bool HandleUnitCommandDidEnd(UnitCommand command)
        {
            if (current_command != command)
            {
                return false;
            }
            //Main.logger.Log("Checking result: " + command.Result.ToString());
            if (command.Result != UnitCommand.ResultType.Success && !(command is UnitMoveTo))
            {
                return false;
            }
            //Main.logger.Log("Check ok");
            tryRunNextCommand();
            return true;
        }
    }



    public class UseAbilityProvider: CommandProvider
    {
        BlueprintAbility ability;
        BlueprintAbility variant;
        UnitEntityData caster;
        TargetWrapper target;

        public UseAbilityProvider(UnitEntityData executor, TargetWrapper spell_target, BlueprintAbility ability_to_use, BlueprintAbility variant_to_use = null)
        {
            ability = ability_to_use;
            variant = variant_to_use;
            caster = executor;
            target = spell_target;
        }


        public UnitCommand getCommand()
        {
            var ab = getAbilityData();
            if (ab == null)
            {
                return null;
            }

            return new UnitUseAbility(ab, target);
        }


        private AbilityData getAbilityData()
        {
            var descriptor = caster.Descriptor;
            var ad = this.getAbilityForUse(descriptor);
            if ((ad != null))
                return ad;
            BlueprintSpellbook spellbook = this.getSpellbookForUse(descriptor);
            if (spellbook == null)
                return null;
            return this.CreateAbilityData(descriptor, null, spellbook);
        }


        private BlueprintSpellbook getSpellbookForUse(UnitDescriptor unit)
        {
            return unit.Spellbooks.FirstOrDefault<Spellbook>((Func<Spellbook, bool>)(spellbook => spellbook.CanSpend(ability)))?.Blueprint;
        }


        private AbilityData getAbilityForUse(UnitDescriptor unit)
        {
            Kingmaker.UnitLogic.Abilities.Ability ab = unit.Abilities.GetAbility(ability);
            if (ab == null)
                return (AbilityData)null;
            return this.CreateAbilityData(unit, ab, null);
        }

        private AbilityData CreateAbilityData(
          UnitDescriptor caster,
          Kingmaker.UnitLogic.Abilities.Ability fact,
          BlueprintSpellbook spellbook)
        {
            AbilityData ad = new AbilityData(ability, caster, fact, spellbook);
            if (variant == null)
                return ad;
            return new AbilityData(variant, caster, fact, spellbook)
            {
                ConvertedFrom = ad
            };
        }
    }


    public class ScriptController
    {
        [Flags]
        public enum UnitTags
        {
            None = 0,
            Custom1 = 1,
            Custom2 = 2,
            Custom3 = 4,
            Custom4 = 8,
            Custom5 = 16,
            Custom6 = 32,
            Custom7 = 64,
            Custom8 = 128,
            Custom9 = 256,
            CustomA = 512,
            CustomB = 1024,
            CustomC = 2048,
            CustomD = 4096,
            CustomE = 8192,
            CustomF = 16384
        }

        public static char[] unit_tags_char = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public class TargetMask
        {
            public UnitTags tags;
            public bool pet;
            public bool self;
        }


        static List<ScriptExecutor> script_executors = new List<ScriptExecutor>();
        static Dictionary<UnitEntityData, UnitTags> party_mask_map = new Dictionary<UnitEntityData, UnitTags>();

        public static void run()
        {
            runScripts();
        }

        public static void reset()
        {
            script_executors.Clear();
            UnitCommand__OnEnded__Patch.clearScripts();
        }

        static void runScripts()
        {
            UnitCommand__OnEnded__Patch.updateScripts(script_executors.ToArray());
            foreach (var s in script_executors)
            {
                s.run();
            }
        }


        static public void registerParty(Dictionary<UnitEntityData, UnitTags> party)
        {
            party_mask_map.Clear();
            foreach (var kv in party)
            {
                party_mask_map.Add(kv.Key, kv.Value);
            }
        }

        static public void registerScriptFile(string filename, UnitEntityData unit)
        {
            var party = Kingmaker.Game.Instance?.Player?.Party;
            if (party == null || party.Empty())
            {
                return;
            }

            string code = "";

            try
            {
                code = File.ReadAllText(filename);
            }
            catch (Exception e)
            {
                Main.logger.Log("Failed to read file: " + filename);
                return;
            }
            var script_exec = new ScriptExecutor(code, unit, party_mask_map);
            if (!script_exec.isOk())
            {
                Main.logger.Log("Failed to compile script from file: " + filename + ": " +  script_exec.getError());
                return;
            }

            script_executors.Add(script_exec);
        }
    }


    [Harmony12.HarmonyPatch(typeof(UnitCommand))]
    [Harmony12.HarmonyPatch("OnEnded", Harmony12.MethodType.Normal)]
    class UnitCommand__OnEnded__Patch
    {
        static List<ScriptExecutor> script_executors = new List<ScriptExecutor>();
        private static Mutex script_execution_mutex = new Mutex();

        static public void updateScripts(params ScriptExecutor[] new_script_executors)
        {
            script_execution_mutex.WaitOne();
            script_executors = new_script_executors.ToList();
            script_execution_mutex.ReleaseMutex();
        }

        static public void clearScripts()
        {
            script_execution_mutex.WaitOne();
            script_executors.Clear();
            script_execution_mutex.ReleaseMutex();
        }


        static void Postfix(UnitCommand __instance)
        {
            script_execution_mutex.WaitOne();
            foreach (var s in script_executors)
            {
                if (s.HandleUnitCommandDidEnd(__instance))
                {
                    break;
                }
            }
            script_execution_mutex.ReleaseMutex();
        }
    }
}
