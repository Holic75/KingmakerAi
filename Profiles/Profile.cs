using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI.Profiles
{
    public class Profile
    {
        AddClassLevels acl;
        BlueprintFeature learn_spells_feature;
        public string name { get; }

        public BlueprintBrain brain { get; }


        List<BlueprintComponent>[] component_arrays = new List<BlueprintComponent>[21];

        BlueprintFeature[] features = new BlueprintFeature[21];

        
        public Profile(string profile_name, BlueprintCharacterClass character_class, StatType stat, 
                       StatType[] skills, BlueprintArchetype archetype = null )
        {
            name = profile_name;

            acl = Helpers.Create<AddClassLevels>();
            acl.CharacterClass = character_class;
            if (archetype != null)
            {
                acl.Archetypes = new BlueprintArchetype[] { archetype };
            }
            acl.RaceStat = stat;
            acl.LevelsStat = stat;
            acl.MemorizeSpells = new BlueprintAbility[0];
            acl.SelectSpells = new BlueprintAbility[0];

            acl.Skills = skills;
            acl.Selections = new SelectionEntry[0];

            brain = Main.library.CopyAndAdd<BlueprintBrain>("bf90f2053c06375418c119115122ae3d", name + "Brain", "");
            brain.Actions = new BlueprintAiAction[0];
        }


        public void addFeatureComponent(int level, params BlueprintComponent[] components)
        {
            if (component_arrays[level] == null)
            {
                component_arrays[level] = new List<BlueprintComponent>();
            }

            component_arrays[level].AddRange(components);
        }



        public BlueprintFeature[] getFeatures(int level)
        {
            List<BlueprintFeature> all_features = new List<BlueprintFeature>();

            var ls = getLearnSpellsFeature();
            if (ls != null)
            {
                all_features.Add(ls);
            }

            for (int i = 0; i <= level; i++)
            {
                var f = getFeatureAtLevel(i);
                if (f != null)
                {
                    all_features.Add(f);
                }
            }

            return all_features.ToArray();
        }


        BlueprintFeature getFeatureAtLevel(int level)
        {
            if (component_arrays[level] == null)
            {
                return null;
            }


            if (features[level] == null)
            {
                features[level] = Helpers.CreateFeature(name + level.ToString() + "LvlFeature",
                                                       "",
                                                       "",
                                                       "",
                                                       null,
                                                       FeatureGroup.None,
                                                       component_arrays[level].ToArray()  
                                                       );
            }
            return features[level];
        }




        public void setAiActions(params BlueprintAiAction[] actions)
        {
            brain.Actions = actions;
        }

        public AddClassLevels getAcl(int levels)
        {
            return acl.CreateCopy(a => a.Levels = levels);
        }


        public void addSelectedSpells(params BlueprintAbility[] spells)
        {
            acl.SelectSpells = spells.Distinct().ToArray();
        }


        public void addMemorizedSpells(params BlueprintAbility[] spells)
        {
            acl.MemorizeSpells = spells.Distinct().ToArray();
        }


        public void  addParametrizedFeatureSelection(BlueprintParametrizedFeature feature, SpellSchool school)
        {
            var spell_focus = new SelectionEntry();
            spell_focus.IsParametrizedFeature = true;
            spell_focus.ParametrizedFeature = feature;
            spell_focus.ParamSpellSchool = school;
            acl.Selections = acl.Selections.AddToArray(spell_focus);
        }


        public void addParametrizedFeatureSelection(BlueprintParametrizedFeature feature, WeaponCategory weapon_category)
        {
            var weapon_focus = new SelectionEntry();
            weapon_focus.IsParametrizedFeature = true;
            weapon_focus.ParametrizedFeature = feature;
            weapon_focus.ParamWeaponCategory = weapon_category;

            acl.Selections = acl.Selections.AddToArray(weapon_focus);
        }

        public void addFeatureSelection(BlueprintFeatureSelection feature_selection, BlueprintFeature feature)
        {
            var selections = acl.Selections;

            var existing_selction = selections.FirstOrDefault(s => s.Selection == feature_selection);

            if (existing_selction == null)
            {
                existing_selction = new SelectionEntry();
                existing_selction.Selection = feature_selection;
                existing_selction.Features = new BlueprintFeature[0];
                acl.Selections = acl.Selections.AddToArray(existing_selction);
            }

            existing_selction.Features = existing_selction.Features.AddToArray(feature);
        }


        static public LearnSpells createLearnSpells(BlueprintCharacterClass character_class, params BlueprintAbility[] spells)
        {
            return Helpers.Create<LearnSpells>(a =>
            {
                a.CharacterClass = character_class;
                a.Spells = spells;
            });
        }

        public BlueprintFeature getLearnSpellsFeature()
        {
            if (acl.MemorizeSpells.Empty())
            {
                return null;
            }
            if (learn_spells_feature == null)
            {
                var ls = Helpers.Create<LearnSpells>(a =>
                                                    {
                                                        a.CharacterClass = acl.CharacterClass;
                                                        a.Spells = acl.MemorizeSpells;
                                                    });
                learn_spells_feature = Helpers.CreateFeature(name + "LearnSpellsFeature",
                                                             "",
                                                             "",
                                                             "",
                                                             null,
                                                             FeatureGroup.None,
                                                             ls
                                                             );
            }
            learn_spells_feature.HideInCharacterSheetAndLevelUp = true;
            return learn_spells_feature;
        }


    }
}
