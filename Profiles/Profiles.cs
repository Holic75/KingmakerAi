using CallOfTheWild;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI.Profiles
{
    public partial class ProfileManager
    {
        static void createTransmuterProfile()
        {
            var profile = new Profile("WizardTransmuter",
                                      Classes.wizard,
                                      StatType.Intelligence,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.enlarge_person, Spells.enlarge_person, Spells.mage_armor, Spells.magic_missile, Spells.magic_missile, Spells.magic_missile,
                                       Spells.bulls_strength, Spells.bulls_strength, Spells.mirror_image, Spells.foxs_cunning, Spells.cats_grace, Spells.cats_grace,
                                       Spells.haste, Spells.slow, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor,
                                       Spells.elemental_body1, Spells.obsidian_flow, CallOfTheWild.NewSpells.rigor_mortis, CallOfTheWild.NewSpells.rigor_mortis, CallOfTheWild.NewSpells.rigor_mortis, CallOfTheWild.NewSpells.rigor_mortis,
                                       Spells.fire_snake, Spells.baleful_polymorph, Spells.baleful_polymorph, Spells.baleful_polymorph, Spells.baleful_polymorph, Spells.baleful_polymorph,
                                       Spells.tar_pool, Spells.bears_endurance_mass, Spells.chain_lightning, Spells.chain_lightning, Spells.chain_lightning,
                                       CallOfTheWild.NewSpells.fly_mass, CallOfTheWild.NewSpells.particulate_form, Spells.resonating_word, Spells.resonating_word, Spells.resonating_word,
                                       Spells.frightful_aspect, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts,
                                       Spells.fiery_body, CallOfTheWild.NewSpells.meteor_swarm, CallOfTheWild.NewSpells.meteor_swarm, CallOfTheWild.NewSpells.meteor_swarm);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true),
                getSingleTargetAiSpell(Spells.enlarge_person, 2, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                getSelfSpell(Spells.mirror_image, 3, is_precast: true),
                getSelfSpell(Spells.foxs_cunning, 3, is_precast: true),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //3
                getAoeAiSpell(Spells.haste, 54, is_ally: true),
                getAoeAiSpell(Spells.slow, 4.5f, is_ally: false, affects_allies: false),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[0]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[1]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[2]),
                //4
                getSelfSpell(Spells.elemental_body1, 5, is_precast: true, variant: Spells.elemental_body1.Variants[2], extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.fiery_body) }),
                getAoeAiSpell(Spells.obsidian_flow, 5.5f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.rigor_mortis, 5, is_ally: false),
                //5
                getAoeAiSpell(Spells.fire_snake, 6, is_ally: false, affects_allies: false),
                getSingleTargetAiSpell(Spells.baleful_polymorph, 6, is_ally: false),
                //6
                getAoeAiSpell(Spells.tar_pool, 7.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.bears_endurance_mass, 7, is_ally: true, is_precast: true),
                getSingleTargetAiSpell(Spells.chain_lightning, 7, is_ally: false),
                //7
                getAoeAiSpell(CallOfTheWild.NewSpells.fly_mass, 8, is_ally: true, is_precast: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.particulate_form, 8.5f, is_ally: true),
                getSingleTargetAiSpell(Spells.resonating_word, 8, is_ally: false),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                //9
                getSelfSpell(Spells.fiery_body, 10, is_precast: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.meteor_swarm, 10, is_ally: false, affects_allies: true)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.enlarge_person, Spells.bulls_strength, Spells.mirror_image, Spells.foxs_cunning, Spells.cats_grace, Spells.elemental_body1, Spells.bears_endurance_mass,
                CallOfTheWild.NewSpells.fly_mass, Spells.frightful_aspect, Spells.fiery_body
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            profile.addFeatureComponent(13,
                 Helpers.Create<AutoMetamagic>(a => a.Abilities = new BlueprintAbility[] {Spells.haste }.ToList())
            );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.quicken_spell); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21
            //discoveries //1, 5, 10, 15, 20
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, FeatSelections.elemental_focus);//1
            profile.addFeatureSelection(FeatSelections.elemental_focus, Feats.elemental_focus_fire);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, CallOfTheWild.WizardDiscoveries.arcane_discovery);//5
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.forests_blessing);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, CallOfTheWild.WizardDiscoveries.arcane_discovery);//10
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.idealize);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, CallOfTheWild.WizardDiscoveries.arcane_discovery);//15
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.alchemical_affinity);
            //class features
            profile.addFeatureSelection(FeatSelections.school_specialization, ClassAbilities.transmutation_specialization);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_divination);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_necromancy);
            profile.addFeatureSelection(FeatSelections.arcane_bond_selection, ClassAbilities.hare_familiar);

            registerProfile(profile);
        }


        static void createConjurerProfile()
        {
            var profile = new Profile("WizardConjurer",
                                      Classes.wizard,
                                      StatType.Intelligence,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.grease, Spells.mage_armor, Spells.magic_missile, Spells.magic_missile, Spells.magic_missile, Spells.magic_missile,
                                       Spells.create_pit, Spells.glitter_dust, Spells.mirror_image, Spells.acid_arrow, Spells.acid_arrow, Spells.acid_arrow,
                                       Spells.spiked_pit, Spells.fireball, Spells.fireball, Spells.fireball, Spells.fireball, Spells.fireball, Spells.fireball,
                                       Spells.acid_pit, Spells.summon_monster4, Spells.controlled_fireball, Spells.controlled_fireball, Spells.controlled_fireball,
                                       Spells.hungry_pit, Spells.acidic_spray, Spells.acidic_spray, Spells.summon_monster5,
                                       Spells.acid_fog, Spells.chains_of_light, Spells.chains_of_light, Spells.chain_lightning, Spells.chain_lightning,
                                       Spells.summon_monster7,
                                       Spells.summon_monster8, Spells.sea_mantle, Spells.rift_of_ruin, CallOfTheWild.NewSpells.incendiary_cloud, Spells.storm_bolts,
                                       Spells.summon_monster9, Spells.clashing_rocks, Spells.clashing_rocks, CallOfTheWild.NewSpells.meteor_swarm, CallOfTheWild.NewSpells.meteor_swarm);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true),
                getAoeAiSpell(Spells.grease, 2.5f, is_ally: false, affects_allies: true),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true),
                getAoeAiSpell(Spells.create_pit, 3.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.glitter_dust, 3.5f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.acid_arrow, 3, is_ally: false),
                //3
                getAoeAiSpell(Spells.spiked_pit, 4.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                //4
                getAoeAiSpell(Spells.acid_pit, 5.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.controlled_fireball, 5f, is_ally: false, affects_allies: false),
                //5
                getAoeAiSpell(Spells.hungry_pit, 6.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.acidic_spray, 6f, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.summon_monster5, 6f, variant: Spells.summon_monster5_d3),
                //6
                getAoeAiSpell(Spells.acid_fog, 7, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.chains_of_light, 7, is_ally: false),
                getSingleTargetAiSpell(Spells.chain_lightning, 7, is_ally: false),
                //7
                getSelfSpell(Spells.summon_monster7, 8f, variant: Spells.summon_monster7_d3, is_precast: true),
                //8
                getSelfSpell(Spells.summon_monster8, 9f, variant: Spells.summon_monster8_d3, is_precast: true),
                getSelfSpell(Spells.sea_mantle, 9, is_precast: true),
                getAoeAiSpell(Spells.rift_of_ruin, 9, is_ally: false, affects_allies: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.incendiary_cloud, 9, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                //9
                getSelfSpell(Spells.summon_monster9, 10f, variant: Spells.summon_monster9_d3, is_precast: true),
                getAoeAiSpell(Spells.clashing_rocks, 10, is_ally: false, affects_allies: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.meteor_swarm, 10, is_ally: false, affects_allies: true)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mirror_image, Spells.summon_monster5, Spells.summon_monster7, Spells.summon_monster8, Spells.summon_monster9,
                Spells.sea_mantle
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.augment_summoning); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.superior_summoning); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21
            //discoveries //1, 5, 10, 15, 20
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, FeatSelections.elemental_focus);//1
            profile.addFeatureSelection(FeatSelections.elemental_focus, Feats.elemental_focus_acid);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, CallOfTheWild.WizardDiscoveries.arcane_discovery);//5
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.forests_blessing);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, FeatSelections.greater_elemental_focus);//10
            profile.addFeatureSelection(FeatSelections.elemental_focus, Feats.greater_elemental_focus_acid);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, CallOfTheWild.WizardDiscoveries.arcane_discovery);//15
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.alchemical_affinity);
            //class features
            profile.addFeatureSelection(FeatSelections.school_specialization, ClassAbilities.conjuration_specialization);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_divination);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_necromancy);
            profile.addFeatureSelection(FeatSelections.arcane_bond_selection, ClassAbilities.hare_familiar);

            registerProfile(profile);
        }
    }
}
