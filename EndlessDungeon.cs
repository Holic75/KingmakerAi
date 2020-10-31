using CallOfTheWild;
using Kingmaker.Dungeon.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI
{
    public class EndlessDungeon
    {
        static internal void load()
        {
            var dungeon_root = Main.library.Get<BlueprintDungeonRoot>("d5ac7a99563987948b653b03b341167b");
            dungeon_root.HardEncountersPerStage = Main.settings.dungeon_hard_encounters_per_stage;
            dungeon_root.ExperienceRewardForCompleteStageStart = (int)(dungeon_root.ExperienceRewardForCompleteStageStart * Main.settings.dungeon_experience_reward_per_stage_scaling);
            dungeon_root.ExperienceRewardForCompleteStageStep = (int)(dungeon_root.ExperienceRewardForCompleteStageStep * Main.settings.dungeon_experience_reward_per_stage_scaling);
            dungeon_root.CorruptedUnitChance = Main.settings.dungeon_corrupted_unit_chance;
        }
    }


    //fix unit spells/brains on area loaded
    [Harmony12.HarmonyPatch(typeof(BlueprintDungeonRoot), "GetStageCR")]
    class BlueprintDungeonRoot_GetStageCR_Patch
    {
        static void Postfix(BlueprintDungeonRoot __instance, ref int __result)
        {
            __result = Math.Max((int)(__result * Main.settings.dungeon_cr_scaling), 2);
        }
    }
}
