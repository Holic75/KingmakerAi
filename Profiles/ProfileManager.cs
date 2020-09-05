using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI.Profiles
{
    public partial class ProfileManager
    {
        static LibraryScriptableObject library = Main.library;
        //will manage creation of profiles
        //like wizard transmuter, necromancer, illusionist, enchanter, etc
        //different sorcerers (blaster, enchanter, 
        //cleric melee, cleric negative, cleric postiive
        //bard
        //alchemist
        //druid
        //kineticist

        //should provide corresponding brain and add_class_levels and other components for specified level
        //brain should be the same for all levels
        //add_class_levels too, only level will change

        //so getProfile("WizardTransmuter", 9) should return
        //brain (from the pool)
        //list of components (including spell_list and add_class_levels)
        //
        //it might make sense to define profile in some text file
        //
        //
        //selections:
        //either feature selection/ feat
        //or parametrized feature/ parameter
        //we will need to purge all basic/combat feat selection in advance
        //
        //abilities to use and priorities, considerations, (add consideration for no buffs), combat count
        //will need to search considerations base for existing buffs and pick one if it already exists
        //considerations:  aoe_everyone, aoe_enemy_only, aoe_no_buffs_within_radius, 
        //
        //so for every spell we will need to create action with priority (standard prirty is 1 + spell_level, we can add additional boost to control spells, like 0.5,
        //so we will have higher priority spells and lower priority spells
        //precasts will have all priority of 100
        //swift action abilities should check for swift actions available
        //move action abilities should check for no standard action available



        //example of profile creation
        //createProfile("WizardEvoker",
        //              class, archetype, level stat,
        //              
        //              new Selections({Selection Feature, SelectedFeature} pairs)
        //              new ParametrizedSelections({ParametrizedFeature, Param})
        //              new Spells(SpellEntry({spell, considerations, priority, count})
        //              new AbilitiesUses({ability, considerations, count})
        //              whether to get spells from selections 
        //              or from fact (in this case corresponding fact will be created)
        //             )
        //
        //
        //
        //then we can fetch this profile by getProfile("ProfileName", level)
        //it will return feature list and fact
        //it will be also possible to fetch multiclass profiles, but frankly it is not necessary


        static Dictionary<BlueprintBuff, BuffConsideration> no_buff_consideration = new Dictionary<BlueprintBuff, BuffConsideration>();
        static Dictionary<BlueprintBuff, BuffsAroundConsideration> no_buffs_around_consideration = new Dictionary<BlueprintBuff, BuffsAroundConsideration>();
        static Dictionary<BlueprintBuff, BuffsAroundConsideration> no_buffs_around_enemy_consideration = new Dictionary<BlueprintBuff, BuffsAroundConsideration>();
        static Dictionary<string, Profile> profiles = new Dictionary<string, Profile>();
        static Dictionary<string, BlueprintAiCastSpell> cast_spell_actions = new Dictionary<string, BlueprintAiCastSpell>();


        static public void registerProfile(Profile profile)
        {
            profiles[profile.name] = profile;
        }


        static public Profile getProfile(string name)
        {
            return profiles[name];
        }


        static public BlueprintAiCastSpell getAoeAiSpell(BlueprintAbility spell, float score, bool is_ally, bool affects_allies = true, BlueprintAbility variant = null, bool is_precast = false, BlueprintBuff checked_buff = null,
                                                 bool stand_still = true, bool ignore_unconscious = true, bool check_buff = true,
                                                 Consideration[] extra_actor_consideration = null, Consideration[] extra_target_consideration = null,
                                                 int combat_count = 0, int cooldown_rounds = 0, string custom_name = "")
        {
            string name = (is_precast ? "Precast" : "") + "Aoe" + spell.name + (variant == null ? "" : $"{variant.name}") + score.ToString().Replace('.', '_');

            var ability = variant ?? spell;
            if (!cast_spell_actions.ContainsKey(name))
            {
                List<Consideration> target_considerations = new List<Consideration>();
                if (is_ally)
                {
                    target_considerations.Add(Considerations.choose_more_friends);
                }
                else
                {
                    if (affects_allies)
                    {
                        target_considerations.AddRange(Considerations.harmful_enemy_ally_aoe_target_consideration);
                    }
                    else
                    {
                        target_considerations.AddRange(Considerations.harmful_enemy_aoe_target_consideration);
                    }
                }

                if (stand_still)
                {
                    if (is_precast)
                    {
                        target_considerations.Add(Considerations.in_range);
                    }
                    else
                    {
                        target_considerations.Add(Considerations.stand_still);
                    }
                }
                if (ignore_unconscious)
                {
                    target_considerations.Add(Considerations.unconcious_penalty);
                }

                if (check_buff)
                {
                    var no_buff = getNoBuffFromSpell(ability, !is_ally, checked_buff);

                    if (no_buff != null)
                    {
                        target_considerations.Add(no_buff);
                    }
                }

                if (extra_target_consideration != null)
                {
                    target_considerations.AddRange(extra_target_consideration);
                }

                List<Consideration> actor_considerations = new List<Consideration>();
                if (extra_actor_consideration != null)
                {
                    actor_considerations.AddRange(extra_actor_consideration);
                }

                cast_spell_actions[name] = createCastSpellAction(name + "AiCastSpellAction", spell, actor_considerations.ToArray(), target_considerations.ToArray(),
                                                                 is_precast ? precast_boost + score + aoe_boost: score + aoe_boost, variant, combat_count, cooldown_rounds,
                                                                 "");
            }

            return cast_spell_actions[name];
        }



        static public BlueprintAiCastSpell getSingleTargetAiSpell(BlueprintAbility spell, float score, bool is_ally, BlueprintAbility variant = null, bool is_precast = false, BlueprintBuff checked_buff = null,
                                                         bool stand_still = true, bool ignore_unconscious = true, bool check_buff = true,
                                                         Consideration[] extra_actor_consideration = null, Consideration[] extra_target_consideration = null,
                                                         int combat_count = 0, int cooldown_rounds = 0)
        {
            string name = (is_precast ? "Precast" : "") + "SingleTarget" + spell.name + (variant == null ? "" : $"{variant.name}") + score.ToString().Replace('.', '_');

            var ability = variant ?? spell;
            if (!cast_spell_actions.ContainsKey(name))
            {
                List<Consideration> target_considerations = new List<Consideration>();
                if (is_ally)
                {
                    target_considerations.Add(Considerations.is_ally);
                }
                else
                {
                    target_considerations.Add(Considerations.is_enemy);
                }

                if (stand_still)
                {
                    if (is_precast)
                    {
                        target_considerations.Add(Considerations.in_range);
                    }
                    else
                    {
                        target_considerations.Add(Considerations.stand_still);
                    }
                }
                if (ignore_unconscious)
                {
                    target_considerations.Add(Considerations.unconcious_penalty);
                }

                if (check_buff)
                {
                    var no_buff = getNoBuffFromSpell(ability, !is_ally, checked_buff);
                    if (no_buff != null)
                    {
                        target_considerations.Add(no_buff);
                    }
                }

                if (extra_target_consideration != null)
                {
                    target_considerations.AddRange(extra_target_consideration);
                }

                List<Consideration> actor_considerations = new List<Consideration>();
                if (extra_actor_consideration != null)
                {
                    actor_considerations.AddRange(extra_actor_consideration);
                }

                cast_spell_actions[name] = createCastSpellAction(name + "AiCastSpellAction", spell, actor_considerations.ToArray(), target_considerations.ToArray(),
                                                                 is_precast ? precast_boost + score : score, variant, combat_count, cooldown_rounds,
                                                                 "");
            }

            return cast_spell_actions[name];
        }


        static public BlueprintAiCastSpell getSelfSpell(BlueprintAbility spell, float score, BlueprintAbility variant = null, bool is_precast = false, BlueprintBuff checked_buff = null,
                                                 bool check_buff = true,
                                                 Consideration[] extra_actor_consideration = null, Consideration[] extra_target_consideration = null,
                                                 int combat_count = 0, int cooldown_rounds = 0)
        {
            string name = (is_precast ? "Precast" : "") + "Self" + spell.name + (variant == null ? "" : $"{variant.name}") + score.ToString().Replace('.', '_');

            var ability = variant ?? spell;
            if (!cast_spell_actions.ContainsKey(name))
            {
                List<Consideration> target_considerations = new List<Consideration>();
                target_considerations.Add(Considerations.target_self_consideration);

                if (check_buff)
                {
                    var no_buff = getNoBuffFromSpell(ability, false, checked_buff);
                    if (no_buff != null)
                    {
                        target_considerations.Add(no_buff);
                    }
                }

                if (extra_target_consideration != null)
                {
                    target_considerations.AddRange(extra_target_consideration);
                }

                List<Consideration> actor_considerations = new List<Consideration>();
                if (extra_actor_consideration != null)
                {
                    actor_considerations.AddRange(extra_actor_consideration);
                }

                cast_spell_actions[name] = createCastSpellAction(name + "AiCastSpellAction", spell, actor_considerations.ToArray(), target_considerations.ToArray(),
                                                                 is_precast ? precast_boost + score : score, variant, is_precast ? 1 : combat_count, cooldown_rounds,
                                                                 "");
            }

            return cast_spell_actions[name];
        }


        static BlueprintAiCastSpell createCastSpellAction(string name, BlueprintAbility spell, Consideration[] actor_consideration, Consideration[] target_consideration,
                                                       float base_score = 1f, BlueprintAbility variant = null, int combat_count = 0, int cooldown_rounds = 0, string guid = "")
        {

            var action = CallOfTheWild.Helpers.Create<BlueprintAiCastSpell>();
            action.Ability = spell;
            action.Variant = variant;
            action.ActorConsiderations = actor_consideration;
            action.TargetConsiderations = target_consideration;
            action.name = name;
            action.BaseScore = base_score;
            action.CombatCount = combat_count;
            action.CooldownRounds = cooldown_rounds;
            library.AddAsset(action, guid);

            return action;
        }


        static Consideration getNoBuffFromSpell(BlueprintAbility spell, bool hostile = true, BlueprintBuff specific_buff = null)
        {
            var buff = specific_buff ?? extractBuffFromSpell(spell);
            if (buff == null)
            {
                return null;
            }

            if (spell.HasAreaEffect())
            {
                return hostile ? getNoBuffsAroundEnemiesConsideration(buff) : getNoBuffsAroundonAlliesConsideration(buff);
            }
            else
            {
                return getSingleTargetNoBuff(buff);
            }
        }


        static BuffConsideration getSingleTargetNoBuff(BlueprintBuff buff)
        {
            if (!no_buff_consideration.ContainsKey(buff))
            {
                var c = library.CopyAndAdd<BuffConsideration>("14698bcd142fb924bbb912d57c3fe712", "KingmakerAiNo" + buff.name + "Consideration", "");
                c.Buffs = new BlueprintBuff[] { buff };
                no_buff_consideration[buff] = c;
            }
            return no_buff_consideration[buff];
        }


        static BuffsAroundConsideration getNoBuffsAroundonAlliesConsideration(BlueprintBuff buff)
        {
            if (!no_buffs_around_consideration.ContainsKey(buff))
            {
                var c = library.CopyAndAdd<BuffsAroundConsideration>("b72a7de89582d4d48ab8eda367b2e282", "KingmakerAiNo" + buff.name + "AlliesAroundConsideration", "");
                c.Buffs = new BlueprintBuff[] { buff };
                no_buffs_around_consideration[buff] = c;
                c.UseAbilityShape = true;

            }
            return no_buffs_around_consideration[buff];
        }


        static BuffsAroundConsideration getNoBuffsAroundEnemiesConsideration(BlueprintBuff buff)
        {
            if (!no_buffs_around_enemy_consideration.ContainsKey(buff))
            {
                var c = library.CopyAndAdd<BuffsAroundConsideration>("18705e45d8cdea746aa0ef47a40b58e6", "KingmakerAiNo" + buff.name + "EnemiesAroundConsideration", "");
                c.Buffs = new BlueprintBuff[] { buff };
                no_buffs_around_enemy_consideration[buff] = c;
                c.UseAbilityShape = true;
            }
            return no_buffs_around_enemy_consideration[buff];
        }


        static BlueprintBuff extractBuffFromSpell(BlueprintAbility spell)
        {
            var run_actions = spell.GetComponent<AbilityEffectRunAction>();
            if (run_actions == null)
            {
                return null;
            }

            return CallOfTheWild.Common.extractActions<ContextActionApplyBuff>(run_actions.Actions.Actions).FirstOrDefault()?.Buff;
        }


        static public void initialize()
        {
            initializeDefines();
            createTransmuterProfile();
            createConjurerProfile();
            createNecromancerProfile();
            createIllusionistProfile();
            createRedDragonSorcerer();
            createUndeadSorcerer();
            createDruidProfile();
            createAlchemistProfile();
        }


        static public void replaceAcl(AddClassLevels old_acl, AddClassLevels new_acl)
        {
            old_acl.CharacterClass = new_acl.CharacterClass;
            old_acl.Archetypes = new_acl.Archetypes;
            old_acl.Levels = new_acl.Levels;
            old_acl.MemorizeSpells = new_acl.MemorizeSpells;
            old_acl.RaceStat = new_acl.RaceStat;
            old_acl.Skills = new_acl.Skills;
            old_acl.SelectSpells = new_acl.SelectSpells;
            old_acl.Selections = new_acl.Selections;
        }
    }
}
