using CallOfTheWild;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI
{
    class UpdateAi
    {
        static LibraryScriptableObject library = Main.library;
        static UnitsAroundConsideration aoe_considertion => library.Get<UnitsAroundConsideration>("96a5a05d98be03446bbf21e217270b06");
        static UnitsAroundConsideration aoe_more_enemies_considertion => library.Get<UnitsAroundConsideration>("b2490b137b8b53a4e950c1d79d1c5c1d");
        static CommandCooldownConsideration swift_action_available = library.Get<CommandCooldownConsideration>("c2b7d2f9a5cb8d04d9e1aa4bf3d3c598");
        static CommandCooldownConsideration no_standard_action = library.Get<CommandCooldownConsideration>("eb52264e87de14842b44b362da4e0673");
        static Consideration[] harmful_enemy_ally_aoe_target_consideration = library.Get<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa").TargetConsiderations;
        static Consideration[] harmful_enemy_aoe_target_consideration = new Consideration[] { aoe_considertion, aoe_more_enemies_considertion };
        static Consideration attack_target_consideration = library.Get<ComplexConsideration>("7a2b25dcc09cd244db261ce0a70cca84");
        static Consideration light_armor_consideration = library.Get<ArmorTypeConsideration>("2ba801c8a6f585749b7fd636e843e6f0");
        static Consideration heavy_armor_consideration = library.Get<ArmorTypeConsideration>("c376d918c01838b48befcb711cc528ff");
        static Consideration injury_around_consideration = library.Get<HealthAroundConsideration>("2a2cfff1d585f3142aadaafe0c1a74e6");

        static class Feats
        {
            public static BlueprintFeature dodge = library.Get<BlueprintFeature>("97e216dbb46ae3c4faef90cf6bbe6fd5");
            public static BlueprintFeature improved_initiative = library.Get<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74");
            public static BlueprintParametrizedFeature spell_focus = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            public static BlueprintParametrizedFeature greater_spell_focus = library.Get<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");
            public static BlueprintFeature spell_penetration = library.Get<BlueprintFeature>("ee7dc126939e4d9438357fbd5980d459");
        }



 
        static class Spells
        {
            public static BlueprintAbility bless = library.Get<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638");
            public static BlueprintAbility shield_of_faith = library.Get<BlueprintAbility>("183d5bb91dea3a1489a6db6c9cb64445");
            public static BlueprintAbility bulls_strength = library.Get<BlueprintAbility>("4c3d08935262b6544ae97599b3a9556d");
            public static BlueprintAbility owls_wisdom = library.Get<BlueprintAbility>("f0455c9295b53904f9e02fc571dd2ce1");
            public static BlueprintAbility divine_favor = library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113");
            public static BlueprintAbility divine_power = library.Get<BlueprintAbility>("ef16771cb05d1344989519e87f25b3c5");
            public static BlueprintAbility prayer = library.Get<BlueprintAbility>("faabd2cc67efa4646ac58c7bb3e40fcc");

            public static BlueprintAbility phantasmal_putrefaction = library.Get<BlueprintAbility>("1f2e6019ece86d64baa5effa15e81ecc");
            public static BlueprintAbility magic_missile = library.Get<BlueprintAbility>("4ac47ddb9fa1eaf43a1b6809980cfbd2");
            public static BlueprintAbility cold_ice_strike = library.Get<BlueprintAbility>("5ef85d426783a5347b420546f91a677b");
            public static BlueprintAbility mind_fog = library.Get<BlueprintAbility>("eabf94e4edc6e714cabd96aa69f8b207");
            public static BlueprintAbility phantasmal_killer = library.Get<BlueprintAbility>("6717dbaef00c0eb4897a1c908a75dfe5");
            public static BlueprintAbility phantasmal_web = library.Get<BlueprintAbility>("12fb4a4c22549c74d949e2916a2f0b6a");
            public static BlueprintAbility fireball = library.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            public static BlueprintAbility acid_fog = library.Get<BlueprintAbility>("dbf99b00cd35d0a4491c6cc9e771b487");
            public static BlueprintAbility cloudkill = library.Get<BlueprintAbility>("548d339ba87ee56459c98e80167bdf10");
            public static BlueprintAbility sirocco = library.Get<BlueprintAbility>("093ed1d67a539ad4c939d9d05cfe192c");
            public static BlueprintAbility stinking_cloud = library.Get<BlueprintAbility>("68a9e6d7256f1354289a39003a46d826");
            public static BlueprintAbility serenity = library.Get<BlueprintAbility>("d316d3d94d20c674db2c24d7de96f6a7");
            public static BlueprintAbility cloak_of_dreams = library.Get<BlueprintAbility>("7f71a70d822af94458dc1a235507e972");
            public static BlueprintAbility confusion = library.Get<BlueprintAbility>("cf6c901fb7acc904e85c63b342e9c949");
            public static BlueprintAbility fear = library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");
            public static BlueprintAbility false_life = library.Get<BlueprintAbility>("7a5b5bf845779a941a67251539545762");
            public static BlueprintAbility false_life_greater = library.Get<BlueprintAbility>("dc6af3b4fd149f841912d8a3ce0983de");

            public static BlueprintAbility spike_stones = library.Get<BlueprintAbility>("d1afa8bc28c99104da7d784115552de5");
            public static BlueprintAbility slowing_mud = library.Get<BlueprintAbility>("6b30813c3709fc44b92dc8fd8191f345");
            public static BlueprintAbility obsidian_flow = library.Get<BlueprintAbility>("e48638596c955a74c8a32dbc90b518c1");
            public static BlueprintAbility flame_strike = library.Get<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");

            public static BlueprintAbility fire_snake = library.Get<BlueprintAbility>("ebade19998e1f8542a1b55bd4da766b3");

            
            public static BlueprintAbility baleful_polymorph = library.Get<BlueprintAbility>("3105d6e9febdc3f41a08d2b7dda1fe74");
            public static BlueprintAbility tar_pool = library.Get<BlueprintAbility>("7d700cdf260d36e48bb7af3a8ca5031f");
        }


        static class AiActions
        {
            static public BlueprintAiAction attack_action = library.Get<BlueprintAiAction>("866ffa6c34000cd4a86fb1671f86c7d8");
            static public Consideration target_self = library.Get<Consideration>("83e2dd97b82d769498394c3edf0d260e");
            static public BlueprintAiCastSpell cast_cold_ice_strike = createCastSpellAction("CastColdIceStrikeSpellAiAction", Spells.cold_ice_strike, 
                                                                                     new Consideration[] { swift_action_available, no_standard_action },
                                                                                     new Consideration[] { aoe_considertion },
                                                                                     base_score: 50.0f, variant: null, cooldown_rounds: 1);
            static public BlueprintAiCastSpell quick_channel_action;
            static public BlueprintAiCastSpell cast_command = createCastSpellAction("CastCommandGreaterHaltSpellAiAction", NewSpells.command_greater, 
                                                                                  new Consideration[0], 
                                                                                  new Consideration[] { aoe_more_enemies_considertion },
                                                                                  base_score: 10, variant: NewSpells.command_greater.Variants[0]);

            static public BlueprintAiCastSpell cast_phantasmal_putrefaction_once = createCastSpellAction("CastPhantasmalPutrefactionSpellAiAction", Spells.phantasmal_putrefaction,
                                                                                  new Consideration[0],
                                                                                  new Consideration[] { aoe_more_enemies_considertion },
                                                                                  base_score: 10.0f, combat_count: 1);
            static public BlueprintAiCastSpell cast_phantasmal_web_plus2_once = createCastSpellAction("CastPhantasmalWebPlus2SpellAiAction", Spells.phantasmal_web,
                                                                                                      new Consideration[0],
                                                                                                      new Consideration[] { aoe_more_enemies_considertion },
                                                                                                      base_score: 10.0f, combat_count: 1);
            static public BlueprintAiCastSpell cast_confusion_plus2_once = createCastSpellAction("CastConfusionPlus2SpellAiAction", Spells.confusion,
                                                                                          new Consideration[0],
                                                                                          new Consideration[] { aoe_more_enemies_considertion },
                                                                                          base_score: 10.0f, combat_count: 1);
            static public BlueprintAiCastSpell cast_acid_fog_once = createCastSpellAction("CastAcidFogSpellAiAction", Spells.acid_fog,
                                                                                      new Consideration[0],
                                                                                      harmful_enemy_ally_aoe_target_consideration,
                                                                                      base_score: 7.5f, combat_count: 1);
            static public BlueprintAiCastSpell cast_phantasmal_killer = library.Get<BlueprintAiCastSpell>("0ae15dd91d960d34f9ffe5fb3eb0d655");
            static public BlueprintAiCastSpell cast_cloudkill_once = createCastSpellAction("CastCloudkillSpellOnceAiAction", Spells.cloudkill,
                                                                                              new Consideration[0],
                                                                                              harmful_enemy_ally_aoe_target_consideration,
                                                                                              base_score: 6.5f, combat_count: 1);
            static public BlueprintAiCastSpell cast_fireball_quicken = createCastSpellAction("CastQuickenFireballAiAction", Spells.fireball,
                                                                      new Consideration[] { swift_action_available},
                                                                      harmful_enemy_ally_aoe_target_consideration,
                                                                      base_score: 20.0f);
            static public BlueprintAiCastSpell cast_sheet_lightning_quicken_once = createCastSpellAction("CastQuickenSheetLightningAiAction", NewSpells.sheet_lightning,
                                                          new Consideration[] { swift_action_available },
                                                          harmful_enemy_ally_aoe_target_consideration,
                                                          base_score: 30.0f, combat_count: 1);

            static public BlueprintAiCastSpell spike_stones6 = library.Get<BlueprintAiCastSpell>("52c7350061720e44f861b867471a8c4c");

            static public BlueprintFeature cleric_free_cast_long_spells;
            static public BlueprintAiAction[] cleric_precast_spells_ai_action;

            static public void init()
            {
                var quick_channel = ChannelEnergyEngine.getQuickChannelVariant(library.Get<BlueprintAbility>("89df18039ef22174b81052e2e419c728"));
                quick_channel_action = createCastSpellAction("QuickChannelNegativeClericAiAction", quick_channel.Parent, 
                                                             new Consideration[1] { no_standard_action }, 
                                                             new Consideration[] { aoe_more_enemies_considertion },
                                                             base_score: 20.0f, variant: quick_channel, cooldown_rounds: 1);

                var spells = new BlueprintAbility[] {NewSpells.aura_of_doom,
                                                Spells.shield_of_faith,
                                                NewSpells.bone_fists,
                                                Spells.owls_wisdom,
                                                Spells.bulls_strength,
                                                Spells.divine_favor,
                                                Spells.divine_power,
                                                Spells.prayer};
                cleric_free_cast_long_spells = Helpers.CreateFeature("AiClericFreeCastBuffSpellsLong",
                                                                     "",
                                                                     "",
                                                                     "",
                                                                     null,
                                                                     FeatureGroup.None,
                                                                     Helpers.Create<CallOfTheWild.TurnActionMechanics.UseAbilitiesAsFreeAction>(u => u.abilities = spells)
                                                                     );

                cleric_precast_spells_ai_action = new BlueprintAiAction[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    cleric_precast_spells_ai_action[i] = createCastSpellAction("ClericFreePrecast" + spells[i].name +"AiAction",
                                                           spells[i],
                                                           new Consideration[0],
                                                           new Consideration[] { target_self },
                                                           100.0f, 
                                                           combat_count: 1);
                    
                }
            }

        }

        static internal void load()
        {
            
            AiActions.init();
            //updateAttackConsiderations();
            updateTsanna();

            updateGoblinFighter();
            updateGoblinRogue();
            updateGoblinArcher();
            updateAlchemist();
            updateGoblinShaman();

            fixFallenPriest();
            fixBanditConjurers();
            fixBanditTransmuter2();


            //fix necromancers, illusionist
            fixMaestroJanush();

            fixDuergarDruids();
            fixDuergarKineticist();

            fixDLC2CultistNecromancerBoss();
            
            fixVarraskInquisitor();
            fixLadyOfShallows();
            fixBSLCyclopsCleric();

            fixSaves();
        }

        static void fixBSLCyclopsCleric()
        {
            var brain = library.Get<BlueprintBrain>("0c17da3383b976b42b853e458b51f6bf");
            var feature_list = library.Get<BlueprintFeature>("82662ebad000b1349baf02e2f8e86748");
            var acl = feature_list.GetComponents<AddClassLevels>().Where(c => c.CharacterClass == library.Get<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0")).FirstOrDefault();
            acl.MemorizeSpells = new BlueprintAbility[]
            {
                Spells.shield_of_faith,
                Spells.bulls_strength,
                Spells.owls_wisdom,
                NewSpells.aura_of_doom,
                Spells.divine_power
            };
            var unit = library.Get<BlueprintUnit>("fe662d20a0272bb4ea66bef675b4b52d");
            unit.AddFacts = unit.AddFacts.AddToArray(AiActions.cleric_free_cast_long_spells);
            //feature_list.AddComponent(Helpers.CreateAddFact(AiActions.cleric_free_cast_long_spells));
            brain.Actions = AiActions.cleric_precast_spells_ai_action.AddToArray(AiActions.attack_action);
        }

        static void fixDuergarKineticist()
        {
            var deadly_earth_action = library.Get<BlueprintAiCastSpell>("90ba668c7e9f73d449f0e320a7755fb1");
            deadly_earth_action.Variant = library.Get<BlueprintAbility>("0be97d0e752060f468bbf62ce032b9f5");
            deadly_earth_action.CombatCount = 1;
            var brain = library.Get<BlueprintBrain>("58455fc3b78db7c4282593ac7acabcb4");
            brain.Actions = brain.Actions.AddToArray(deadly_earth_action);
        }


        static void fixDuergarDruids()
        {
            var cast_tar_pool = createCastSpellAction("CastTarPool7AiAction", Spells.tar_pool,
                                                                                  new Consideration[0],
                                                                                  harmful_enemy_ally_aoe_target_consideration,
                                                                                  base_score: 7.0f, combat_count: 1);

            var cast_sirocco = createCastSpellAction("CastSirocco7AiAction", Spells.sirocco,
                                                                      new Consideration[0],
                                                                      harmful_enemy_ally_aoe_target_consideration,
                                                                      base_score: 7.0f, combat_count: 1);


            var cast_fire_snake6 = createCastSpellAction("CastFireSnake6AiAction", Spells.fire_snake,
                                                                      new Consideration[0],
                                                                      harmful_enemy_aoe_target_consideration,
                                                                      base_score: 6.0f);

            var cast_slowing_mud = createCastSpellAction("CastSlowingMud6_1AiAction", Spells.slowing_mud,
                                                          new Consideration[0],
                                                          harmful_enemy_aoe_target_consideration,
                                                          base_score: 5.1f, combat_count: 1);


            var cast_flamestrike = createCastSpellAction("CastFlamestrike5AiAction", Spells.flame_strike,
                                                          new Consideration[0],
                                                          harmful_enemy_ally_aoe_target_consideration,
                                                          base_score: 5.0f);


            var cast_be = createCastSpellAction("CastBurningEntanglement4_1AiAction", CallOfTheWild.NewSpells.burning_entanglement,
                                  new Consideration[0],
                                  harmful_enemy_ally_aoe_target_consideration,
                                  base_score: 4.1f);

            var cast_earth_tremor = new BlueprintAiCastSpell[CallOfTheWild.NewSpells.earth_tremor.Variants.Length];
            for (int i = 0; i < CallOfTheWild.NewSpells.earth_tremor.Variants.Length; i++)
            {
                cast_earth_tremor[i] = createCastSpellAction($"CastEarthTremor{i}4AiAction", CallOfTheWild.NewSpells.earth_tremor,
                                                              new Consideration[0],
                                                              harmful_enemy_ally_aoe_target_consideration,
                                                              base_score: 4.0f, variant: CallOfTheWild.NewSpells.earth_tremor.Variants[i]);
            }

            var brain = library.Get<BlueprintBrain>("7f6527bd36838ff42a1cb3964a05fd1b");
            brain.Actions = brain.Actions.Take(1).ToArray().AddToArray(cast_tar_pool, 
                                                                       cast_sirocco,
                                                                       cast_fire_snake6,
                                                                       cast_slowing_mud,
                                                                       cast_flamestrike,
                                                                       cast_be).AddToArray(cast_earth_tremor);

            var features = library.Get<BlueprintFeature>("fd2460491b9d8e843b52c19f915ef47b");
            features.GetComponent<AddClassLevels>().MemorizeSpells = new BlueprintAbility[]
            {
                Spells.tar_pool, Spells.sirocco, //6
                Spells.fire_snake, Spells.fire_snake, Spells.fire_snake, //5
                Spells.slowing_mud, Spells.flame_strike, Spells.flame_strike, Spells.flame_strike, //4
                CallOfTheWild.NewSpells.burning_entanglement, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor, CallOfTheWild.NewSpells.earth_tremor
            };



        }


        static void fixLadyOfShallows()
        {
            var sorcerer = library.Get<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            var unit = library.Get<BlueprintUnit>("ac131b0101870b6489609a7f33b5e576");

            var class_levels = unit.GetComponents<AddClassLevels>().Where(c => c.CharacterClass == sorcerer).FirstOrDefault();
            class_levels.SelectSpells = class_levels.SelectSpells.AddToArray(Spells.fireball,
                                                                             Spells.confusion,
                                                                             Spells.phantasmal_killer,
                                                                             Spells.cloudkill, 
                                                                             Spells.acid_fog).RemoveFromArray(Spells.serenity);
            class_levels.Selections[0].Features = class_levels.Selections[0].Features.AddToArray(Feats.spell_focus, Feats.spell_focus, Feats.spell_penetration);
            class_levels.Selections = class_levels.Selections.AddToArray(createSpellFocusSelection(SpellSchool.Evocation));
            class_levels.Selections = class_levels.Selections.AddToArray(createGreaterSpellFocusSelection(SpellSchool.Evocation));
            class_levels.Selections = class_levels.Selections.AddToArray(createSpellFocusSelection(SpellSchool.Conjuration));
            class_levels.Selections = class_levels.Selections.AddToArray(createSpellFocusSelection(SpellSchool.Enchantment));
            //var auto_quicken_metamagic = library.Get<BlueprintFeature>("d26acdac8ded95b44b787c9700634fc9");
           // auto_quicken_metamagic.GetComponent<AutoMetamagic>().Abilities.AddRange(new BlueprintAbility[] { Spells.fireball, NewSpells.sheet_lightning });
           // auto_quicken_metamagic.AddComponent(Helpers.Create<IncreaseSpellDC>(i => { i.BonusDC = 2; i.Spell = Spells.confusion; }));
            var brain = unit.Brain;
            brain.Actions = brain.Actions.AddToArray(AiActions.cast_fireball_quicken, 
                                                     AiActions.cast_cloudkill_once,
                                                     AiActions.cast_confusion_plus2_once,
                                                     AiActions.cast_acid_fog_once,
                                                     AiActions.cast_phantasmal_killer);
           // unit.Body.QuickSlots[0] = library.Get<BlueprintItemEquipmentUsable>("55a059b32df920c4abe65b8ee8b56056"); //rod of quicken metamagic lesser
            //unit.Body.QuickSlots[0] = library.Get<BlueprintItemEquipmentUsable>("651b0460f600d5f42b0467e7186aab80"); //rod of lesser maximize
        }

        static void fixMaestroJanush()
        {
            //increase level of janush to allow him to cast baleful polymorph and tar pool
            var unit = library.Get<BlueprintUnit>("14d442fea70487047bedb82d86b55451");

            var new_feature_list = library.CopyAndAdd<BlueprintFeature>("8bcac5e1bd4f254438fa8797337137a6", "MaestroJanushFeatureList", "");
            new_feature_list.ReplaceComponent<AddClassLevels>(a =>
                                                                {
                                                                    a.Levels = 12;
                                                                    a.Selections[0].Features = a.Selections[0].Features.AddToArray(Feats.improved_initiative, Feats.dodge);

                                                                }
                                                                );
            unit.AddFacts[0] = new_feature_list;
            unit.GetComponent<Experience>().CR = 10;
            var fox_cunning_buff = library.Get<BlueprintBuff>("c8c9872e9e02026479d82b9264b9cc6b");
            var stoneskin_buff = library.Get<BlueprintBuff>("7aeaf147211349b40bb55c57fec8e28d");
            var mirror_image_buff = library.Get<BlueprintBuff>("e0f432ae40dcd894f8d80ee4e81a38d4");
            new_feature_list.GetComponent<AddFacts>().Facts = new_feature_list.GetComponent<AddFacts>().Facts.AddToArray(fox_cunning_buff, stoneskin_buff, mirror_image_buff);
        }


        static void fixDLC2CultistNecromancerBoss()
        {
            var unit = library.Get<BlueprintUnit>("c71f32cb01138b54a8e9747f84c435b8");

            var spell_list = library.CopyAndAdd<BlueprintFeature>("941a90b7b2e418e4597b99513f96db2e", "DLC2_NecromancerSpellList", "");
            spell_list.ReplaceComponent<LearnSpells>(l => l.Spells = l.Spells.AddToArray(Spells.cloudkill, Spells.stinking_cloud, Spells.fear));
            unit.AddFacts = unit.AddFacts.AddToArray(spell_list);

            var auto_metamgic = library.Get<BlueprintFeature>("f65fc9a042f5e7247a03702dca121936");
            auto_metamgic.GetComponent<AutoMetamagic>().Abilities.Add(Spells.false_life_greater);

            var fear_ai_action = createCastSpellAction("CastFear13SpellOnceAiAction", Spells.fear,
                                                                                              new Consideration[0],
                                                                                              harmful_enemy_ally_aoe_target_consideration,
                                                                                              base_score: 13.0f, combat_count: 1,
                                                                                              guid : "63620d265f8e46fc9c2ee2e997a468e5");

            var cloudkill_ai_action = createCastSpellAction("CastCloudkill15SpellOnceAiAction", Spells.cloudkill,
                                                                                              new Consideration[0],
                                                                                              harmful_enemy_ally_aoe_target_consideration,
                                                                                              base_score: 15.0f, combat_count: 1);
            var class_levels = unit.GetComponent<AddClassLevels>();
            class_levels.MemorizeSpells[4] = Spells.fear;
            class_levels.MemorizeSpells = class_levels.MemorizeSpells.AddToArray(Spells.cloudkill);

            var brain = unit.Brain;
            brain.Actions = brain.Actions.AddToArray(fear_ai_action, cloudkill_ai_action);
        }

        static void fixBanditTransmuter2()
        {
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditTransmuterFeatureListLevel")).ToArray();
            var spell_lists = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditTransmuterSpellListLevel")).ToArray();

            var obsidian_flow = library.Get<BlueprintAbility>("e48638596c955a74c8a32dbc90b518c1");
            var baleful_polymorph = library.Get<BlueprintAbility>("3105d6e9febdc3f41a08d2b7dda1fe74");
            var tar_pool = library.Get<BlueprintAbility>("7d700cdf260d36e48bb7af3a8ca5031f");

            var spells_to_cast = new BlueprintAbility[] { obsidian_flow,
                                                          baleful_polymorph,
                                                          tar_pool,
                                                          CallOfTheWild.NewSpells.rigor_mortis,
                                                        };

            var reduce_mass = library.Get<BlueprintAbility>("2427f2e3ca22ae54ea7337bbab555b16");
            var enlarge_mass = library.Get<BlueprintAbility>("66dc49bf154863148bd217287079245e");

            foreach (var f in features)
            {
                var add_calss_levels = f.GetComponent<AddClassLevels>();

                var spell_list = add_calss_levels.MemorizeSpells.RemoveFromArray(reduce_mass).RemoveFromArray(reduce_mass).RemoveFromArray(enlarge_mass);

                spell_list = spell_list.AddToArray(new BlueprintAbility[]
                                                                        {
                                                                            CallOfTheWild.NewSpells.rigor_mortis,
                                                                            CallOfTheWild.NewSpells.rigor_mortis,
                                                                            CallOfTheWild.NewSpells.rigor_mortis,
                                                                            baleful_polymorph,
                                                                            baleful_polymorph,
                                                                            baleful_polymorph,
                                                                            tar_pool
                                                                        }
                                                   );
                add_calss_levels.MemorizeSpells = spell_list;
            }

            
            foreach (var sl in spell_lists)
            {
                sl.GetComponent<LearnSpells>().Spells = sl.GetComponent<LearnSpells>().Spells.AddToArray(spells_to_cast);
            }

            var brain = library.Get<BlueprintBrain>("bf90f2053c06375418c119115122ae3d");


            var area_spells = new BlueprintAbility[] { obsidian_flow, tar_pool };
            for (int i = 0; i < area_spells.Length; i++)
            {
                var ai_action = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", area_spells[i].name + "BanditTransmuterAiAction", "");
                ai_action.Ability = area_spells[i];
                ai_action.BaseScore = 5.9f + 2*i;
                brain.Actions = brain.Actions.AddToArray(ai_action);
            }

            var polymorph_buff = library.Get<BlueprintBuff>("0a52d8761bfd125429842103aed48b90");
            var rigor_mortis_buff = library.Get<BlueprintBuff>("ed6ccaa592c4414cb6a9d29149a368e0");

            var no_polymorph_buff = library.Get<BuffConsideration>("881bcb09ccf53074d88c5dec9f2fcaa9");
            var no_rigor_mortis_buff = createNoBuffConsideration("NoBufRigorMortis", rigor_mortis_buff);

            var apply_polymorph = createCastSpellAction("BanditTransmuterCastBalefulPolymorphAiAction", baleful_polymorph, new Consideration[0],
                                                        new Consideration[] { light_armor_consideration, no_polymorph_buff },
                                                        5 + 1);

            var apply_rigor_mortis = createCastSpellAction("BanditTransmuterCastRigorMortisAiAction", CallOfTheWild.NewSpells.rigor_mortis, new Consideration[0],
                                            new Consideration[] { light_armor_consideration, no_rigor_mortis_buff },
                                            4 + 1);
            brain.Actions = brain.Actions.AddToArray(apply_polymorph, apply_rigor_mortis);

            var blueprint_fix = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", "BlueprintFix7b662573dc6842b4aa3643f620d61693", "7b662573dc6842b4aa3643f620d61693");
            var blueprint_fix2 = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", "BlueprintFixf14df4d4c1884f22a790b05d303de6ab", "f14df4d4c1884f22a790b05d303de6ab");
            var blueprint_fix3 = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", "BlueprintFix985e411952d94369bc155c6725872022", "985e411952d94369bc155c6725872022");
            var blueprint_fix4 = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", "BlueprintFixdfde3680c8cc4164888772e4fd3b7dca", "dfde3680c8cc4164888772e4fd3b7dca");
        }


        static void fixBanditConjurers()
        {
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditConjurerFeatureListLevel")).ToArray();
            var spell_lists = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditConjurerSpellList")).ToArray();

            var grease = library.Get<BlueprintAbility>("95851f6e85fe87d4190675db0419d112");
            var glitterdust = library.Get<BlueprintAbility>("ce7dad2b25acf85429b6c9550787b2d9");
            var stinking_cloud = library.Get<BlueprintAbility>("68a9e6d7256f1354289a39003a46d826");
            var acid_pit = library.Get<BlueprintAbility>("1407fb5054d087d47a4c40134c809f12");

            var spells_to_cast = new BlueprintAbility[] { grease,
                                                          glitterdust,
                                                          stinking_cloud,
                                                          acid_pit,
                                                        };

            var spell_list = new BlueprintAbility[] { library.Get<BlueprintAbility>("0c852a2405dd9f14a8bbcfaf245ff823"), //acid splash
                                                      grease,
                                                      library.Get<BlueprintAbility>("8fd74eddd9b6c224693d9ab241f25e84"), //sm 1
                                                      library.Get<BlueprintAbility>("8fd74eddd9b6c224693d9ab241f25e84"), //sm 1
                                                      glitterdust,
                                                      library.Get<BlueprintAbility>("1724061e89c667045a6891179ee2e8e7"), //sm2
                                                      library.Get<BlueprintAbility>("1724061e89c667045a6891179ee2e8e7"), //sm2
                                                      stinking_cloud,
                                                      library.Get<BlueprintAbility>("5d61dde0020bbf54ba1521f7ca0229dc"), //sm3
                                                      library.Get<BlueprintAbility>("5d61dde0020bbf54ba1521f7ca0229dc"), //sm3
                                                      acid_pit,
                                                      library.Get<BlueprintAbility>("7ed74a3ec8c458d4fb50b192fd7be6ef"), //sm4
                                                      library.Get<BlueprintAbility>("7ed74a3ec8c458d4fb50b192fd7be6ef"), //sm4
                                                      library.Get<BlueprintAbility>("903092f6488f9ce45a80943923576ab3"), //displacement
                                                      library.Get<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564"), //mirror image
                                                     };
      
            foreach (var f in features)
            {
                f.GetComponent<AddClassLevels>().MemorizeSpells = spell_list;
            }

            foreach (var sl in spell_lists)
            {
                sl.GetComponent<LearnSpells>().Spells = sl.GetComponent<LearnSpells>().Spells.AddToArray(grease, glitterdust, stinking_cloud, acid_pit);
            }

            var brain = library.Get<BlueprintBrain>("fde24a9130c94f74baa7f166ca1b8fcb");


            
            for (int i = 0; i < spells_to_cast.Length; i++)
            {
                var ai_action = library.CopyAndAdd<BlueprintAiCastSpell>("d33950abdb291564696f29302a022faa", spells_to_cast[i].name + "BanditConjurerAiAction", "");
                ai_action.Ability = spells_to_cast[i];
                ai_action.BaseScore = 3 + i;
                brain.Actions = brain.Actions.AddToArray(ai_action);
            }
        }


        static void fixVarraskInquisitor()
        {
            var inquisitor = library.Get<BlueprintUnit>("685baae218ee68e41890342f197a9156");
            var class_levels = inquisitor.GetComponent<AddClassLevels>();
            class_levels.Selections[0].Features[0] = library.Get<BlueprintFeature>("121811173a614534e8720d7550aae253"); //shield bash
            class_levels.Selections[0].Features[3] = library.Get<BlueprintFeature>("ac8aaf29054f5b74eb18f2af950e752d"); //twf
            inquisitor.Body.PrimaryHand = library.Get<BlueprintItemWeapon>("684ff52e687aacf43a3f673e42b9c957"); //flaming ls +2
            inquisitor.Body.SecondaryHand = library.Get<BlueprintItemShield>("3b3f25ba61ba5f346b4d546e702732cb"); //light shield + 3
            class_levels.Selections[0].Features[5] = library.Get<BlueprintFeature>("9af88f3ed8a017b45a6837eab7437629"); //improved twf

            class_levels.SelectSpells = class_levels.SelectSpells.AddToArray(library.Get<BlueprintAbility>("ef16771cb05d1344989519e87f25b3c5"));//add divine power

            var brain = inquisitor.Brain;
            brain.Actions[1] = library.Get<BlueprintAiCastSpell>("09de02db1b07d364795f412abb557de3"); //replace shield with divine power

            var buffs = library.Get<BlueprintFeature>("d63d5ef2907eb73419fed27db0bcbb70");
            buffs.AddComponent(Helpers.CreateAddStatBonus(StatType.AC, 4, ModifierDescriptor.Deflection)); //shield of faith
            //buffs.CreateAddFact(library.Get<BlueprintFeature>("63bdc7fca5acbc749aa9e3cfffb53f11")); improved critical longsword
        }

        static void updateGoblinShaman()
        {
            var goblin_shaman = library.Get<BlueprintUnit>("8421b6137d7765947958973526b5249b");
            /*goblin_shaman.AddFacts = new Kingmaker.Blueprints.Facts.BlueprintUnitFact[] { library.Get<BlueprintFeature>("9d168ca7100e9314385ce66852385451"),
                                                                                          library.Get<BlueprintBuff>("33d5368d93d949ecbd82b3528750b5a0"), //aura of doom
                                                                                          library.Get<BlueprintBuff>("5274ddc289f4a7447b7ace68ad8bebb0"), //shield of faith
                                                                                          library.Get<BlueprintFeature>("3adb2c906e031ee41a01bfc1d5fb7eea"), //channel negative
                                                                                        };*/
            //will change to shaman (speaker for the past?)
            var spell_focus = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            var class_levels = goblin_shaman.GetComponent<AddClassLevels>();

            var feat_selection = new SelectionEntry();
            feat_selection.Selection = library.Get<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");//basic feat selection
            feat_selection.Features = new BlueprintFeature[]
            {
                library.Get<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff"), //1 - selective channel
                spell_focus, //3 - spell focus conjuration
                spell_focus, //5 - spell focus evocation
                library.Get<BlueprintFeature>("06964d468fde1dc4aa71a92ea04d930d"), //7 - combat casting
                library.Get<BlueprintFeature>("38155ca9e4055bb48a89240a2055dcc3"), //9 - augment summoning
                library.Get<BlueprintFeature>("0477936c0f74841498b5c8753a8062a3"), //11 -  superior summoning
                ChannelEnergyEngine.quick_channel, //13 - weapon focus
            };


            var domain_selection = createFeatureSelection(library.Get<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9"),
                                                          library.Get<BlueprintFeature>("8d454cbb7f25070419a1c8eaf89b5be5")); //war

            var domain_selection2 = createFeatureSelection(library.Get<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e"),
                                              library.Get<BlueprintFeature>("82b654d68ea6ce143be5f7df646d6385")); //evil

            var war_domain_feat = createFeatureSelection(library.Get<BlueprintFeatureSelection>("79c6421dbdb028c4fa0c31b8eea95f16"),
                                                         library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e")); //weapon focus

            class_levels.Selections = new SelectionEntry[] { feat_selection, domain_selection, domain_selection2,
                                                            createSpellFocusSelection(SpellSchool.Conjuration),
                                                            createSpellFocusSelection(SpellSchool.Evocation),
                                                            war_domain_feat,
                                                            createWeaponFocusSelection(WeaponCategory.Spear)
                                                           };

            var flame_strike = library.Get<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");

            class_levels.MemorizeSpells = new BlueprintAbility[]
            {
                //level 1
                //level 2
                Spells.owls_wisdom,
                Spells.bulls_strength,
                NewSpells.bone_fists,
                //level 3
                Spells.prayer,
                //level 4
                NewSpells.aura_of_doom,
                library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0"), //fear
                Spells.divine_power,
                Spells.divine_power,

                //level 5
                CallOfTheWild.NewSpells.command_greater,
                flame_strike,
                flame_strike,
                flame_strike,
                flame_strike,

                //level 6
                library.Get<BlueprintAbility>("e740afbab0147944dab35d83faa0ae1c"), //sm 6
                Spells.cold_ice_strike,
                Spells.cold_ice_strike,
                library.Get<BlueprintAbility>("36c8971e91f1745418cc3ffdfac17b74"), //blade barrier

                //level 7
                library.Get<BlueprintAbility>("d3ac756a229830243a72e84f3ab050d0"), //sm7
                library.Get<BlueprintAbility>("0cea35de4d553cc439ae80b3a8724397"), //cure serious wounds mass
                library.Get<BlueprintAbility>("bd10c534a09f44f4ea632c8b8ae97145"), //blashphemy
            };

            //opens with blashphemy
            //than command
            //blade barrier
            //flame strike
            //sm7
            //cloak of dreams if surrounded, bane enemy

            //every turn if possible casts swift cold ice strike and quick channel negative
            
            //cure serious wounds mass 

            var cast_blade_barrier = library.CopyAndAdd<BlueprintAiCastSpell>("4f48fd03d530f86439657e4d93bffc89", "ShamanBladeBarrierAiAction", ""); //flame strike
            cast_blade_barrier.Ability = library.Get<BlueprintAbility>("36c8971e91f1745418cc3ffdfac17b74");
            cast_blade_barrier.BaseScore = 9.0f;

            var cast_fear = library.CopyAndAdd<BlueprintAiCastSpell>("4429f17c883c8e246b645553d8293da5", "ShamanFearAiAction", ""); //fear
            cast_fear.Ability = library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");
            cast_fear.BaseScore = 5.0f;

            var cast_blasphemy = library.CopyAndAdd<BlueprintAiCastSpell>("4f48fd03d530f86439657e4d93bffc89", "ShamanBlasphemyAiAction", "");
            cast_blasphemy.BaseScore = 12.0f;
            cast_blasphemy.Ability = library.Get<BlueprintAbility>("bd10c534a09f44f4ea632c8b8ae97145");




            var high_priestess_brain = library.Get<BlueprintBrain>("7c94ef2470c715344bd3cdff1a6feae0");

            var new_actions = high_priestess_brain.Actions.Skip(6).ToArray().AddToArray(cast_blade_barrier, cast_fear, cast_blasphemy,
                                                                      library.Get<BlueprintAiAction>("1c5cdf69effdbdf4e91aa7fa8e36a261"), //sm6
                                                                       library.Get<BlueprintAiAction>("1dd4f2a2fb786714eaca3a94a03e8c5f"), //sm7
                                                                      library.Get<BlueprintAiAction>("866ffa6c34000cd4a86fb1671f86c7d8")); //attack
            goblin_shaman.Brain.Actions = new_actions.AddToArray(AiActions.cleric_precast_spells_ai_action);
            goblin_shaman.AddFacts = goblin_shaman.AddFacts.AddToArray(AiActions.cleric_free_cast_long_spells);

        }


        static void updateAttackConsiderations()
        {
            var ac_consideration = Helpers.Create<NewConsiderations.AcConsideration>();
            ac_consideration.name = "ACConsideration";
            library.AddAsset(ac_consideration, "");

            var attack_actions = library.GetAllBlueprints().OfType<BlueprintAiAction>().Where(f => f.name.Contains("AttackAiAction")).ToArray();

            foreach (var attack_action in attack_actions)
            {
                attack_action.TargetConsiderations = attack_action.TargetConsiderations.AddToArray(ac_consideration);
            }
        }

        static void updateGoblinFighter()
        {
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinFighterFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

            var wf = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var ws = library.Get<BlueprintParametrizedFeature>("31470b17e8446ae4ea0dacd6c5817d86");
            var gwf = library.Get<BlueprintParametrizedFeature>("09c9e82965fb4334b984a1e9df3bd088");
            var ic = library.Get<BlueprintParametrizedFeature>("f4201c85a991369408740c6888362e20");

            var wt_close = library.Get<BlueprintFeature>("bd75a95b36a3cd8459513ee1932c8c22");

            foreach (var feature in features)
            {
                var levels = feature.GetComponent<AddClassLevels>();
                var selections = levels.Selections;
                selections[2].Features = selections[2].Features.AddToArray(wt_close); //give fighters bonus to shields
                //selections[0].Features[1] = ProperFlanking20.NewFeats.low_profile; // replace improved initiative with low profile - they are already small, so should not provide cover
                selections[0].Features[0] = wf;
                selections[0].Features[4] = ic;

                selections[1].Features[3] = ws;
                selections[1].Features[4] = gwf;

                var wf_selection = new SelectionEntry();
                wf_selection.ParametrizedFeature = wf;
                wf_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortsword;
                wf_selection.IsParametrizedFeature = true;
                levels.Selections = levels.Selections.AddToArray(wf_selection);

                var ws_selection = new SelectionEntry();
                ws_selection.ParametrizedFeature = ws;
                ws_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortsword;
                ws_selection.IsParametrizedFeature = true;
                levels.Selections = levels.Selections.AddToArray(ws_selection);
                

                var gwf_selection = new SelectionEntry();
                gwf_selection.ParametrizedFeature = gwf;
                gwf_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortsword;
                gwf_selection.IsParametrizedFeature = true;
                levels.Selections = levels.Selections.AddToArray(gwf_selection);

                var ic_selection = new SelectionEntry();
                ic_selection.ParametrizedFeature = ic;
                ic_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortsword;
                ic_selection.IsParametrizedFeature = true;
                levels.Selections = levels.Selections.AddToArray(ic_selection);
            }
        }


        static void updateGoblinArcher()
        {
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinArcherFeatureListLevel")).ToArray();

            var wf = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var ws = library.Get<BlueprintParametrizedFeature>("31470b17e8446ae4ea0dacd6c5817d86");
            var volley_fire = library.Get<BlueprintFeature>("c4b555225f565bb40a855c1bfeeff07e");
            var combat_trick = library.Get<BlueprintFeatureSelection>("c5158a6622d0b694a99efb1d0025d2c1");
            var many_shot = library.Get<BlueprintFeature>("adf54af2a681792489826f7fd1b62889");
            var deadly_aim = library.Get<BlueprintFeature>("f47df34d53f8c904f9981a3ee8e84892");

            foreach (var feature in features)
            {
                var fighter_levels = feature.GetComponent<AddClassLevels>();                
                var selections = fighter_levels.Selections;
                selections[1].Features[1] = wf;
                selections[1].Features[2] = ws;
                selections[0].Features = selections[0].Features.AddToArray(deadly_aim);

                var wf_selection = new SelectionEntry();
                wf_selection.ParametrizedFeature = wf;
                wf_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortbow;
                wf_selection.IsParametrizedFeature = true;
                fighter_levels.Selections = fighter_levels.Selections.AddToArray(wf_selection);

                var ws_selection = new SelectionEntry();
                ws_selection.ParametrizedFeature = ws;
                ws_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Shortbow;
                ws_selection.IsParametrizedFeature = true;
                fighter_levels.Selections = fighter_levels.Selections.AddToArray(ws_selection);

                var levels = feature.GetComponents<AddClassLevels>().ToArray();

                if (levels.Length > 1)
                {
                    var levels_rogue = levels[1];
                    var selections_rogue = levels_rogue.Selections;
                    selections_rogue[0].Features[1] = many_shot;
                    selections[1].Features[0] = combat_trick;

                    var trick_entry = new SelectionEntry();
                    trick_entry.Features = new BlueprintFeature[] { volley_fire };
                    trick_entry.Selection = combat_trick;
                    levels_rogue.Selections = levels_rogue.Selections.AddToArray(trick_entry);
                }
            }
        }


        static void updateGoblinRogue()
        {
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinRogueFeatureListLevel")).ToArray();

            var combat_trick = library.Get<BlueprintFeatureSelection>("c5158a6622d0b694a99efb1d0025d2c1");
            var slow_reactions = library.Get<BlueprintFeature>("7787030571e87704d9177401c595408e");
            var opportunist = library.Get<BlueprintFeature>("5bb6dc5ce00550441880a6ff8ad4c968");
            var wf = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            foreach (var feature in features)
            {
                var levels = feature.GetComponent<AddClassLevels>();
                var selections = levels.Selections;
                selections[1].Features[0] = wf;
                selections[1].Features[1] = ProperFlanking20.RogueTalents.underhanded; //replace dodge with underhanded trick
                selections[1].Features[2] = combat_trick; // replace stealthy with combat trick dirty fighting
                selections[1].Features[3] = slow_reactions;
                selections[1].Features = selections[1].Features.AddToArray(opportunist); //opportunist if level 10
                selections[0].Features[4] = ProperFlanking20.NewFeats.quick_dirty_trick; //replace twf greater which is usnelectable with quick dirty trick

                var trick_entry = new SelectionEntry();
                trick_entry.Features = new BlueprintFeature[] { ProperFlanking20.NewFeats.dirty_fighting };
                trick_entry.Selection = combat_trick;

                levels.Selections = levels.Selections.AddToArray(trick_entry);

                var wf_selection = new SelectionEntry();
                wf_selection.ParametrizedFeature = wf;
                wf_selection.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Dagger;
                wf_selection.IsParametrizedFeature = true;
                levels.Selections = levels.Selections.AddToArray(wf_selection);
            }

            var units = library.GetAllBlueprints().OfType<BlueprintUnit>().Where(f => f.name.Contains("_GoblinRogue")).ToArray();
            foreach (var u in units)
            {
                var levels = u.GetComponent<AddClassLevels>();
                levels.Selections = features[0].GetComponent<AddClassLevels>().Selections;
            }
        }


        static void updateAlchemist()
        {
            var spells = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinAlchemistSpellListLevel") || f.name.Contains("BanditAlchemistSpellListLevel")).Cast<BlueprintFeature>().ToArray();


            var haste = library.Get<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98");
            foreach (var s in spells)
            {
                var learn = s.GetComponent<LearnSpells>();
                learn.Spells = learn.Spells.AddToArray(haste);
            }

            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where(f => f.name.Contains("GoblinAlchemistFeatureListLevel") || f.name.Contains("BanditAlchemistFeatureListLevel")).ToArray();

            var chocking_bombs = library.Get<BlueprintFeature>("b3c6cb76d5b11cf4c8314d7b1c7b9b8b");

            foreach (var feature in features)
            {
                var levels = feature.GetComponent<AddClassLevels>();
                levels.MemorizeSpells[7] = haste; //replace displacement with haste
                var selections = levels.Selections;
                selections[1].Features = selections[1].Features.AddToArray(chocking_bombs); //add chocking bombs
            }
            var brains = new BlueprintBrain[]{library.Get<BlueprintBrain>("53a68e3631a6e1646997de0cb50ba49f"), //goblin
                                              library.Get<BlueprintBrain>("b7853e33e6e32d94aac4cd2375547e23") //bandit
                                              };

            var use_chocking_bomb = library.Get<BlueprintAiCastSpell>("9efb8b70ddb084445a2be809e8271259");
            var use_haste = library.Get<BlueprintAiCastSpell>("c2030f9e42b7e3d4fb08f6f05c68eae1");


            foreach (var b in brains)
            {
                b.Actions = b.Actions.AddToArray(use_chocking_bomb, use_haste);
            }
        }


        static void fixFallenPriest()
        {
            var priest = library.Get<BlueprintUnit>("f0c4eafde7038e1488f54905e6846fc3");
            priest.AddFacts = priest.AddFacts.AddToArray(library.Get<BlueprintFeature>("3adb2c906e031ee41a01bfc1d5fb7eea")); //channel negative
            priest.Charisma = 14;

            var spell_focus = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            var greater_spell_focus = library.Get<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");
            var class_levels = priest.GetComponent<AddClassLevels>();


            var feat_selection = new SelectionEntry();
            feat_selection.Selection = library.Get<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");//basic feat selection
            feat_selection.Features = new BlueprintFeature[]
            {
                library.Get<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff"), //1 - selective channel
                spell_focus, //3 - spell focus enchantment
                spell_focus, //5 - spell focus necromancy
                library.Get<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e"), //7 - quick spell
                greater_spell_focus, //9 - gsf - enchantment
                greater_spell_focus, //11 - gsf - necromancy
                ChannelEnergyEngine.quick_channel, //13
                ChannelEnergyEngine.improved_channel
            };
            class_levels.Skills = new StatType[] { StatType.SkillLoreReligion, StatType.SkillPerception};


            var domain_selection = createFeatureSelection(library.Get<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9"),
                                                          library.Get<BlueprintFeature>("07854f99c8d029b4cbfdf6ae6c7bc452")); //strength

            var domain_selection2 = createFeatureSelection(library.Get<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e"),
                                              library.Get<BlueprintFeature>("9ebe166b9b901c746b1858029f13a2c5")); //maddness


            class_levels.Selections = new SelectionEntry[] { feat_selection, domain_selection, domain_selection2,
                                                            createSpellFocusSelection(SpellSchool.Enchantment),
                                                            createSpellFocusSelection(SpellSchool.Necromancy),
                                                            createGreaterSpellFocusSelection(SpellSchool.Enchantment),
                                                            createGreaterSpellFocusSelection(SpellSchool.Necromancy)
                                                           };

            //uses quick channel
            //aura of madness
            //rift of ruin
            //destruction, boneshatter
            var brain = priest.Brain;
            brain.Actions = brain.Actions.AddToArray(AiActions.quick_channel_action);
        }




        static void fixSaves()
        {
            /*Action<UnitDescriptor> ai_fix = delegate (UnitDescriptor u)
            {
                fixAiOnLoad(u);
            };
            SaveGameFix.save_game_actions.Add(ai_fix);*/
        }


        static public void fixAiOnLoad(UnitDescriptor u)
        {
            if (u.IsPlayerFaction)
            {
                return;
            }
            
            //var tr = Harmony12.Traverse.Create(u);
            //tr.Property("Brain").SetValue(new UnitBrain(u));
            //u.Initialize();

            var acls = u.Blueprint.GetComponents<AddClassLevels>().ToArray();
            foreach (var f in u.Blueprint.AddFacts)
            {
                acls = acls.AddToArray(f.GetComponents<AddClassLevels>());
            }
            Main.logger.Log("Found acls: " + acls.Length.ToString());
            //add new facts
            /*foreach (BlueprintUnitFact bf in (u.Blueprint.AddFacts).EmptyIfNull<BlueprintUnitFact>())
            {
                if (bf != null && !u.HasFact(bf))
                {
                    u.AddFact(bf, (MechanicsContext)null, (FeatureParam)null);
                }
            }*/

            foreach (var acl in acls)
            {
                //add spells if they have changed
                var sb = u.GetSpellbook(acl.CharacterClass);
                if (sb == null)
                {
                    continue;
                }

                if (!sb.Blueprint.Spontaneous)
                {
                    var slots = sb.GetAllMemorizedSpells().ToArray();
                    foreach (var s in slots)
                    {
                        sb.ForgetMemorized(s);
                    }
                }
                acl.LevelUp(u, acl.Levels - u.Progression.GetClassLevel(acl.CharacterClass));
            }


            u.Brain.RestoreAvailableActions();
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


        static BuffConsideration createNoBuffConsideration(string name, params BlueprintBuff[] buffs)
        {
            var no_buff = library.CopyAndAdd<BuffConsideration>("881bcb09ccf53074d88c5dec9f2fcaa9", name, "");
            no_buff.Buffs = buffs;

            return no_buff;
        }


        static SelectionEntry createSpellFocusSelection(SpellSchool school)
        {
            var spell_focus = new SelectionEntry();
            spell_focus.IsParametrizedFeature = true;
            spell_focus.ParametrizedFeature = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe"); ;
            spell_focus.ParamSpellSchool = school;

            return spell_focus;
        }


        static SelectionEntry createGreaterSpellFocusSelection(SpellSchool school)
        {
            var spell_focus = new SelectionEntry();
            spell_focus.IsParametrizedFeature = true;
            spell_focus.ParametrizedFeature = library.Get<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf"); ;
            spell_focus.ParamSpellSchool = school;
            return spell_focus;
        }


        static SelectionEntry createWeaponFocusSelection(WeaponCategory weapon_category)
        {
            var weapon_focus = new SelectionEntry();
            weapon_focus.IsParametrizedFeature = true;
            weapon_focus.ParametrizedFeature = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e"); ;
            weapon_focus.ParamWeaponCategory = weapon_category;

            return weapon_focus;
        }


        static SelectionEntry createFeatureSelection(BlueprintFeatureSelection selection, params BlueprintFeature[] features)
        {
            var selection_entry = new SelectionEntry();
            selection_entry.Selection = selection;
            selection_entry.Features = features;

            return selection_entry;
        }


        static BlueprintFeature BuffToFeature(BlueprintBuff buff)
        {
            var feature = Helpers.CreateFeature(buff.name + "Feature",
                                                buff.Name,
                                                buff.Description,
                                                "",
                                                buff.Icon,
                                                FeatureGroup.None,
                                                buff.ComponentsArray
                                                );
            return feature;
        }

        static void updateTsanna()
        {
            var units = new BlueprintUnit[] {library.Get<BlueprintUnit>("7c3e0ecea7956be46ad5d74e9b3fd4fb"),
                                             library.Get<BlueprintUnit>("07e607f30d7de6c49a002339211d074f"),
                                             library.Get<BlueprintUnit>("bd4bace18805d9f4e89821e7a4f0b173"),
                                             library.Get<BlueprintUnit>("cf68a7bc6251d754d8ccd27f4dc59be8"),
                                             library.Get<BlueprintUnit>("61bc44f3224a0c7449dc8e28c7cf3b9b")
                                            };

            var slay_living = library.Get<BlueprintAbility>("4fbd47525382517419c66fb548fe9a67");
            var flame_strike = library.Get<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");
            var inflict_critical_wounds = library.Get<BlueprintAbility>("651110ed4f117a948b41c05c5c7624c0");
            foreach (var u in units)
            {
                //u.AddFacts = u.AddFacts.AddToArray(AiActions.cleric_free_cast_long_spells);
                var class_levels = u.GetComponent<AddClassLevels>();
                class_levels.Skills = new StatType[] { StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception };
                var spells = class_levels.MemorizeSpells.ToList();
                spells.Add(Spells.bulls_strength);
                spells.Add(Spells.owls_wisdom);
                spells.Add(NewSpells.bone_fists);
                spells.Remove(inflict_critical_wounds);
                spells.Remove(inflict_critical_wounds);
                spells.Remove(inflict_critical_wounds);
                spells.Remove(inflict_critical_wounds);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Add(NewSpells.aura_of_doom);
                spells.Add(CallOfTheWild.NewSpells.command_greater);
                spells.Add(CallOfTheWild.NewSpells.command_greater);
                spells.Add(flame_strike);
                spells.Add(flame_strike);
                spells.Add(flame_strike);
                spells.Add(Spells.cold_ice_strike);
                spells.Add(Spells.cold_ice_strike);
                spells.Add(Spells.cold_ice_strike);
                class_levels.MemorizeSpells = spells.ToArray();
                class_levels.Selections[2].Features[1] = ChannelEnergyEngine.quick_channel;
                class_levels.Selections[3].ParamSpellSchool = Kingmaker.Blueprints.Classes.Spells.SpellSchool.Enchantment;
                class_levels.Selections[4].ParamSpellSchool = Kingmaker.Blueprints.Classes.Spells.SpellSchool.Enchantment;

                //class_levels.Selections[6].ParamSpellSchool = Kingmaker.Blueprints.Classes.Spells.SpellSchool.Necromancy;
                // class_levels.Selections[7].ParamSpellSchool = Kingmaker.Blueprints.Classes.Spells.SpellSchool.Necromancy;
            }

            var brain = units[0].Brain;

            var cast_flame_strike = library.CopyAndAdd<BlueprintAiCastSpell>("4f48fd03d530f86439657e4d93bffc89", "CastFlameStrikePriority9SpellAiAction", "");
            cast_flame_strike.BaseScore = 9.0f;
            
            brain.Actions = brain.Actions.AddToArray(cast_flame_strike, AiActions.cast_command, AiActions.quick_channel_action, AiActions.cast_cold_ice_strike).AddToArray(AiActions.cleric_precast_spells_ai_action);
        }


        //fix unit spells/brains on area loaded
        [Harmony12.HarmonyPatch(typeof(Game), "OnAreaLoaded")]
        class Game_OnAreaLoaded_Patch
        {
            static void Postfix(Game __instance)
            {
                foreach (var u in __instance.State.AwakeUnits)
                {
                    if (u.Descriptor == null)
                    {
                        continue;
                    }
                    fixAiOnLoad(u.Descriptor);
                    Main.logger.Log("Fixing " + u.CharacterName);
                }
            }
        }
    }

}
