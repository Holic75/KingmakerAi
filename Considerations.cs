using Kingmaker.Controllers.Brain;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
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
        static System.Random rng = new System.Random();

        public float min_score = 0.1f;
        public float max_score = 1.0f;

        public override float Score(DecisionContext context)
        {
            UnitEntityData attacker = context.Unit;
            UnitEntityData target = context.Target.Unit ?? context.Unit;

            var weapon = attacker.Body?.PrimaryHand?.MaybeWeapon;

            int odds = -10;
            if (weapon != null)
            {
                var attack_bonus = Rulebook.Trigger(new RuleCalculateAttackBonus(attacker, target, weapon, 0)).Result;
                var target_ac = Rulebook.Trigger(new RuleCalculateAC(attacker, target, weapon.Blueprint.AttackType)).TargetAC;
                odds = attack_bonus - target_ac;
            }

            var uncertanity_base = (float)(5 + Math.Max(-4, Math.Max(attacker.Stats.Intelligence.Bonus, attacker.Stats.Wisdom.Bonus)));
            int uncertainty_range = (int)(50.0f / uncertanity_base); //so if bonus is 0, it will be 10, if -4, it will be 50, if +5 it will be + 5
            int uncertainty = 0;
            if (uncertanity_base != 0)
            {
                uncertainty = rng.Next(0, uncertainty_range);
            }

            odds = Math.Min(Math.Max(odds, -20), 0); //from -20 to 0
            odds += uncertainty;
            odds = Math.Min(Math.Max(odds, -20), 0); //from -20 to 0

            var score = (float)(odds + 20) / 20.0f;

            return Math.Max(Math.Min(score, max_score), min_score);
        }
    }
}
