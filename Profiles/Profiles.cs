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
                                       Spells.fiery_body, Spells.polar_midnight, CallOfTheWild.NewSpells.meteor_swarm, CallOfTheWild.NewSpells.meteor_swarm);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.enlarge_person, 2, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.foxs_cunning, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //3
                getAoeAiSpell(Spells.haste, 54, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.slow, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[0]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[1]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[2]),
                //4
                getSelfSpell(Spells.elemental_body1, 5, is_precast: true, variant: Spells.elemental_body1.Variants[2], extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.fiery_body) }, combat_count: 1),
                getAoeAiSpell(Spells.obsidian_flow, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.rigor_mortis, 5, is_ally: false),
                //5
                getAoeAiSpell(Spells.fire_snake, 6, is_ally: false, affects_allies: false),
                getSingleTargetAiSpell(Spells.baleful_polymorph, 6, is_ally: false),
                //6
                getAoeAiSpell(Spells.tar_pool, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.bears_endurance_mass, 7, is_ally: true, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.chain_lightning, 7, is_ally: false),
                //7
                getAoeAiSpell(CallOfTheWild.NewSpells.fly_mass, 8, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.particulate_form, 8.5f, is_ally: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.resonating_word, 8, is_ally: false),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                //9
                getSelfSpell(Spells.fiery_body, 10, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.polar_midnight, 10.5f, is_ally: false, affects_allies: true),
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
                 Helpers.Create<AutoMetamagic>(a => a.Abilities = new BlueprintAbility[] { Spells.haste }.ToList())
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
                                       Spells.summon_monster9, Spells.clashing_rocks, Spells.clashing_rocks, Spells.tsunami, Spells.tsunami);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.grease, 2.5f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.create_pit, 3.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.glitter_dust, 3.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.acid_arrow, 3, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                //3
                getAoeAiSpell(Spells.spiked_pit, 4.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                //4
                getAoeAiSpell(Spells.acid_pit, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.controlled_fireball, 5f, is_ally: false, affects_allies: false),
                //5
                getAoeAiSpell(Spells.hungry_pit, 6.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.acidic_spray, 6f, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.summon_monster5, 6f, variant: Spells.summon_monster5_d3, combat_count: 1),
                //6
                getAoeAiSpell(Spells.acid_fog, 7, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.chains_of_light, 7, is_ally: false, combat_count: 2),
                getSingleTargetAiSpell(Spells.chain_lightning, 7, is_ally: false),
                //7
                getSelfSpell(Spells.summon_monster7, 8f, variant: Spells.summon_monster7_d3, is_precast: true, combat_count: 1),
                //8
                getSelfSpell(Spells.summon_monster8, 9f, variant: Spells.summon_monster8_d3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.sea_mantle, 9, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.rift_of_ruin, 9.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.incendiary_cloud, 9.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                //9
                getSelfSpell(Spells.summon_monster9, 10f, variant: Spells.summon_monster9_d3, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.clashing_rocks, 10, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.tsunami, 10, is_ally: false, affects_allies: true)
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


        static void createNecromancerProfile()
        {
            var profile = new Profile("WizardNecromancer",
                                      Classes.wizard,
                                      StatType.Intelligence,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.mage_armor, Spells.cause_fear, Spells.cause_fear, Spells.magic_missile, Spells.magic_missile,
                                       Spells.scare, Spells.mirror_image, Spells.false_life, Spells.blindness, Spells.blindness, Spells.blindness,
                                       CallOfTheWild.NewSpells.howling_agony, CallOfTheWild.NewSpells.accursed_glare, CallOfTheWild.NewSpells.accursed_glare, Spells.fireball, Spells.fireball,
                                       Spells.fear, Spells.false_life_greater, Spells.enervation, Spells.enervation, Spells.bone_shatter, Spells.bone_shatter,
                                       Spells.waves_of_fatigue, Spells.acidic_spray, Spells.acidic_spray, Spells.cloudkill,
                                       Spells.banshee_blast, Spells.banshee_blast, CallOfTheWild.NewSpells.suffocation, CallOfTheWild.NewSpells.suffocation, CallOfTheWild.NewSpells.suffocation,
                                       Spells.plague_storm, Spells.waves_of_exhaustion, Spells.finger_of_death, Spells.finger_of_death, Spells.finger_of_death, Spells.finger_of_death,
                                       Spells.horrid_wilting, Spells.horrid_wilting, Spells.death_clutch, Spells.death_clutch, Spells.death_clutch,
                                       CallOfTheWild.NewSpells.mass_suffocation, Spells.wail_of_banshee, Spells.energy_drain, Spells.energy_drain, Spells.energy_drain);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.cause_fear, 2, is_ally: false),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.false_life, 3, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.false_life_greater, false) }),
                getAoeAiSpell(Spells.scare, 3.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.blindness, 3, is_ally: false),
                //3
                getAoeAiSpell(CallOfTheWild.NewSpells.howling_agony, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.accursed_glare, 4f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.higher_bab }),
                //4
                getAoeAiSpell(Spells.fear, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSelfSpell(Spells.false_life_greater, 5f, is_precast: true, combat_count: 1), 
                getSingleTargetAiSpell(Spells.enervation, 5, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                getSingleTargetAiSpell(Spells.bone_shatter, 5, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //5
                getAoeAiSpell(Spells.waves_of_fatigue, 6.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.acidic_spray, 6f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.cloudkill, 6f, is_ally: false, affects_allies: true, combat_count: 1),
                //6
                getAoeAiSpell(Spells.banshee_blast, 7, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.suffocation, 7, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //7
                getAoeAiSpell(Spells.plague_storm, 8.5f, is_ally: false, affects_allies: true, variant: Spells.plague_storm.Variants[0], combat_count: 1),
                getAoeAiSpell(Spells.waves_of_exhaustion, 8.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.finger_of_death, 8, is_ally: false),
                //8
                getAoeAiSpell(Spells.horrid_wilting, 9.5f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.death_clutch, 8, is_ally: false),
                //9
                getAoeAiSpell(CallOfTheWild.NewSpells.mass_suffocation, 10.5f, is_ally: false, affects_allies: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_around_enemies_consideration }),
                getAoeAiSpell(Spells.wail_of_banshee, 10.5f, is_ally: false, affects_allies: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_around_enemies_consideration }),
                getSingleTargetAiSpell(Spells.energy_drain, 10, is_ally: false)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mirror_image, Spells.false_life, Spells.false_life_greater
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Conjuration);
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
            profile.addFeatureSelection(FeatSelections.school_specialization, ClassAbilities.necromancy_specialization);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_divination);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_enchantment);
            profile.addFeatureSelection(FeatSelections.arcane_bond_selection, ClassAbilities.hare_familiar);

            registerProfile(profile);
        }


        static void createIllusionistProfile()
        {
            var profile = new Profile("WizardIllusionist",
                                      Classes.wizard,
                                      StatType.Intelligence,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.mage_armor, Spells.color_spray, Spells.magic_missile, Spells.magic_missile, Spells.magic_missile,
                                       Spells.blur, Spells.blur, Spells.mirror_image, Spells.burning_arc, Spells.burning_arc, Spells.burning_arc,
                                       Spells.displacement, Spells.displacement, Spells.fireball, Spells.fireball, Spells.fireball,
                                       Spells.rainbow_pattern, Spells.phantasmal_killer, Spells.phantasmal_killer, Spells.phantasmal_killer, Spells.phantasmal_killer, Spells.phantasmal_killer,
                                       Spells.phantasmal_web, Spells.fire_snake, Spells.fire_snake, Spells.fire_snake,
                                       Spells.phantasmal_putrefaction, Spells.chain_lightning, Spells.chain_lightning, Spells.chain_lightning, Spells.chain_lightning, Spells.chain_lightning,
                                       Spells.prismatic_spray, Spells.prismatic_spray, Spells.prismatic_spray, Spells.prismatic_spray, Spells.prismatic_spray, Spells.prismatic_spray,
                                       Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts,
                                       Spells.weird, Spells.weird, Spells.weird, Spells.weird, Spells.weird, Spells.weird);


            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.color_spray, 2.5f, is_ally: false, affects_allies: true, check_buff: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.blur, 3, is_ally: true, is_precast: true, combat_count: 2),
                getSingleTargetAiSpell(Spells.burning_arc, 3, is_ally: false),
                //3
                getSingleTargetAiSpell(Spells.displacement, 4, is_ally: true, is_precast: true, combat_count: 2),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                //4
                getAoeAiSpell(Spells.rainbow_pattern, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.phantasmal_killer, 5, is_ally: false),
                //5
                getAoeAiSpell(Spells.phantasmal_web, 6.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.fire_snake, 6f, is_ally: false, affects_allies: false),
                //6
                getAoeAiSpell(Spells.phantasmal_putrefaction, 7.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.chain_lightning, 7, is_ally: false),
                //7
                getAoeAiSpell(Spells.prismatic_spray, 8.5f, is_ally: false, affects_allies: true),
                //8
                getAoeAiSpell(Spells.storm_bolts, 9f, is_ally: false, affects_allies: false),
                //9
                getAoeAiSpell(Spells.weird, 10f, is_ally: false, affects_allies: false)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mirror_image, Spells.blur, Spells.displacement
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Illusion);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Illusion);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.quicken_spell); //13
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
            profile.addFeatureSelection(CallOfTheWild.WizardDiscoveries.arcane_discovery, CallOfTheWild.WizardDiscoveries.resilent_illusions);
            profile.addFeatureSelection(FeatSelections.wizard_bonus_feat, FeatSelections.elemental_focus);//15
            profile.addFeatureSelection(FeatSelections.elemental_focus, Feats.elemental_focus_elec);
            //class features
            profile.addFeatureSelection(FeatSelections.school_specialization, ClassAbilities.illusion_specialization);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_divination);
            profile.addFeatureSelection(FeatSelections.opposition_school, ClassAbilities.opposition_necromancy);
            profile.addFeatureSelection(FeatSelections.arcane_bond_selection, ClassAbilities.hare_familiar);

            registerProfile(profile);
        }


        static void createRedDragonSorcerer()
        {
            var profile = new Profile("SorcerorRedDragon",
                                      Classes.sorceror,
                                      StatType.Charisma,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillUseMagicDevice, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.mage_shield, Spells.burning_hands, Spells.magic_missile, Spells.enlarge_person, Spells.reduce_person, //+mage armor
                                       Spells.burning_arc, Spells.mirror_image, Spells.eagles_splendor, Spells.scorching_ray, Spells.blur,// + resist energy
                                       Spells.fireball, Spells.haste, Spells.heroism, Spells.dispel_magic, //+fly
                                       Spells.controlled_fireball, Spells.dragon_breath, Spells.obsidian_flow, Spells.false_life_greater,//+ fear
                                       Spells.fire_snake, Spells.elemental_body2, Spells.stoneskin_communal, Spells.echolocation,//+spell resistance
                                       Spells.sirocco, Spells.cold_ice_strike, Spells.bulls_Strength_mass, //+ form of the dragon
                                       Spells.prismatic_spray, Spells.legendary_proportions, Spells.power_word_blind, //+ form of the dragon II
                                       CallOfTheWild.NewSpells.incendiary_cloud, Spells.summon_elemental_elder, Spells.power_word_stun, // + form of the dragon 3
                                       CallOfTheWild.NewSpells.meteor_swarm, Spells.fiery_body, Spells.heroic_invocation //+ overwhelming presense);
                                       );

            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.mage_shield, 2, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.burning_hands, 2, is_ally: false),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.eagles_splendor, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.blur, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.burning_arc, 3f, is_ally: false),
                //3
                getAoeAiSpell(Spells.haste, 54, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.heroism, 4f, is_precast: true, is_ally: true, combat_count: 2, extra_target_consideration: new Consideration[] { Considerations.higher_bab }),
                //4
                getAoeAiSpell(Spells.controlled_fireball, 5f, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.obsidian_flow, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.dragon_breath, 5f, is_ally: false, affects_allies: true, variant: Spells.dragon_breath.Variants[3]), //gold cone
                getAoeAiSpell(Spells.dragon_breath, 5f, is_ally: false, affects_allies: true, variant: Spells.dragon_breath.Variants[5]), //brass line
                getSelfSpell(Spells.false_life_greater, 5f, is_precast: true, combat_count: 1), //to cast after false life
                //5                                                         
                getAoeAiSpell(Spells.fire_snake, 6, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.stoneskin_communal, 6f, is_precast: true, is_ally: true, combat_count: 1),
                getSelfSpell(Spells.echolocation, 6f, is_precast: true, combat_count: 1),
                //elemental
                //6
                getAoeAiSpell(Spells.sirocco, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.bulls_Strength_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.cold_ice_strike, 57f, is_ally: true, extra_actor_consideration: new Consideration[] { Considerations.no_standard_action, Considerations.swift_action_available }),
                getSelfSpell(Spells.form_of_the_dragon1, 7f, is_precast: true, combat_count: 1, variant: Spells.form_of_the_dragon1.Variants[3], extra_actor_consideration: new Consideration[] { Considerations.not_polymorphed }),
                //dragon
                //7
                getSingleTargetAiSpell(Spells.legendary_proportions, 8, is_precast: true, is_ally: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getAoeAiSpell(Spells.prismatic_spray, 8, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.power_word_blind, 8f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.higher_bab }),
                getSelfSpell(Spells.form_of_the_dragon2, 8f, is_precast: true, combat_count: 1, variant: Spells.form_of_the_dragon2.Variants[3], extra_actor_consideration: new Consideration[] { Considerations.not_polymorphed }),
                //dragon

                //8
                getAoeAiSpell(CallOfTheWild.NewSpells.incendiary_cloud, 9.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSelfSpell(Spells.summon_elemental_elder, 9f, is_precast: true, combat_count: 1, variant: Spells.summon_elemental_elder.Variants[2]),//fire elemental
                getSingleTargetAiSpell(Spells.power_word_stun, 9f, is_ally: false),
                getSelfSpell(Spells.form_of_the_dragon3, 9f, is_precast: true, combat_count: 1, variant: Spells.form_of_the_dragon3.Variants[3], extra_actor_consideration: new Consideration[] { Considerations.not_polymorphed }),
                //dragon
                //9
                getAoeAiSpell(CallOfTheWild.NewSpells.meteor_swarm, 10f, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.fiery_body, 10f, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.heroic_invocation, 10f, is_precast: true, is_ally: true, combat_count: 1, extra_target_consideration: new Consideration[] { Considerations.higher_bab })
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mage_shield, Spells.mirror_image, Spells.blur, Spells.false_life_greater,
                Spells.eagles_splendor, Spells.bulls_Strength_mass, Spells.heroism, Spells.heroic_invocation, Spells.stoneskin_communal, Spells.summon_elemental_elder,
                Spells.fiery_body, Spells.legendary_proportions, Spells.form_of_the_dragon1, Spells.form_of_the_dragon2, Spells.form_of_the_dragon3
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );
            profile.addFeatureComponent(13,
                                       Helpers.Create<AutoMetamagic>(a => a.Abilities = new BlueprintAbility[] { Spells.haste }.ToList())
            );
            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //1
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, CallOfTheWild.NewFeats.mages_tattoo); //5
            profile.addParametrizedFeatureSelection(CallOfTheWild.NewFeats.mages_tattoo, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21


            profile.addFeatureSelection(FeatSelections.bloodlines, Bloodlines.red_dragon);
            profile.addFeatureSelection(Bloodlines.red_dragon_claws, CallOfTheWild.BloodlinesFix.blood_havoc);
            profile.addFeatureSelection(Bloodlines.red_dragon_resistance_selection, Bloodlines.red_dragon_resistance);

            //bonus feat
            profile.addFeatureSelection(FeatSelections.sorcerer_feat, FeatSelections.elemental_focus);//1
            profile.addFeatureSelection(FeatSelections.elemental_focus, Feats.elemental_focus_fire);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_draconic_feat_selection);//7
            profile.addFeatureSelection(Bloodlines.bloodline_draconic_feat_selection, Feats.toughness);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_draconic_feat_selection);//13
            profile.addFeatureSelection(Bloodlines.bloodline_draconic_feat_selection, Feats.quicken_spell);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_draconic_feat_selection);//19
            profile.addFeatureSelection(Bloodlines.bloodline_draconic_feat_selection, Feats.great_fortitude);


            registerProfile(profile);
        }
        //add undead bloodline sorcerer

        static void createUndeadSorcerer()
        {
            var profile = new Profile("SorcererUndead",
                                      Classes.sorceror,
                                      StatType.Charisma,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillUseMagicDevice, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.mage_shield, Spells.cause_fear, Spells.magic_missile, Spells.enlarge_person, Spells.mage_shield, //+chill touch
                                       Spells.scare, Spells.bone_shatter, Spells.mirror_image, Spells.eagles_splendor, Spells.blur,// + false life
                                       CallOfTheWild.NewSpells.howling_agony, Spells.slow, CallOfTheWild.NewSpells.accursed_glare, Spells.dispel_magic, //+vampyric touch
                                       Spells.fear, Spells.bone_shatter, Spells.false_life_greater, Spells.enervation,//+ animate dead
                                       Spells.acidic_spray, Spells.hungry_pit, Spells.stoneskin_communal,//+waves of fatigue
                                       Spells.banshee_blast, CallOfTheWild.NewSpells.suffocation, Spells.bears_endurance_mass, //+ undeath to death
                                       Spells.plague_storm, Spells.waves_of_exhaustion, Spells.create_undead, //+ finger of death
                                       Spells.death_clutch, Spells.frightful_aspect, Spells.power_word_stun,// + horrid wilting
                                       CallOfTheWild.NewSpells.mass_suffocation, Spells.wail_of_banshee //+ energy drain
                                       );

            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.mage_shield, 2, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.cause_fear, 2, is_ally: false),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.eagles_splendor, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.blur, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.false_life, 3, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.false_life_greater, false) }),
                getAoeAiSpell(Spells.scare, 3.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.bone_shatter, 3f, is_ally: false),
                //3
                getAoeAiSpell(CallOfTheWild.NewSpells.howling_agony, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.slow, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.accursed_glare, 4f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.higher_bab }),
                //4
                getAoeAiSpell(Spells.fear, 5.5f, is_ally: false, affects_allies: true),
                getSingleTargetAiSpell(Spells.bone_shatter, 5f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                getSingleTargetAiSpell(Spells.enervation, 5f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                getSelfSpell(Spells.false_life_greater, 5f, is_precast: true, combat_count: 1),
                 //5  
                getAoeAiSpell(Spells.waves_of_fatigue, 6.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.acidic_spray, 6, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.stoneskin_communal, 6f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.hungry_pit, 6.5f, is_ally: false, affects_allies: true, combat_count: 1),
                //elemental
                //6
                getAoeAiSpell(Spells.banshee_blast, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.bears_endurance_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                getSingleTargetAiSpell(CallOfTheWild.NewSpells.suffocation, 7f, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //dragon
                //7
                getAoeAiSpell(Spells.plague_storm, 8.5f, is_ally: false, affects_allies: true, variant: Spells.plague_storm.Variants[0], combat_count: 1),
                getAoeAiSpell(Spells.waves_of_exhaustion, 8.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.finger_of_death, 8, is_ally: false),
                getSelfSpell(Spells.create_undead, 8f, is_precast: true, combat_count: 1, variant: Spells.create_undead.Variants[0]),
                //8
                getAoeAiSpell(Spells.horrid_wilting, 9.5f, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.frightful_aspect, 9f, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.death_clutch, 8, is_ally: false),
                //9
                getAoeAiSpell(CallOfTheWild.NewSpells.mass_suffocation, 10.5f, is_ally: false, affects_allies: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_around_enemies_consideration }),
                getAoeAiSpell(Spells.wail_of_banshee, 10.5f, is_ally: false, affects_allies: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_around_enemies_consideration }),
                getSingleTargetAiSpell(Spells.energy_drain, 10, is_ally: false)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mage_shield, Spells.false_life, Spells.mirror_image, Spells.blur, Spells.false_life_greater,
                Spells.eagles_splendor, Spells.bears_endurance_mass, Spells.create_undead, Spells.stoneskin_communal,
                Spells.frightful_aspect
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //1
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21


            profile.addFeatureSelection(FeatSelections.bloodlines, Bloodlines.undead);
            profile.addFeatureSelection(Bloodlines.undead_grave_touch, CallOfTheWild.BloodlinesFix.bloodline_familiar);
            profile.addFeatureSelection(CallOfTheWild.BloodlinesFix.bloodline_familiar, ClassAbilities.hare_familiar);
            profile.addFeatureSelection(Bloodlines.deaths_gift_selection, Bloodlines.deaths_gift);

            //bonus feat
            profile.addFeatureSelection(FeatSelections.sorcerer_feat, Feats.quicken_spell);//1
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_undead_feat_selection);//7
            profile.addFeatureSelection(Bloodlines.bloodline_undead_feat_selection, Feats.toughness);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_undead_feat_selection);//13
            profile.addFeatureSelection(Bloodlines.bloodline_undead_feat_selection, Feats.iron_will);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_draconic_feat_selection);//19
            profile.addFeatureSelection(Bloodlines.bloodline_draconic_feat_selection, Feats.great_fortitude);



            registerProfile(profile);
        }


        static void createDruidProfile()
        {
            var profile = new Profile("DruidCaster",
                                      Classes.druid,
                                      StatType.Wisdom,
                                      new StatType[] { StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.longstrider, Spells.feather_step, Spells.flare_burst,
                                       CallOfTheWild.NewSpells.burst_of_radiance, CallOfTheWild.NewSpells.flurry_of_snowballs, Spells.barkskin, Spells.owls_wisdom, Spells.barkskin, CallOfTheWild.NewSpells.flurry_of_snowballs,
                                       CallOfTheWild.NewSpells.burning_entanglement, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor, Spells.feather_step_mass, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor,
                                       Spells.obsidian_flow, Spells.slowing_mud, Spells.flame_strike, Spells.flame_strike, CallOfTheWild.NewSpells.explosion_of_rot, CallOfTheWild.NewSpells.explosion_of_rot,
                                       Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, Spells.stone_skin, Spells.baleful_polymorph, Spells.baleful_polymorph,
                                       Spells.tar_pool, Spells.sirocco, Spells.stoneskin_communal, Spells.plague_storm, Spells.bears_endurance_mass,
                                       Spells.creeping_doom, Spells.legendary_proportions, Spells.fire_storm, Spells.fire_storm, Spells.fire_storm, Spells.fire_storm,
                                       Spells.frightful_aspect, Spells.sea_mantle, Spells.storm_bolts, Spells.storm_bolts, Spells.storm_bolts,
                                       Spells.fiery_body, Spells.polar_midnight, Spells.summon_elder_worm, Spells.summon_elder_worm);


            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getSelfSpell(Spells.longstrider, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.feather_step, 2, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.flare_burst, 2, is_ally: false, affects_allies: true,  combat_count: 1),
                //2
                getAoeAiSpell(CallOfTheWild.NewSpells.burst_of_radiance, 3.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.flurry_of_snowballs, 3f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.barkskin, 3, is_precast: true, combat_count: 3),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //3
                getAoeAiSpell(Spells.feather_step_mass, 4, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.burning_entanglement, 4.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[0]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[1]),
                getAoeAiSpell(CallOfTheWild.NewSpells.earth_tremor, 4, is_ally: false, affects_allies: true, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[2]),
                //4
                getSelfSpell(Spells.stone_skin, 5, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.obsidian_flow, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.slowing_mud, 5.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.flame_strike, 5, is_ally: false, affects_allies: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.explosion_of_rot, 5, is_ally: false, affects_allies: true),
                //5
                getAoeAiSpell(Spells.fire_snake, 6, is_ally: false, affects_allies: false),
                getSingleTargetAiSpell(Spells.baleful_polymorph, 6, is_ally: false),
                //6
                getAoeAiSpell(Spells.tar_pool, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.sirocco, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.bears_endurance_mass, 7, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.plague_storm, 7.5f, is_ally: false, affects_allies: true, variant: Spells.plague_storm.Variants[0], combat_count: 1),
                getAoeAiSpell(Spells.stoneskin_communal, 7, is_ally: true, is_precast: true, combat_count: 1),
                //7
                getSelfSpell(Spells.creeping_doom, 8, combat_count: 1),
                getSingleTargetAiSpell(Spells.legendary_proportions, 8, is_precast: true, is_ally: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getAoeAiSpell(Spells.fire_storm, 8.0f, is_ally: false, affects_allies: false),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.sea_mantle, 9, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                //9
                getSelfSpell(Spells.fiery_body, 10, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.polar_midnight, 10.5f, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.summon_elder_worm, 10.0f, is_precast: true, combat_count: 2)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.longstrider, Spells.feather_step, Spells.bulls_strength, Spells.mirror_image, Spells.owls_wisdom, Spells.cats_grace, Spells.feather_step_mass, Spells.bears_endurance_mass,
                Spells.stone_skin, Spells.stoneskin_communal,  Spells.summon_elder_worm, Spells.sea_mantle, Spells.frightful_aspect, Spells.fiery_body
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells)
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.quicken_spell); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Transmutation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21
           //animal companion can be added separately

            registerProfile(profile);
        }
    }
}
