using CallOfTheWild;
using Kingmaker;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Controllers.Brain;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KingmakerAI.NewConsiderations
{
    public class UnitPartAttackScoreStorage : UnitPart, IUnitCombatHandler, IGlobalSubscriber
    {
        public Dictionary<UnitEntityData, int> attack_scores = new Dictionary<UnitEntityData, int>();

        public void HandleUnitJoinCombat(UnitEntityData unit)
        {
            if (unit.Descriptor == this.Owner)
            {
                attack_scores.Clear();
            }
        }

        public void HandleUnitLeaveCombat(UnitEntityData unit)
        {
            if (unit.Descriptor == this.Owner)
            {
                Main.logger.Log("Clearing attack scores for " + this.Owner.CharacterName);
                attack_scores.Clear();
            }
            else
            {
                attack_scores.Remove(unit);
            }
        }
    }

    public class ArmorAroundConsideration : UnitsAroundConsideration
    {
        public bool is_light;

        public override bool IsOk(UnitEntityData target)
        {
            if (!base.IsOk(target))
                return false;
           
            ItemEntityArmor maybeArmor = target.Body.Armor.MaybeArmor;
            if (maybeArmor == null)
                return is_light;
            switch (maybeArmor.Blueprint.Type.ProficiencyGroup)
            {
                case ArmorProficiencyGroup.None:
                case ArmorProficiencyGroup.Light:
                    return is_light;
                case ArmorProficiencyGroup.Medium:
                case ArmorProficiencyGroup.Heavy:
                    return !is_light;
                case ArmorProficiencyGroup.Buckler:
                case ArmorProficiencyGroup.LightShield:
                case ArmorProficiencyGroup.HeavyShield:
                case ArmorProficiencyGroup.TowerShield:
                    //UberDebug.LogWarning(target, string.Format("Shield in armor slot ({0})", target));
                    return !is_light;
                default:
                    return is_light;
            }
        }
    }

    public class TargetFactionConsideration : Consideration
    {
        [Range(0.0f, 1f)]
        public float enemy_score;
        [Range(0.0f, 1f)]
        public float ally_score;

        public override float Score(DecisionContext context)
        {
            if ((context.Target.Unit ?? context.Unit).IsEnemy(context.Unit))
                return this.enemy_score;
            return this.ally_score;
        }
    }


    public class UnitPolymorphed : Consideration
    {
        public float polymorphed_score;
        public float not_polymorphed_score;

        public override float Score(DecisionContext context)
        {
            var unit = context.Target?.Unit;
            if (unit == null)
            {
                return 0.0f;
            }

            return unit.Body.IsPolymorphed ? polymorphed_score : not_polymorphed_score;
        }
    }


    public class BabPartConsideration : Consideration
    {
        public float min_score = 0f;
        public float max_score = 1.0f;
        public float min_bab_part = 0.7f;

        public override float Score(DecisionContext context)
        {
            var unit = context.Target?.Unit;
            if (unit == null)
            {
                return min_score;
            }

            float val = (float) unit.Descriptor.Stats.BaseAttackBonus.ModifiedValue / (float) unit.Descriptor.Progression.CharacterLevel;
            if (val < min_bab_part)
            {
                return min_score;
            }
            else
            {
                return Math.Min(max_score, val);
            }
        }
    }


    public class AcConsideration : Consideration
    {
        //static Dictionary<(UnitEntityData, UnitEntityData), int> uncertainty_values = new Dictionary<(UnitEntityData, UnitEntityData), int>();
        static System.Random rng = new System.Random();

        public float min_score = 0.1f;
        public float max_score = 1.0f;
        public static BlueprintActivatableAbility tumble = Main.library.Get<BlueprintActivatableAbility>("4be5757b85af47545a5789f1d03abda9");

        public override float Score(DecisionContext context)
        {
            float chance_to_hit_weight = Main.settings.ai_chance_to_hit_weight;
            float engaged_by_weight = Main.settings.ai_engaged_by_target_weight;
            float distance_weight = Main.settings.ai_distance_to_target_weight;

            UnitEntityData attacker = context.Unit;
            UnitEntityData target = context.Target.Unit ?? context.Unit;

            if (attacker == null || target == null || !target.IsEnemy(attacker) || attacker.CombatState.AIData.UnreachableUnits.Contains(target) || attacker.IsPlayerFaction)
            {
                return min_score;
            }
            var weapon = attacker.Body?.PrimaryHand?.MaybeWeapon;

            if (!attacker.Body.HandsAreEnabled && attacker.Body.AdditionalLimbs.Count > 0)
            {
                weapon = attacker.Body.AdditionalLimbs[0].MaybeWeapon;
            }
            int odds = 0;
            if (weapon != null)
            {
                var attack_bonus = Rulebook.Trigger(new RuleCalculateAttackBonus(attacker, target, weapon, 0)).Result;
                var target_ac = Rulebook.Trigger(new RuleCalculateAC(attacker, target, weapon.Blueprint.AttackType)).TargetAC;
                odds = attack_bonus - target_ac + 10;  // 0 means 50% chance to hit more is better
            }

            int certainty = 0;
            var unit_part_attack_scores = attacker.Ensure<UnitPartAttackScoreStorage>();

            if (unit_part_attack_scores.attack_scores.ContainsKey(target))
            {
                certainty = unit_part_attack_scores.attack_scores[target];
            }
            else
            {
                var check_bonus = attacker.Stats.Intelligence.Bonus + attacker.Stats.Wisdom.Bonus + attacker.Descriptor.Progression.CharacterLevel;
                var dc = 10 + (target.Stats.CheckBluff.ModifiedValue + target.Stats.SkillStealth.ModifiedValue)/2 + attacker.Descriptor.Progression.CharacterLevel;
                certainty = rng.Next(1, 20) + check_bonus - dc;
                certainty = Math.Max(Math.Min(10, certainty), -10); // from -10 to 10
                
                unit_part_attack_scores.attack_scores[target] = certainty;
            }

            odds = Math.Min(Math.Max(odds, -10), 10); //from -10 to 10
            float certainty_coeff = (1.0f + certainty*0.1f) * 0.5f; //from 0 to 1
            float odds_coeff = (1.0f + odds*0.1f)*0.5f;  //1 for auto hit, 0 for auto miss

            float score = 1.0f - chance_to_hit_weight *  (0.5f + (0.5f - odds_coeff) * certainty_coeff); 

            var distance = Math.Max((target.Position - attacker.Position).magnitude, 5.Feet().Meters);
            var distance_coeff = 5.Feet().Meters / distance; //from 1 to 0;

            if ((weapon != null && weapon.Blueprint.IsRanged) 
                || (context.Ability?.Blueprint != null && context.Ability.Blueprint.Range >= AbilityRange.Close
                    && context.Ability.Blueprint.Range <= AbilityRange.Unlimited)
               )
            {
                distance_coeff = 1.0f;
            }

            score *= (1.0f - distance_weight * (1.0f - distance_coeff)); //will more likely attack closer units if does not have ranged weapon

            var tumble_toggle = attacker.ActivatableAbilities.Enumerable.Where(a => a.Blueprint == tumble).FirstOrDefault();
            if (tumble_toggle != null && !attacker.IsPlayerFaction)
            {
                tumble_toggle.IsOn = attacker.CombatState.IsEngaged && attacker.Stats.GetStat(Kingmaker.EntitySystem.Stats.StatType.SkillMobility).ModifiedValue >= 10;
            }

            var engaged_score = 1.0f - engaged_by_weight;
            if (attacker.CombatState.EngagedBy.Contains(target))
            {
                engaged_score = 1.0f;
            }

            score *= engaged_score;

            return Math.Max(Math.Min(score, max_score), min_score);
        }

        /*public static void clearUncertanityValues(UnitEntityData unit)
        {

            foreach (var k in uncertainty_values.Keys.ToArray())
            {
                if (unit == k.Item1 || unit == k.Item2)
                {
                    uncertainty_values.Remove(k);
                }
            }
            //Main.logger.Log("Stored uncertanities: " + uncertainty_values.Count.ToString());
        }*/
    }


   /* [Harmony12.HarmonyPatch(typeof(UnitEntityData))]
    [Harmony12.HarmonyPatch("LeaveCombat", Harmony12.MethodType.Normal)]
    class UnitEntityData__LeaveCombat__Patch
    {
        static void Postfix(UnitEntityData __instance)
        {
            AcConsideration.clearUncertanityValues(__instance);
        }
    }*/
}
