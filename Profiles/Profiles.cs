using CallOfTheWild;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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
                                       Spells.fire_snake, NewSpells.fickle_winds, Spells.baleful_polymorph, Spells.baleful_polymorph, Spells.baleful_polymorph, Spells.baleful_polymorph,
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
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }, combat_count: 2),
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
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
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
                CallOfTheWild.NewSpells.fly_mass, Spells.frightful_aspect, Spells.fiery_body, NewSpells.fickle_winds,
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
                                       Spells.waves_of_fatigue, Spells.acidic_spray, CallOfTheWild.NewSpells.suffocation, CallOfTheWild.NewSpells.suffocation, CallOfTheWild.NewSpells.suffocation,
                                       Spells.banshee_blast, Spells.banshee_blast, Spells.circle_of_death, Spells.circle_of_death, Spells.circle_of_death,
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
                getAoeAiSpell(Spells.banshee_blast, 7.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.circle_of_death, 7.5f, is_ally: false, affects_allies: true, combat_count: 3),
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
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
            var profile = new Profile("SorcererRedDragon",
                                      Classes.sorceror,
                                      StatType.Charisma,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillUseMagicDevice, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.mage_shield, Spells.burning_hands, Spells.magic_missile, Spells.enlarge_person, Spells.reduce_person, //+mage armor
                                       Spells.burning_arc, Spells.mirror_image, Spells.eagles_splendor, Spells.scorching_ray, Spells.blur,// + resist energy
                                       Spells.fireball, Spells.heroism, Spells.dispel_magic, Spells.haste,//+fly
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
                getSingleTargetAiSpell(Spells.heroism, 4, is_precast: true, is_ally: true, combat_count: 2, extra_target_consideration: new Consideration[] {Considerations.higher_bab, getNoBuffFromSpell(Spells.heroism, false, extractBuffFromSpell(Spells.heroism_greater)), getNoBuffFromSpell(Spells.heroism, false, extractBuffFromSpell(Spells.good_hope)) }),
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
                getSingleTargetAiSpell(Spells.heroic_invocation, 10f, is_precast: true, is_ally: true, combat_count: 2, extra_target_consideration: new Consideration[] { Considerations.higher_bab })
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mage_shield, Spells.mirror_image, Spells.blur, Spells.false_life_greater,
                Spells.eagles_splendor, Spells.bulls_Strength_mass, Spells.heroism, Spells.heroic_invocation, Spells.stoneskin_communal, Spells.summon_elemental_elder,
                Spells.fiery_body, Spells.legendary_proportions, Spells.form_of_the_dragon1, Spells.form_of_the_dragon2, Spells.form_of_the_dragon3
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
            profile.addFeatureSelection(Bloodlines.red_dragon_breath_selection, BloodlinesFix.blood_piercing);

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

        static void createUndeadSorcerer()
        {
            var profile = new Profile("SorcererUndead",
                                      Classes.sorceror,
                                      StatType.Charisma,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillUseMagicDevice, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.mage_shield, Spells.cause_fear, Spells.magic_missile, Spells.enlarge_person, Spells.mage_armor, //+chill touch
                                       Spells.scare, Spells.bone_shatter, Spells.mirror_image, Spells.eagles_splendor, Spells.blur,// + false life
                                       CallOfTheWild.NewSpells.howling_agony, Spells.slow, CallOfTheWild.NewSpells.accursed_glare, Spells.dispel_magic, //+vampyric touch
                                       Spells.fear, Spells.false_life_greater, Spells.bone_shatter, Spells.enervation,//+ animate dead
                                       Spells.acidic_spray, CallOfTheWild.NewSpells.suffocation, Spells.hungry_pit, Spells.stoneskin_communal,//+waves of fatigue
                                       Spells.banshee_blast, Spells.circle_of_death, Spells.bears_endurance_mass, //+ undeath to death
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
                getAoeAiSpell(Spells.circle_of_death, 7.5f, is_ally: false, affects_allies: true, combat_count: 3),
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
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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
            profile.addFeatureSelection(Bloodlines.grasp_of_the_dead_selection, Bloodlines.grasp_of_the_dead);

            //bonus feat
            profile.addFeatureSelection(FeatSelections.sorcerer_feat, Feats.quicken_spell);//1
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_undead_feat_selection);//7
            profile.addFeatureSelection(Bloodlines.bloodline_undead_feat_selection, Feats.toughness);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_undead_feat_selection);//13
            profile.addFeatureSelection(Bloodlines.bloodline_undead_feat_selection, Feats.iron_will);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_undead_feat_selection);//19
            profile.addFeatureSelection(Bloodlines.bloodline_undead_feat_selection, Feats.great_fortitude);



            registerProfile(profile);
        }


        static void createFeySorcerer()
        {
            var profile = new Profile("SorcererFey",
                                      Classes.sorceror,
                                      StatType.Charisma,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillUseMagicDevice, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.mage_shield, Spells.sleep, Spells.magic_missile, Spells.enlarge_person, Spells.mage_armor, //+entangle
                                       NewSpells.hypnotic_pattern, Spells.eagles_splendor, Spells.mirror_image, Spells.cats_grace, Spells.blur,// + hideous laughter
                                       Spells.slow, Spells.heroism, Spells.fireball, Spells.haste, Spells.dispel_magic, //+deep slumber
                                       Spells.confusion, Spells.crashing_despair, Spells.phantasmal_killer, Spells.false_life_greater, Spells.controlled_fireball,//+ poison
                                       Spells.dominate_person, Spells.phantasmal_web, Spells.stoneskin_communal, Spells.cloudkill,//+vinetrap
                                       Spells.serenity, Spells.phantasmal_putrefaction, Spells.heroism_greater, Spells.bears_endurance_mass, //+ dispel greater
                                       NewSpells.hold_person_mass, Spells.waves_of_ecstasy, Spells.legendary_proportions, //+ changestaff
                                       Spells.power_word_stun, Spells.storm_bolts, Spells.sea_mantle,// + irresistible dance
                                       Spells.overwhelming_presence, Spells.weird, Spells.heroic_invocation //+ shapechange
                                       );

            profile.setAiActions(AiActions.acid_splash_ai_action,
                //1
                getSelfSpell(Spells.mage_armor, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.mage_shield, 2, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.sleep, 2.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.enlarge_person, 2, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSingleTargetAiSpell(Spells.magic_missile, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.eagles_splendor, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.blur, 3, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.hypnotic_pattern, 3.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.hideous_laughter, 3f, is_ally: false, extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.hideous_laughter, false, extractBuffFromSpell(NewSpells.hypnotic_pattern)) }),
                //3
                getAoeAiSpell(Spells.haste, 54, is_ally: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.heroism, 4, is_precast: true, is_ally: true, combat_count: 2, extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.heroism, false, extractBuffFromSpell(Spells.heroism_greater)), getNoBuffFromSpell(Spells.heroism, false, extractBuffFromSpell(Spells.good_hope)) }),
                getAoeAiSpell(Spells.slow, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.fireball, 4f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.deep_slumber, 4.5f, is_ally: false, affects_allies: false, combat_count: 1),
                //4
                getAoeAiSpell(Spells.confusion, 5.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.crashing_despair, 5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.phantasmal_killer, 5, is_ally: false),
                getAoeAiSpell(Spells.controlled_fireball, 5f, is_ally: false, affects_allies: false),
                getSelfSpell(Spells.false_life_greater, 5f, is_precast: true, combat_count: 1),
                //5  
                getAoeAiSpell(Spells.phantasmal_web, 6.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.dominate_person, 6f, is_ally: false),
                getAoeAiSpell(Spells.stoneskin_communal, 6f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.cloudkill, 6f, is_ally: false, affects_allies: true, combat_count: 1),
                 //6
                getSingleTargetAiSpell(Spells.heroism_greater, 7, is_ally: true, is_precast: true, combat_count: 2),
                getAoeAiSpell(Spells.phantasmal_putrefaction, 7.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.serenity, 7.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.bears_endurance_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                //7
                getSelfSpell(Spells.change_staff, 8f, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.hold_person_mass, 8.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.waves_of_ecstasy, 8.5f, is_ally: false, affects_allies: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.legendary_proportions, 8, is_precast: true, is_ally: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                //8
                getAoeAiSpell(Spells.storm_bolts, 9.5f, is_ally: false, affects_allies: false),
                getSelfSpell(Spells.sea_mantle, 9f, is_precast: true, combat_count: 1),
                //9
                getAoeAiSpell(Spells.overwhelming_presence, 10.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.heroic_invocation, 10f, is_precast: true, is_ally: true, combat_count: 1, extra_target_consideration: new Consideration[] { Considerations.higher_bab }),
                getAoeAiSpell(Spells.weird, 10f, is_ally: false, affects_allies: false),
                getSelfSpell(Wildshape.shapechange, 10f, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.shapechange_ability, 15f, combat_count: 1, variant: Spells.shapechange_ability_silver_dragon_variant)

                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.mage_armor, Spells.mage_shield, Spells.false_life, Spells.mirror_image, Spells.blur, Spells.false_life_greater,
                Spells.eagles_splendor, Spells.bears_endurance_mass, Spells.cats_grace, Spells.stoneskin_communal, Spells.heroism, Spells.heroism_greater, Spells.heroic_invocation,
                Spells.sea_mantle, Spells.change_staff, Wildshape.shapechange, Spells.legendary_proportions, Spells.heroic_invocation
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; } )
                );
            profile.addFeatureComponent(13,
                Helpers.Create<AutoMetamagic>(a => a.Abilities = new BlueprintAbility[] { Spells.haste }.ToList())
            );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //1
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //7
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //11
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Illusion);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //13
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Illusion);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //17
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //21


            profile.addFeatureSelection(FeatSelections.bloodlines, Bloodlines.bloodline_fey);
            profile.addFeatureSelection(Bloodlines.laughing_touch_selection, CallOfTheWild.BloodlinesFix.bloodline_familiar);
            profile.addFeatureSelection(CallOfTheWild.BloodlinesFix.bloodline_familiar, ClassAbilities.hare_familiar);
            profile.addFeatureSelection(Bloodlines.woodland_stride_selection, Bloodlines.woodland_stride);
            profile.addFeatureSelection(Bloodlines.fleeting_glance_selection, Bloodlines.fleeting_glance);

            //bonus feat
            profile.addFeatureSelection(FeatSelections.sorcerer_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_fey_feat_selection);//7
            profile.addFeatureSelection(Bloodlines.bloodline_fey_feat_selection, Feats.dodge);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_fey_feat_selection);//13
            profile.addFeatureSelection(Bloodlines.bloodline_fey_feat_selection, Feats.quicken_spell);
            profile.addFeatureSelection(Bloodlines.bloodline_feat, Bloodlines.bloodline_fey_feat_selection);//19
            profile.addFeatureSelection(Bloodlines.bloodline_fey_feat_selection, Feats.dodge);



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
                                       NewSpells.fickle_winds, Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, Spells.baleful_polymorph, Spells.baleful_polymorph,
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
                getSelfSpell(Spells.owls_wisdom, 3, is_precast: true, combat_count: 1),
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
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
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
                Spells.stone_skin, Spells.stoneskin_communal,  Spells.summon_elder_worm, Spells.sea_mantle, Spells.frightful_aspect, Spells.fiery_body, NewSpells.fickle_winds,
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
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


        static void createMeleeDruidProfile()
        {
            var profile = new Profile("DruidMelee",
                                      Classes.druid,
                                      StatType.Strength,
                                      new StatType[] { StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillLoreReligion });

            profile.addMemorizedSpells(Spells.longstrider, Spells.feather_step, Spells.magic_fang,
                                       Spells.barkskin, Spells.owls_wisdom, Spells.barkskin, Spells.bulls_strength, Spells.cats_grace, Spells.bears_endurance,
                                       Spells.feather_step_mass, Spells.magic_fang_greater, Spells.resist_energy_communal,
                                       CallOfTheWild.NewSpells.strong_jaw, Spells.cape_of_wasps, Spells.thorn_body, Spells.echolocation,
                                       NewSpells.fickle_winds, Spells.stone_skin, Spells.blessing_of_the_salamander,
                                       Spells.bulls_Strength_mass, Spells.cats_grace_mass, Spells.bears_endurance_mass, Spells.stoneskin_communal,
                                       Spells.creeping_doom, Spells.legendary_proportions, Spells.legendary_proportions, Spells.true_seeing,
                                       Spells.frightful_aspect, Spells.sea_mantle, Spells.summon_nature_ally_8,
                                       Spells.fiery_body, Spells.foresight, Spells.summon_elder_worm, Spells.summon_elder_worm);


            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getSelfSpell(Spells.longstrider, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.feather_step, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.magic_fang, 2, is_precast: true, combat_count: 1),
                //2
                getSingleTargetAiSpell(Spells.bears_endurance, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.barkskin, 3, is_precast: true, combat_count: 3),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //3
                getAoeAiSpell(Spells.feather_step_mass, 4, is_precast: true, is_ally: true, combat_count: 1),
                getSelfSpell(Spells.magic_fang_greater, 4, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.resist_energy_communal, 4, is_precast: true, is_ally: true, combat_count: 1, variant: Spells.resist_energy_communal.Variants[3]),
                //4
                getSelfSpell(Spells.stone_skin, 5, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.echolocation, 5, is_precast: true, combat_count: 1),
                getSelfSpell(NewSpells.strong_jaw, 5, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.cape_of_wasps, 5, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.thorn_body, 5, is_precast: true, combat_count: 1),
                //5
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.stone_skin, 5, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.blessing_of_the_salamander, 5, is_precast: true, combat_count: 1),
                //6
                getAoeAiSpell(Spells.bears_endurance_mass, 7, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.stoneskin_communal, 7, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.bulls_Strength_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.cats_grace_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                //7
                getSelfSpell(Spells.creeping_doom, 8, combat_count: 1),
                getSingleTargetAiSpell(Spells.legendary_proportions, 8, is_precast: true, is_ally: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.true_seeing, 8, is_precast: true, combat_count: 1),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.sea_mantle, 9, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.summon_nature_ally_8, 10f, variant: Spells.summon_nature_ally_8.Variants[1], is_precast: true, combat_count: 1),
                //9
                getSelfSpell(Spells.fiery_body, 10, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.foresight, 10, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.summon_elder_worm, 10, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.summon_elder_worm, 10, is_precast: true, combat_count: 1),


                //
                getSelfSpell(Wildshapes.leopard, 21, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] {Considerations.not_polymorphed }),
                getSelfSpell(Wildshapes.bear, 23, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] { Considerations.not_polymorphed }),
                getSelfSpell(Wildshapes.smilodon, 25, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] { Considerations.not_polymorphed }),
                getSelfSpell(Wildshapes.giant_flytrap, 27, is_precast: true, combat_count: 1, extra_target_consideration: new Consideration[] { Considerations.not_polymorphed })
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.longstrider, Spells.feather_step, Spells.bulls_strength, Spells.bears_endurance, Spells.owls_wisdom, Spells.cats_grace, Spells.feather_step_mass, Spells.bears_endurance_mass,
                Spells.stone_skin, Spells.stoneskin_communal,  Spells.summon_elder_worm, Spells.summon_nature_ally_8, Spells.sea_mantle, Spells.frightful_aspect, Spells.fiery_body,
                Spells.foresight, Spells.true_seeing, Spells.cats_grace_mass, Spells.bulls_Strength_mass, Spells.blessing_of_the_salamander, NewSpells.fickle_winds, Spells.echolocation,
                Spells.thorn_body, Spells.cape_of_wasps,
                Wildshapes.leopard, Wildshapes.bear, Wildshapes.smilodon, Wildshapes.giant_flytrap
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.natural_spell); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.iron_will); //7
            profile.addFeatureSelection(FeatSelections.basic_feat, Wildshape.mutated_shape); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.quicken_spell); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.scales_and_skin); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.great_fortitude); //19
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.lightning_reflexes); //21
            //animal companion can be added separately

            registerProfile(profile);
        }

        static void createBardProfile()
        {
            var profile = new Profile("BardArcher",
                                      Classes.bard,
                                      StatType.Dexterity,
                                      new StatType[] { StatType.SkillPersuasion, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillThievery, StatType.SkillPersuasion });

            profile.addSelectedSpells(Spells.grease, Spells.remove_fear, Spells.hideous_laughter, Spells.ear_piercing_scream, Spells.feather_step,
                                       Spells.heroism, NewSpells.blistering_invective, Spells.eagles_splendor, Spells.sound_burst, Spells.cats_grace, 
                                       Spells.haste, Spells.good_hope, Spells.confusion, Spells.thudnering_drums, Spells.feather_step_mass,
                                       Spells.echolocation, Spells.dominate_person, Spells.see_invisibility_communal, Spells.shout, Spells.freedom_of_movement,
                                       Spells.cacaphonous_call_mass, Spells.heroism_greater, Spells.resonating_word, Spells.cloak_of_dreams, Spells.mind_fog,
                                       Spells.overwhelming_presence, Spells.waves_of_ecstasy, Spells.shout_greater, Spells.cats_grace_mass
                                      );


            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getAoeAiSpell(Spells.grease, 2.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.remove_fear, 2f, is_ally: true, combat_count: 1, is_precast: true),
                getSingleTargetAiSpell(Spells.hideous_laughter, 2, is_ally: false),
                //2
                getSingleTargetAiSpell(Spells.heroism, 4, is_precast: true, is_ally: true, combat_count: 2),
                getSelfSpell(Spells.eagles_splendor, 3, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.blistering_invective, 3.5f, is_ally: false, combat_count: 1),
                getAoeAiSpell(Spells.sound_burst, 3, is_ally: false, affects_allies: true),
                //3
                getAoeAiSpell(Spells.haste, 4.5f, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.good_hope, 4f, is_ally: true, combat_count: 1, is_precast: true),
                getAoeAiSpell(Spells.feather_step_mass, 4f, is_ally: true, combat_count: 1, is_precast: true),
                getAoeAiSpell(Spells.confusion, 4.9f, is_ally: false, affects_allies: true, combat_count: 1),
                getAoeAiSpell(Spells.thudnering_drums, 4.5f, is_ally: false, affects_allies: true),
                //4
                getSelfSpell(Spells.echolocation, 5, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.dominate_person, 5, is_ally: false),
                getAoeAiSpell(Spells.good_hope, 4.5f, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.shout, 5.5f, is_ally: false, affects_allies: true),
                getAoeAiSpell(Spells.see_invisibility_communal, 5f, is_ally: true, combat_count: 1, is_precast: true),
                //5
                getSingleTargetAiSpell(Spells.heroism_greater, 6, is_ally: true, is_precast: true, combat_count: 2),
                getAoeAiSpell(Spells.cacaphonous_call_mass, 6.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getSingleTargetAiSpell(Spells.resonating_word, 6, is_ally: false, combat_count: 2),
                 //6
                getAoeAiSpell(Spells.cats_grace_mass, 7f, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.overwhelming_presence, 7.5f, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(Spells.shout_greater, 7f, is_ally: false, affects_allies: true)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.remove_fear, Spells.heroism, Spells.eagles_splendor, Spells.cats_grace, Spells.good_hope, Spells.feather_step_mass,
                Spells.echolocation, Spells.see_invisibility_communal, Spells.heroism_greater, Spells.cats_grace_mass
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            var melee_profile = profile.getCopy("BardMelee");
            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.rapid_shot); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //7
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.arcane_strike); //9
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Enchantment);//11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //
            profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.discordant_voice); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //15
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);//
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_precise_shot); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //19
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21
         
            registerProfile(profile);

            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness);//1
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //3
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.arcane_strike); //5
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.power_attack); //7
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.furious_focus); //9
            melee_profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Enchantment);//11
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.discordant_voice); //13
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //15
            melee_profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);//
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.cornugon_smash); //17
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, SkillUnlocks.intimidate_unlock); //19
            melee_profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21

            registerProfile(melee_profile);

            var inspire_courage = library.Get<BlueprintActivatableAbility>("5250fe10c377fdb49be449dfe050ba70");
            inspire_courage.IsOnByDefault = true;
            inspire_courage.DeactivateIfCombatEnded = true;
        }

        static void createAlchemistProfile()
        {
            var profile = new Profile("Alchemist",
                                      Classes.alchemist,
                                      StatType.Intelligence,
                                      new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillThievery });

            profile.addMemorizedSpells(Spells.reduce_person, Spells.mage_shield, Spells.mage_shield, Spells.enlarge_person, Spells.enlarge_person,
                                       Spells.barkskin, Spells.barkskin, Spells.bulls_strength, Spells.cats_grace, Spells.false_life,
                                       Spells.haste, Spells.delay_poison_communal, Spells.protection_from_arrows_communal, Spells.heroism, Spells.heroism,
                                       Spells.false_life_greater, Spells.stone_skin, Spells.stone_skin, Spells.false_life_greater,
                                       Spells.spell_resistance, Spells.spell_resistance, Spells.stoneskin_communal, NewSpells.air_walk_communal,
                                       Spells.legendary_proportions, Spells.legendary_proportions, Spells.heal, Spells.heal, Spells.heal
                                      );

            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getSelfSpell(Spells.reduce_person, 2, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.enlarge_person, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSingleTargetAiSpell(Spells.mage_shield, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { }, combat_count: 2),
                //2
                getSelfSpell(Spells.false_life, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.barkskin, 3, is_precast: true, combat_count: 3),
                getSingleTargetAiSpell(Spells.cats_grace, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }, combat_count: 2),
                //3
                getAoeAiSpell(Spells.haste, 4.5f, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.protection_from_arrows_communal, 4, is_ally: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.heroism, 4, is_precast: true, is_ally: true, combat_count: 2, extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.heroism,  false, extractBuffFromSpell(Spells.heroism_greater)), getNoBuffFromSpell(Spells.heroism, false, extractBuffFromSpell(Spells.good_hope)) }),
                getAoeAiSpell(Spells.delay_poison_communal, 4, is_precast: true,is_ally: true, combat_count: 1),
                //4
                getSingleTargetAiSpell(Spells.stone_skin, 5, is_ally: true, is_precast: true, combat_count: 2),
                getSingleTargetAiSpell(Spells.false_life_greater, 5, is_ally: true, is_precast: true, combat_count: 2),
                //5
                getSingleTargetAiSpell(Spells.spell_resistance, 6, is_ally: true, is_precast: true, combat_count: 2),
                getAoeAiSpell(Spells.stoneskin_communal, 6, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.air_walk_communal, 6, is_ally: true, is_precast: true, combat_count: 1),
                //6
                getSingleTargetAiSpell(Spells.legendary_proportions, 7, is_precast: true, is_ally: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getSelfSpell(Spells.elemental_body3, 7, is_precast: true, variant: Spells.elemental_body3.Variants[2], extra_target_consideration: new Consideration[] { getNoBuffFromSpell(Spells.fiery_body) }, combat_count: 1),


                getAoeAiSpell(AlchemistAbilities.fire_bomb, 3, is_ally: false, affects_allies: false),
                getAoeAiSpell(AlchemistAbilities.force_bomb, 4, is_ally: false, affects_allies: false),
                getAoeAiSpell(AlchemistAbilities.chocking_bomb, 5, is_ally: false, affects_allies: false, combat_count: 1),
                getAoeAiSpell(AlchemistAbilities.blinding_bomb, 6, is_ally: false, affects_allies: false, combat_count: 1),

                getSelfSpell(AlchemistAbilities.dex_mutagen, 5, is_precast: true, combat_count: 1),
                getSelfSpell(AlchemistAbilities.int_cognatogen, 6, is_precast: true, combat_count: 1),
                getSelfSpell(AlchemistAbilities.int_greater_cognatogen, 7, is_precast: true, combat_count: 1),
                getSelfSpell(AlchemistAbilities.int_grand_cognatogen, 8, is_precast: true, combat_count: 1)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.reduce_person, Spells.enlarge_person, Spells.mage_shield, Spells.false_life, Spells.barkskin, Spells.protection_from_arrows_communal,
                Spells.bulls_strength, Spells.cats_grace, Spells.heroism, Spells.delay_poison_communal, NewSpells.air_walk_communal,
                Spells.stone_skin, Spells.stoneskin_communal,  Spells.false_life_greater, Spells.spell_resistance, Spells.elemental_body3, Spells.legendary_proportions,
                AlchemistAbilities.dex_mutagen, AlchemistAbilities.int_cognatogen, AlchemistAbilities.int_greater_cognatogen, AlchemistAbilities.int_grand_cognatogen
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.ability_focus_bombs); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //7
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.rapid_shot); //9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.extra_bombs); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.weapon_focus); //15
            profile.addParametrizedFeatureSelection(Feats.weapon_focus, WeaponCategory.Bomb);
            profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.scales_and_skin); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.iron_will); //19
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //21

            //discoveries
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.precise_bomb); //2
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.infusion); //4
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.choking_bomb); //6
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.blinding_bomb); //8
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.fast_bombs); //10
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.force_bomb); //12
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.cognatogen); //14
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.greater_cognatogen); //16
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.grand_cognatogen); //18
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.preserve_organs); //20
            profile.addFeatureSelection(FeatSelections.alchemist_discovery, Discoveries.mummification); //20

            profile.addFeatureSelection(FeatSelections.grand_discovery, Discoveries.awakened_intellect); 
            registerProfile(profile);
        }


        static void createClericCasterPositive()
        {
            var profile = new Profile("ClericCasterPositive",
                                      Classes.cleric,
                                      StatType.Wisdom,
                                      new StatType[] { StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillLoreNature });

            profile.addMemorizedSpells(Spells.bless, Spells.shield_of_faith, Spells.shield_of_faith, Spells.remove_fear, Spells.haze_of_dreams,
                                       Spells.owls_wisdom, NewSpells.burst_of_radiance, Spells.bulls_strength, Spells.bulls_strength, Spells.bears_endurance,
                                       Spells.archons_aura, Spells.delay_poison_communal, Spells.prayer, Spells.blindness, Spells.blindness,
                                       NewSpells.aura_of_doom, Spells.protection_from_energy_communal, Spells.protection_from_energy_communal, Spells.freedom_of_movement,
                                       NewSpells.fickle_winds, NewSpells.command_greater, Spells.flame_strike, Spells.flame_strike, Spells.flame_strike, Spells.angleic_aspect,
                                       Spells.blade_barrier, Spells.cold_ice_strike, Spells.cold_ice_strike, Spells.cold_ice_strike, Spells.heal,
                                       Spells.waves_of_ecstasy, Spells.destruction, Spells.destruction, Spells.destruction, Spells.destruction,
                                       Spells.frightful_aspect, Spells.fire_storm, Spells.rift_of_ruin, Spells.storm_bolts, Spells.strom_bolts,
                                       Spells.summon_monster9, Spells.overwhelming_presence, Spells.polar_midnight, Spells.heal_mass
                                      );

            var quick_channel = ChannelEnergyEngine.getQuickChannelVariant(library.Get<BlueprintAbility>("f5fc9a1a2a3c1a946a31b320d1dd31b2"));


            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getAoeAiSpell(Spells.bless, 2, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.remove_fear, 2, is_ally: true, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.shield_of_faith, 2, is_ally: true, is_precast: true, combat_count: 2),
                getSingleTargetAiSpell(Spells.haze_of_dreams, 2, is_ally: false,  extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                //2
                getSelfSpell(Spells.owls_wisdom, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.bears_endurance, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getAoeAiSpell(Spells.sound_burst, 3, is_ally: false, affects_allies: true),
                getAoeAiSpell(CallOfTheWild.NewSpells.burst_of_radiance, 3.5f, is_ally: false, affects_allies: true, combat_count: 1),
                //3
                getSelfSpell(Spells.archons_aura, 4, is_precast: true),
                getAoeAiSpell(Spells.delay_poison_communal, 4, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.prayer, 4.5f, is_ally: true),
                getSingleTargetAiSpell(Spells.blindness, 4, is_precast: true, is_ally: true),
                //4
                getSelfSpell(NewSpells.aura_of_doom, 5, is_precast: true),
                getSelfSpell(Spells.protection_from_energy_communal, 5, is_precast: true, variant: Spells.protection_from_energy_communal.Variants[3]), //fire
                getSelfSpell(Spells.protection_from_energy_communal, 5, is_precast: true, variant: Spells.protection_from_energy_communal.Variants[3]), //elec
                getSelfSpell(Spells.freedom_of_movement, 5, is_precast: true),
                //5
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.angleic_aspect, 6, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.command_greater, 8.2f, is_ally: false, affects_allies: false, combat_count: 1, variant: NewSpells.command_greater.Variants[0]),
                getAoeAiSpell(Spells.flame_strike, 6, is_ally: false),
                //6
                getAoeAiSpell(Spells.cold_ice_strike, 57f, is_ally: true, extra_actor_consideration: new Consideration[] { Considerations.no_standard_action, Considerations.swift_action_available }),
                getAoeAiSpell(Spells.blade_barrier, 7.5f, is_ally: false),
                //heal
                //7
                getAoeAiSpell(Spells.waves_of_ecstasy, 8.5f, is_ally: false),
                getSingleTargetAiSpell(Spells.destruction, 8, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                 //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.fire_storm, 9, is_ally: false, affects_allies: false),

                //9
                getAoeAiSpell(Spells.overwhelming_presence, 10.5f, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.polar_midnight, 10, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.summon_monster9, 10f, variant: Spells.summon_monster9_d3, is_precast: true, combat_count: 1),
                //heal mass

                getSelfSpell(quick_channel.Parent, 20.0f, quick_channel, extra_actor_consideration: new Consideration[] {Considerations.no_standard_action, Considerations.injury_around_consideration })
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.bless, Spells.shield_of_faith, Spells.owls_wisdom, Spells.bulls_strength, Spells.bears_endurance, Spells.archons_aura,
                Spells.protection_from_energy_communal, Spells.freedom_of_movement, Spells.delay_poison_communal, NewSpells.aura_of_doom,
                Spells.angleic_aspect, Spells.frightful_aspect, NewSpells.fickle_winds, Spells.summon_monster9, Spells.remove_fear
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //3
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //5
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.selective_channel);//7
            profile.addFeatureSelection(FeatSelections.basic_feat, ChannelEnergyEngine.quick_channel);//9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //11
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //13
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.extra_channel_cleric); //19
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //21

            profile.addFeatureSelection(FeatSelections.channel_energy_selection, ClassAbilities.channel_positive); //21
            registerProfile(profile);
        }


        static void createClericCasterNegative()
        {
            var profile = new Profile("ClericCasterNegative",
                                      Classes.cleric,
                                      StatType.Wisdom,
                                      new StatType[] { StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillLoreNature });

            profile.addMemorizedSpells(Spells.bless, Spells.shield_of_faith, Spells.shield_of_faith, Spells.remove_fear, Spells.haze_of_dreams,
                                       Spells.owls_wisdom, Spells.sound_burst, Spells.bulls_strength, Spells.bulls_strength, Spells.bears_endurance,
                                       Spells.archons_aura, Spells.delay_poison_communal, Spells.prayer, Spells.blindness, Spells.blindness,
                                       NewSpells.aura_of_doom, Spells.protection_from_energy_communal, Spells.protection_from_energy_communal, Spells.freedom_of_movement,
                                       NewSpells.fickle_winds, NewSpells.command_greater, Spells.flame_strike, Spells.flame_strike, Spells.flame_strike, Spells.angleic_aspect,
                                       Spells.blade_barrier, Spells.cold_ice_strike, Spells.cold_ice_strike, Spells.cold_ice_strike, Spells.cold_ice_strike,
                                       Spells.blasphemy, Spells.destruction, Spells.destruction, Spells.destruction, Spells.destruction,
                                       Spells.frightful_aspect, Spells.fire_storm, Spells.rift_of_ruin, Spells.storm_bolts, Spells.strom_bolts,
                                       Spells.summon_monster9, Spells.overwhelming_presence, Spells.polar_midnight, Spells.heal_mass
                                      );

            var quick_channel = ChannelEnergyEngine.getQuickChannelVariant(library.Get<BlueprintAbility>("89df18039ef22174b81052e2e419c728"));


            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getAoeAiSpell(Spells.bless, 2, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.remove_fear, 2, is_ally: true, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.shield_of_faith, 2, is_ally: true, is_precast: true, combat_count: 2),
                getSingleTargetAiSpell(Spells.haze_of_dreams, 2, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }),
                //2
                getSelfSpell(Spells.owls_wisdom, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.bears_endurance, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                getAoeAiSpell(Spells.sound_burst, 3.5f, is_ally: false, affects_allies: true),
                //3
                getSelfSpell(Spells.archons_aura, 4, is_precast: true),
                getAoeAiSpell(Spells.delay_poison_communal, 4, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.prayer, 4.5f, is_ally: true),
                getSingleTargetAiSpell(Spells.blindness, 4, is_precast: true, is_ally: true),
                //4
                getSelfSpell(NewSpells.aura_of_doom, 5, is_precast: true),
                getSelfSpell(Spells.protection_from_energy_communal, 5, is_precast: true, variant: Spells.protection_from_energy_communal.Variants[3]), //fire
                getSelfSpell(Spells.protection_from_energy_communal, 5, is_precast: true, variant: Spells.protection_from_energy_communal.Variants[3]), //elec
                getSelfSpell(Spells.freedom_of_movement, 5, is_precast: true),
                //5
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.angleic_aspect, 6, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.command_greater, 8.2f, is_ally: false, affects_allies: false, combat_count: 1, variant: NewSpells.command_greater.Variants[0]),
                getAoeAiSpell(Spells.flame_strike, 6, is_ally: false),
                //6
                getAoeAiSpell(Spells.cold_ice_strike, 57f, is_ally: true, extra_actor_consideration: new Consideration[] { Considerations.no_standard_action, Considerations.swift_action_available }),
                getAoeAiSpell(Spells.blade_barrier, 7.5f, is_ally: false),
                //heal
                //7
                getAoeAiSpell(Spells.blasphemy, 8.5f, is_ally: false, affects_allies: false),
                getSingleTargetAiSpell(Spells.destruction, 8, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.storm_bolts, 9, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.fire_storm, 9, is_ally: false, affects_allies: false),

                //9
                getAoeAiSpell(Spells.overwhelming_presence, 10.5f, is_ally: false, affects_allies: false),
                getAoeAiSpell(Spells.polar_midnight, 10, is_ally: false, affects_allies: true),
                getSelfSpell(Spells.summon_monster9, 10f, variant: Spells.summon_monster9_d3, is_precast: true, combat_count: 1),
                //heal mass

                getSelfSpell(quick_channel.Parent, 20.0f, quick_channel, extra_actor_consideration: new Consideration[] {Considerations.no_standard_action, Considerations.aoe_more_enemies_considertion })
                );

            getAoeAiSpell(NewSpells.command_greater, 8.2f, is_ally: false, affects_allies: false, combat_count: 1); //for compatibility
            var free_spells = new BlueprintAbility[]
            {
                Spells.bless, Spells.shield_of_faith, Spells.owls_wisdom, Spells.bulls_strength, Spells.bears_endurance, Spells.archons_aura, Spells.remove_fear,
                Spells.protection_from_energy_communal, Spells.freedom_of_movement, Spells.delay_poison_communal, NewSpells.aura_of_doom,
                Spells.angleic_aspect, Spells.frightful_aspect, NewSpells.fickle_winds, Spells.summon_monster9,
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //3
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.selective_channel);//5
            profile.addFeatureSelection(FeatSelections.basic_feat, ChannelEnergyEngine.quick_channel);//7
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //9
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //11
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Enchantment);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //13
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_penetration); //15
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_penetration); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //21
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Necromancy);

            profile.addFeatureSelection(FeatSelections.channel_energy_selection, ClassAbilities.channel_negative);
            registerProfile(profile);
        }





        static void createClericFighter()
        {
            var profile = new Profile("ClericMelee",
                                      Classes.cleric,
                                      StatType.Strength,
                                      new StatType[] { StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillMobility, StatType.SkillLoreNature });

            profile.addMemorizedSpells(Spells.enlarge_person, Spells.bless, Spells.shield_of_faith, Spells.shield_of_faith, Spells.remove_fear,  Spells.divine_favor,
                                       NewSpells.savage_maw, Spells.bulls_strength, Spells.bulls_strength, Spells.bears_endurance, 
                                       NewSpells.deadly_juggernaut, Spells.magical_vestement, Spells.delay_poison_communal, Spells.archons_aura,
                                       Spells.enlarge_person_mass, NewSpells.magic_weapon_greater, NewSpells.aura_of_doom, NewSpells.wrathful_weapon, Spells.protection_from_energy_communal, Spells.freedom_of_movement,
                                       NewSpells.fickle_winds, Spells.righteous_might, Spells.angleic_aspect, Spells.spell_resistance, Spells.burst_of_glory,
                                       Spells.bulls_Strength_mass, Spells.bears_endurance_mass, Spells.owls_wisdom, Spells.heal, Spells.heal,
                                       Spells.summon_monster7, Spells.summon_monster7, NewSpells.particulate_form,
                                       Spells.frightful_aspect, Spells.angleic_aspect_greater, Spells.summon_monster8, Spells.summon_monster8,
                                       Spells.heal_mass, Spells.summon_monster9, Spells.polar_midnight, Spells.heal_mass
                                      );

            var quick_channel = ChannelEnergyEngine.getQuickChannelVariant(library.Get<BlueprintAbility>("f5fc9a1a2a3c1a946a31b320d1dd31b2"));

            getSelfSpell(NewSpells.magic_weapon_greater, 5, is_precast: true, combat_count: 1);
            getSelfSpell(NewSpells.wrathful_weapon, 5, is_precast: true, combat_count: 1, variant: NewSpells.wrathful_weapon.Variants[2]);
            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                 //1
                getSelfSpell(Spells.enlarge_person, 2, is_precast: true,  combat_count: 2),
                getSelfSpell(Spells.divine_favor, 51, combat_count: 1),
                getAoeAiSpell(Spells.bless, 2, is_ally: true, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.remove_fear, 2, is_ally: true, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.shield_of_faith, 2, is_ally: true, is_precast: true, combat_count: 2),
                 //2
                getSelfSpell(NewSpells.savage_maw, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.owls_wisdom, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.bears_endurance, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.bulls_strength, 3, is_ally: true, is_precast: true, extra_target_consideration: new Consideration[] { Considerations.heavy_armor_consideration }, combat_count: 2),
                //3
                getSelfSpell(NewSpells.deadly_juggernaut, 4, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.magical_vestement, 4, is_precast: true, combat_count: 1, variant: Spells.magical_vestement_armor),
                getSelfSpell(Spells.archons_aura, 4, is_precast: true),
                getAoeAiSpell(Spells.delay_poison_communal, 4, is_ally: true, is_precast: true, combat_count: 1),
                //4
                getSelfSpell(NewSpells.magic_weapon_greater, 5, is_precast: true, combat_count: 1, variant: NewSpells.magic_weapon_greater.Variants[0]),
                getSelfSpell(NewSpells.wrathful_weapon, 5, is_precast: true, combat_count: 1, variant: NewSpells.wrathful_weapon.Variants[0]),
                getAoeAiSpell(Spells.enlarge_person_mass, 5, is_ally: true, is_precast: true, combat_count: 1),
                getSelfSpell(NewSpells.aura_of_doom, 5, is_precast: true),
                getSelfSpell(Spells.protection_from_energy_communal, 5, is_precast: true, variant: Spells.protection_from_energy_communal.Variants[3]), //fire
                getSelfSpell(Spells.freedom_of_movement, 5, is_precast: true),
                //5
                getAoeAiSpell(NewSpells.fickle_winds, 6, is_ally: true, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.angleic_aspect, 6, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.righteous_might, 6, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.spell_resistance, 6, is_precast: true, combat_count: 1),
                //6
                getAoeAiSpell(Spells.bulls_Strength_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.bears_endurance_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                getAoeAiSpell(Spells.owls_strength_mass, 7f, is_precast: true, is_ally: true, combat_count: 1),
                //heal
                //7
                getSelfSpell(Spells.summon_monster7, 8f, variant: Spells.summon_monster7_d3, is_precast: true, combat_count: 1),
                getAoeAiSpell(NewSpells.particulate_form, 8.5f, is_ally: true, combat_count: 1),
                //8
                getSelfSpell(Spells.frightful_aspect, 9, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.angleic_aspect_greater, 9, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.summon_monster8, 9f, variant: Spells.summon_monster8_d3, is_precast: true, combat_count: 1),

                //9
                getSelfSpell(Spells.summon_monster9, 10f, variant: Spells.summon_monster9_d3, is_precast: true, combat_count: 1),
                getAoeAiSpell(Spells.polar_midnight, 10, is_ally: false, affects_allies: true)
                //heal mass

                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.enlarge_person, Spells.bless, Spells.shield_of_faith, Spells.owls_wisdom, Spells.bulls_strength, Spells.bears_endurance, Spells.archons_aura,
                NewSpells.savage_maw, Spells.remove_fear, NewSpells.deadly_juggernaut, Spells.magical_vestement, NewSpells.magic_weapon_greater, NewSpells.wrathful_weapon,
                Spells.protection_from_energy_communal, Spells.freedom_of_movement, Spells.delay_poison_communal, NewSpells.aura_of_doom,
                Spells.righteous_might, Spells.spell_resistance, Spells.bulls_Strength_mass, Spells.bears_endurance_mass, Spells.owls_strength_mass,
                Spells.summon_monster7, Spells.summon_monster8, Spells.summon_monster9, Spells.angleic_aspect_greater,
                Spells.angleic_aspect, Spells.frightful_aspect, NewSpells.fickle_winds,
            };
            profile.addFeatureComponent(0,
                Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = free_spells),
                Helpers.Create<AutoMetamagic>(u => { u.Abilities = free_spells.ToList(); u.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach; })
                );

            profile.addFeatureComponent(9,
                Helpers.Create<AutoMetamagic>(a => a.Abilities = new BlueprintAbility[] { Spells.divine_favor }.ToList())
            );

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.martial_weapon_proficiency);//0
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus);//7
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Conjuration);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.quicken_spell);//9
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.augment_summoning); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.superior_summoning); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //15
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Necromancy);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.dodge); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.scales_and_skin); //19
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.power_attack); //21
            profile.addFeatureSelection(FeatSelections.basic_feat, NewFeats.furious_focus); //23

            profile.addFeatureSelection(FeatSelections.channel_energy_selection, ClassAbilities.channel_positive);
            profile.addFeatureSelection(FeatSelections.channel_energy_selection, ClassAbilities.channel_negative);
            registerProfile(profile);
        }


        static void createEldritchArcher()
        {
            var profile = new Profile("EldritchArcher",
                                      Classes.magus,
                                      StatType.Strength,
                                      new StatType[] { StatType.SkillMobility, StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillUseMagicDevice },
                                      Archetypes.eldritch_archer
                                      );

            profile.addMemorizedSpells(Spells.snow_ball, Spells.reduce_person, Spells.shield, Spells.snow_ball, Spells.snow_ball, Spells.snow_ball, Spells.snow_ball,
                                       Spells.mirror_image, Spells.scorching_ray, Spells.blur, Spells.scorching_ray, Spells.scorching_ray, Spells.scorching_ray,
                                       Spells.haste, NewSpells.ray_of_exhaustion, NewSpells.channel_vigor, NewSpells.ray_of_exhaustion,
                                       Spells.greater_invisibility, Spells.controlled_fireball, Spells.controlled_fireball, Spells.controlled_fireball, Spells.controlled_fireball,
                                       Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, Spells.fire_snake,
                                       Spells.hell_fire_ray, Spells.hell_fire_ray, Spells.disintegrate, Spells.disintegrate, Spells.hell_fire_ray
                                      );
     
            profile.setAiActions(//no cantrips
                AiActions.attack_action,
                //1
                getSelfSpell(Spells.reduce_person, 2, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.shield, 2,  is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.snow_ball, 2, is_ally: false),
                //2
                getSelfSpell(Spells.mirror_image, 3, is_precast: true, combat_count: 1),
                getSelfSpell(Spells.blur, 3, is_precast: true, combat_count: 1),
                getSingleTargetAiSpell(Spells.scorching_ray, 3, is_ally: false),
                //3
                getAoeAiSpell(Spells.haste, 54, is_ally: true, combat_count: 1),
                getSelfSpell(NewSpells.channel_vigor, 4, combat_count: 1, variant: NewSpells.channel_vigor.Variants[1]),
                getSingleTargetAiSpell(NewSpells.ray_of_exhaustion, 3, is_ally: false),
                //4
                getSelfSpell(Spells.greater_invisibility, 5, combat_count: 1),
                getAoeAiSpell(Spells.controlled_fireball, 5f, is_ally: false, affects_allies: false),
                //5
                getAoeAiSpell(Spells.fire_snake, 6, is_ally: false, affects_allies: false),
                 //6
                getSingleTargetAiSpell(Spells.hell_fire_ray, 7, is_ally: false),
                getSingleTargetAiSpell(Spells.disintegrate, 7, is_ally: false, extra_target_consideration: new Consideration[] { Considerations.light_armor_consideration }),

                getSelfSpell(Arcana.arcane_weapon_switch, 101, combat_count: 1),
                getSelfSpell(Arcana.prescient_attack, 100, cooldown_rounds: 1),
                getSelfSpell(Arcana.aracne_accuracy, 99, cooldown_rounds: 1)
                );

            var free_spells = new BlueprintAbility[]
            {
                Spells.reduce_person, Spells.shield, Spells.mirror_image, Spells.blur
            };

            //feats
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.point_blank_shot);//0
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.precise_shot);//1
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.rapid_shot); //3
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.toughness); //5
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.weapon_focus);//7
            //to specialize on profile instantiation
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.many_shot);//9
            
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_initiative); //11
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.combat_casting); //13
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.spell_focus); //15
            profile.addParametrizedFeatureSelection(Feats.spell_focus, SpellSchool.Evocation);
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.improved_precise_shot); //17
            profile.addFeatureSelection(FeatSelections.basic_feat, Feats.greater_spell_focus); //19
            profile.addParametrizedFeatureSelection(Feats.greater_spell_focus, SpellSchool.Evocation);


            profile.addFeatureSelection(FeatSelections.magus_feat, Feats.dodge);//5
            profile.addFeatureSelection(FeatSelections.magus_feat, Feats.clustered_shots);//11
            profile.addFeatureSelection(FeatSelections.magus_feat, Feats.weapon_specialization);//17

            profile.addFeatureSelection(FeatSelections.magus_arcana, ClassAbilities.arcane_accuracy);
            profile.addFeatureSelection(FeatSelections.magus_arcana, ClassAbilities.prescient_strike);
            profile.addFeatureSelection(FeatSelections.magus_arcana, ClassAbilities.enduring_blade);

            profile.addFeatureComponent(0, Helpers.CreateAddFacts(library.Get<BlueprintBuff>("91e4b45ab5f29574aa1fb41da4bbdcf2"), //spell combat
                                                                  library.Get<BlueprintBuff>("06e0c9887eb1724409977dac7168bfd7") //spell strike
                                        ));
            registerProfile(profile);
        }
    }
}
