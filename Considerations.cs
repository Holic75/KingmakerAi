using CallOfTheWild;
using Kingmaker.Controllers.Brain;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KingmakerAI.NewConsiderations
{
    public class AcConsideration : Consideration
    {
        static Dictionary<(UnitEntityData, UnitEntityData), int> uncertainty_values = new Dictionary<(UnitEntityData, UnitEntityData), int>();
        static System.Random rng = new System.Random();

        public float min_score = 0.1f;
        public float engaged_by_bonus = 0.05f;
        public float engage_bonus = 0.15f;
        public float max_score = 1.0f;
        public static BlueprintActivatableAbility tumble = Main.library.Get<BlueprintActivatableAbility>("4be5757b85af47545a5789f1d03abda9");

        public override float Score(DecisionContext context)
        {
            
            UnitEntityData attacker = context.Unit;
            UnitEntityData target = context.Target.Unit ?? context.Unit;

            if (attacker == null || target == null)
            {
                return min_score;
            }
            var weapon = attacker.Body?.PrimaryHand?.MaybeWeapon;

            if (!attacker.Body.HandsAreEnabled && attacker.Body.AdditionalLimbs.Count > 0)
            {
                weapon = attacker.Body.AdditionalLimbs[0].MaybeWeapon;
            }
            int odds = -10;
            if (weapon != null)
            {
                var attack_bonus = Rulebook.Trigger(new RuleCalculateAttackBonus(attacker, target, weapon, 0)).Result;
                var target_ac = Rulebook.Trigger(new RuleCalculateAC(attacker, target, weapon.Blueprint.AttackType)).TargetAC;
                odds = attack_bonus - target_ac;
            }

            int uncertainty = 0;
            if (uncertainty_values.ContainsKey((attacker, target)))
            {
                uncertainty = uncertainty_values[(attacker, target)];
            }
            else
            {
                float uncertanity_base = 0.0f;
                float sanity = Math.Max(-4, (attacker.Stats.Intelligence.Bonus + attacker.Stats.Wisdom.Bonus) / 2);
                if (sanity >= 0)
                {
                    uncertanity_base = (float)(2.0 + 0.5f*sanity); // 10 for 0,  7 for +2,  5 for + 4,   2 for + 16
                }
                else
                {
                    uncertanity_base = 2.0f + 0.25f * sanity; //20 for -4, 14 for -2, 10 for 0
                }
                //20 for -4, 13 for -2, 10 for 0, 7 for +2,  5 for +4, 4 for +6, 2 for +16
                
                int uncertainty_range = (int)Math.Round(20.0f / uncertanity_base);               
                if (uncertanity_base != 0)
                {
                    uncertainty = rng.Next(0, uncertainty_range);
                }
                uncertainty_values[(attacker, target)] = uncertainty;
            }


            odds = Math.Min(Math.Max(odds, -20), 0); //from -20 to 0
            odds += uncertainty;
            odds = Math.Min(Math.Max(odds, -20), 0); //from -20 to 0

            var score = (float)(odds + 20) / 20.0f;
            score *= 0.5f;
            //try to use tumble if possible
            var tumble_toggle = attacker.ActivatableAbilities.Enumerable.Where(a => a.Blueprint == tumble).FirstOrDefault();
            if (tumble_toggle != null && !attacker.IsPlayerFaction)
            {
                tumble_toggle.IsOn = attacker.CombatState.IsEngaged && attacker.Stats.GetStat(Kingmaker.EntitySystem.Stats.StatType.SkillMobility).ModifiedValue >= 10;
            }

            
            if (attacker.CombatState.IsEngage(target))
            {
                score += engage_bonus;
            }
            if (attacker.CombatState.EngagedBy.Contains(target))
            {
                score += engaged_by_bonus;
            }

            return Math.Max(Math.Min(score, max_score), min_score);
        }

        public static void clearUncertanityValues(UnitEntityData unit)
        {

            foreach (var k in uncertainty_values.Keys.ToArray())
            {
                if (unit == k.Item1 || unit == k.Item2)
                {
                    uncertainty_values.Remove(k);
                }
            }
            //Main.logger.Log("Stored uncertanities: " + uncertainty_values.Count.ToString());
        }
    }


    [Harmony12.HarmonyPatch(typeof(UnitEntityData))]
    [Harmony12.HarmonyPatch("LeaveCombat", Harmony12.MethodType.Normal)]
    class BlueprintAbility__LeaveCombat__Patch
    {
        static void Postfix(UnitEntityData __instance)
        {
            AcConsideration.clearUncertanityValues(__instance);
        }
    }
}
