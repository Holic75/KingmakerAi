using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingmakerAI
{
    class Core
    {
        static LibraryScriptableObject library = Main.library;
        static internal void load()
        {
            Profiles.ProfileManager.initialize();
            updateAttackConsiderations();
            fixBanditTransmuters();
            fixMaestroJanush();
            fixBanditConjurers();
            fixBanditNecromancers();
            fixDLC2CultistNecromancerBoss();
            fixBanditIllusionists();
            fixDruids();
            fixKoboldsDragonSorcerers();
            fixKoboldsUndeadSorcerers();
            fixDryads();
            fixAlchemists();
            fixClericCasters();
            fixMeleeClerics();
            fixBards();
            fixGoblinShaman();
            fixDrawEldritchArcher();

            fixAbilities();
        }


        static void fixDrawEldritchArcher()
        {
            var eldritch_archer = Profiles.ProfileManager.getProfile("EldritchArcher");
            var draw_magus_feature = library.Get<BlueprintFeature>("2fbadb17e2953d34da6684c3e75b5536");
            var draw_magus_brain = library.Get<BlueprintBrain>("3bd2bbf70d1ee8449910750aeee53c93");
            draw_magus_brain.Actions = eldritch_archer.brain.Actions;
            var weapon_selections = new SelectionEntry[]
            {
                Profiles.ProfileManager.createParametrizedFeatureSelection(Profiles.ProfileManager.Feats.weapon_focus, Kingmaker.Enums.WeaponCategory.LightCrossbow),
                Profiles.ProfileManager.createParametrizedFeatureSelection(Profiles.ProfileManager.Feats.weapon_specialization, Kingmaker.Enums.WeaponCategory.LightCrossbow),
            };


            var old_acl = draw_magus_feature.GetComponent<AddClassLevels>();
            Profiles.ProfileManager.replaceAcl(old_acl, eldritch_archer.getAcl(old_acl.Levels, weapon_selections));
            draw_magus_feature.RemoveComponents<AddFacts>();
            draw_magus_feature.AddComponent(Helpers.CreateAddFacts(eldritch_archer.getFeatures(old_acl.Levels)));
        }

        static void fixAbilities()
        {
            //imitate a potion
            var enlarge_self = library.Get<BlueprintAbility>("549e9fcaadc861348b05bf01624387aa");
            enlarge_self.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
        }


        static void fixGoblinShaman()
        {
            var cleric_fighter = Profiles.ProfileManager.getProfile("ClericMelee");
            var goblin_shaman = library.Get<BlueprintUnit>("8421b6137d7765947958973526b5249b");
            goblin_shaman.Brain.Actions = cleric_fighter.brain.Actions;

            var shaman_selections1 = new SelectionEntry[]
            {
                Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.deity_selection, CallOfTheWild.Deities.lamashtu),
                Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection, Profiles.ProfileManager.Domains.strength),
                Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection2, Profiles.ProfileManager.Domains.chaos2),
            };

            goblin_shaman.Body.Armor = library.Get<BlueprintItemArmor>("fb2664f7d8534dc40aeb23392dc58c0c"); //mithral chainshirt +3 to make use of high dex
            goblin_shaman.Strength = 16;
            goblin_shaman.Constitution = 16;
            var old_acl = goblin_shaman.GetComponent<AddClassLevels>();
            Profiles.ProfileManager.replaceAcl(old_acl, cleric_fighter.getAcl(old_acl.Levels, shaman_selections1));
            goblin_shaman.RemoveComponents<AddFacts>();
            goblin_shaman.AddFacts = goblin_shaman.AddFacts.AddToArray(cleric_fighter.getFeatures(old_acl.Levels));
            goblin_shaman.Body.Neck = library.Get<BlueprintItemEquipmentNeck>("081a2ffe763320a469de20f1e9b1cd71"); //amulet of natural armor +3
            goblin_shaman.Body.Shoulders = library.Get<BlueprintItemEquipmentShoulders>("9f3c56d5247154e47b5ca9500f4d86ce"); //cloak of resistance +3
        }


        static void fixDryads()
        {
            var fey_sorcerer = Profiles.ProfileManager.getProfile("SorcererFey");
            {
                var bloodmoon_dryad = library.Get<BlueprintUnit>("82a6be0ba1d061243848871ce2feda27");   
                bloodmoon_dryad.Brain.Actions = fey_sorcerer.brain.Actions;

                var acl = bloodmoon_dryad.GetComponents<AddClassLevels>().Where(a => a.CharacterClass == Profiles.ProfileManager.Classes.sorceror).FirstOrDefault();
                bloodmoon_dryad.AddFacts = bloodmoon_dryad.AddFacts.AddToArray(fey_sorcerer.getFeatures(acl.Levels));
                Profiles.ProfileManager.replaceAcl(acl, fey_sorcerer.getAcl(acl.Levels));
            }

            {
                var lady_of_shallows = library.Get<BlueprintUnit>("ac131b0101870b6489609a7f33b5e576");
                lady_of_shallows.Brain.Actions = fey_sorcerer.brain.Actions;

                var acl = lady_of_shallows.GetComponents<AddClassLevels>().Where(a => a.CharacterClass == Profiles.ProfileManager.Classes.sorceror).FirstOrDefault();
                lady_of_shallows.AddFacts = lady_of_shallows.AddFacts.AddToArray(fey_sorcerer.getFeatures(acl.Levels));
                Profiles.ProfileManager.replaceAcl(acl, fey_sorcerer.getAcl(acl.Levels));
                lady_of_shallows.Body.QuickSlots[0] = library.Get<BlueprintItemEquipmentUsable>("55a059b32df920c4abe65b8ee8b56056"); //rod of quicken metamagic lesser
                var auto_quicken_metamagic = library.Get<BlueprintFeature>("d26acdac8ded95b44b787c9700634fc9");
                auto_quicken_metamagic.GetComponent<AutoMetamagic>().Abilities.AddRange(new BlueprintAbility[] { Profiles.ProfileManager.Spells.fireball, Profiles.ProfileManager.Spells.slow });
                auto_quicken_metamagic.AddComponent(Helpers.Create<IncreaseSpellDC>(i => { i.BonusDC = 2; i.Spell = Profiles.ProfileManager.Spells.confusion; }));
            }

  
            {
                var nixie_sorcerer = library.Get<BlueprintUnit>("7fd2ae7369b28e3489861407df3984ae");
                nixie_sorcerer.Brain.Actions = fey_sorcerer.brain.Actions;

                var acl = nixie_sorcerer.GetComponents<AddClassLevels>().Where(a => a.CharacterClass == Profiles.ProfileManager.Classes.sorceror).FirstOrDefault();
                nixie_sorcerer.AddFacts = nixie_sorcerer.AddFacts.AddToArray(fey_sorcerer.getFeatures(acl.Levels));
                Profiles.ProfileManager.replaceAcl(acl, fey_sorcerer.getAcl(acl.Levels));
            }

            var undead_sorcerer = Profiles.ProfileManager.getProfile("SorcererUndead");
            {
                var bloodmoon_nymph = library.Get<BlueprintUnit>("0bdeff53fd8249d478ff8276eb8a1658");
                bloodmoon_nymph.Brain.Actions = undead_sorcerer.brain.Actions.AddToArray(library.Get<BlueprintAiCastSpell>("4aa53873b28d3d148bde2f3828758d33"));

                var acl = bloodmoon_nymph.GetComponents<AddClassLevels>().Where(a => a.CharacterClass == Profiles.ProfileManager.Classes.sorceror).FirstOrDefault();
                acl.Levels = 9; //increase from 6
                bloodmoon_nymph.AddFacts = bloodmoon_nymph.AddFacts.AddToArray(undead_sorcerer.getFeatures(acl.Levels));
                Profiles.ProfileManager.replaceAcl(acl, undead_sorcerer.getAcl(acl.Levels));
            }



        }



        static void fixBards()
        {
            var bard_melee = Profiles.ProfileManager.getProfile("BardMelee");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditBardFeatureListLevel")).ToArray();
            var brain = library.Get<BlueprintBrain>("424d2726bc862a04bb31180be3661013");
            brain.Actions = bard_melee.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                Profiles.ProfileManager.replaceAcl(old_acl, bard_melee.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(bard_melee.getFeatures(old_acl.Levels)));
            }


            var bard_ranged = Profiles.ProfileManager.getProfile("BardArcher");
            features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("KoboldBardFeatureListLevel")).ToArray();
            brain = library.Get<BlueprintBrain>("542f43821b9db194d987c23bbff3e664");
            brain.Actions = bard_ranged.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                Profiles.ProfileManager.replaceAcl(old_acl, bard_ranged.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(bard_ranged.getFeatures(old_acl.Levels)));
            }
            //irovetti?

        }


        static void fixMeleeClerics()
        {
            var cleric_melee = Profiles.ProfileManager.getProfile("ClericMelee");
            var cyclops = library.GetAllBlueprints().OfType<BlueprintUnit>().OfType<BlueprintUnit>().Where(f => f.name.Contains("CyclopCleric")).ToArray();

            var brain = library.Get<BlueprintBrain>("57e33f7b6b629454ba8144b600816379");
            brain.Actions = cleric_melee.brain.Actions;

            var cleric_selections1 = new SelectionEntry[]
            {
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.deity_selection, Profiles.ProfileManager.Deities.gorum),
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection, Profiles.ProfileManager.Domains.war),
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection2, Profiles.ProfileManager.Domains.strength2),
            };

            foreach (var u in cyclops)
            {
                var old_acl = u.GetComponents<AddClassLevels>().Where(a => a.CharacterClass == library.Get<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0")).FirstOrDefault();
                if (old_acl == null)
                {
                    continue;
                }
                Profiles.ProfileManager.replaceAcl(old_acl, cleric_melee.getAcl(old_acl.Levels, cleric_selections1));
               
                u.AddFacts = u.AddFacts.AddToArray(cleric_melee.getFeatures(old_acl.Levels));
            }
        }

        static void fixClericCasters()
        {
            var cleric_caster = Profiles.ProfileManager.getProfile("ClericCasterPositive");
            var cleric_caster_negative = Profiles.ProfileManager.getProfile("ClericCasterNegative");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditPositiveClericFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("a19c889be3392b24a9890ffe5a196f0e");
            brain.Actions = cleric_caster.brain.Actions;

            var cleric_selections1 = new SelectionEntry[]
            {
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.deity_selection, Profiles.ProfileManager.Deities.gorum),
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection, Profiles.ProfileManager.Domains.glory),
               Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection2, Profiles.ProfileManager.Domains.chaos2),
            };

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                Profiles.ProfileManager.replaceAcl(old_acl, cleric_caster.getAcl(old_acl.Levels, cleric_selections1));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(cleric_caster.getFeatures(old_acl.Levels)));
            }


            //tsanna
            {
                var cunning_initiative = library.Get<BlueprintFeature>("6be8b4031d8b9fc4f879b72b5428f1e0");
                var tsanna_units = new BlueprintUnit[] {library.Get<BlueprintUnit>("7c3e0ecea7956be46ad5d74e9b3fd4fb"),
                                             library.Get<BlueprintUnit>("07e607f30d7de6c49a002339211d074f"),
                                             library.Get<BlueprintUnit>("bd4bace18805d9f4e89821e7a4f0b173"),
                                             library.Get<BlueprintUnit>("cf68a7bc6251d754d8ccd27f4dc59be8"),
                                             library.Get<BlueprintUnit>("61bc44f3224a0c7449dc8e28c7cf3b9b"),
                                              library.Get<BlueprintUnit>("546e1f3739476cd43aeb160cb2344320")
                                            };
                var tsanna_brain = tsanna_units[0].Brain;
                tsanna_brain.Actions = cleric_caster_negative.brain.Actions;
                var tsanna_selections1 = new SelectionEntry[]
                {
                    Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.deity_selection, CallOfTheWild.Deities.lamashtu),
                    Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection, Profiles.ProfileManager.Domains.evil),
                    Profiles.ProfileManager.createFeatureSelection(Profiles.ProfileManager.FeatSelections.domain_selection2, Profiles.ProfileManager.Domains.chaos2),
                };

                foreach (var u in tsanna_units)
                {
                    var old_acl = u.GetComponent<AddClassLevels>();
                    Profiles.ProfileManager.replaceAcl(old_acl, cleric_caster_negative.getAcl(old_acl.Levels, tsanna_selections1));
                    u.RemoveComponents<AddFacts>();
                    u.AddFacts = u.AddFacts.AddToArray(cleric_caster_negative.getFeatures(old_acl.Levels));
                    u.AddFacts = u.AddFacts.AddToArray(cunning_initiative);
                    u.Body.Armor = library.Get<BlueprintItemArmor>("ef5ee1c481c0139438a7097868685a88");//replace her standard breastpalte with mithral brestplate +3
                    u.Body.Neck = library.Get<BlueprintItemEquipmentNeck>("081a2ffe763320a469de20f1e9b1cd71"); //amulet of natural armor +3
                    u.Body.Shoulders = library.Get<BlueprintItemEquipmentShoulders>("9f3c56d5247154e47b5ca9500f4d86ce"); //cloak of resistance +3
                }
            }

        }


        static void fixAlchemists()
        {
            var alchemist = Profiles.ProfileManager.getProfile("Alchemist");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinAlchemistFeatureListLevel")
                                                                                                                    || f.name.Contains("BanditAlchemistFeatureListLevel")
                                                                                                                    || f.name.Contains("KoboldAlchemistFeatureListLevel")).ToArray();

            var brains = new BlueprintBrain[]
            {
                library.Get<BlueprintBrain>("53a68e3631a6e1646997de0cb50ba49f"), // goblin
                library.Get<BlueprintBrain>("5374487ceea5ac04d907d301cc53ae38"), //kobold
                library.Get<BlueprintBrain>("b7853e33e6e32d94aac4cd2375547e23"), //bandit
            };

            foreach (var b in brains)
            {
                b.Actions = alchemist.brain.Actions;
            }
          

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                Profiles.ProfileManager.replaceAcl(old_acl, alchemist.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(alchemist.getFeatures(old_acl.Levels)));
            }
        }


        static void fixDruids()
        {
            var druid = Profiles.ProfileManager.getProfile("DruidCaster");
            { 
                //bsl duergars
                var brain = library.Get<BlueprintBrain>("7f6527bd36838ff42a1cb3964a05fd1b");
                brain.Actions = druid.brain.Actions;

                var features = library.Get<BlueprintFeature>("fd2460491b9d8e843b52c19f915ef47b");

                var old_acl = features.GetComponent<AddClassLevels>();
                features.RemoveComponent(old_acl);
                var new_acl = druid.getAcl(old_acl.Levels).CreateCopy(a => a.Archetypes = new BlueprintArchetype[] { library.Get<BlueprintArchetype>("35a3b7bfc663ac74aa8bb50adfe70813") }); //add blight druid
                features.AddComponent(new_acl);
                features.RemoveComponents<AddFacts>();
                features.AddComponent(Helpers.CreateAddFacts(druid.getFeatures(old_acl.Levels)));
            }


            //crazy_dryad
            {
                var dryad_brain = library.Get<BlueprintBrain>("b37d5b11de3de4b40bedfa04d338f2b5");
                dryad_brain.Actions = druid.brain.Actions;
                var dryad_features = library.Get<BlueprintFeature>("2573ea7a13133934381f51b34ff3c938");
                var old_acl = dryad_features.GetComponent<AddClassLevels>();
                dryad_features.RemoveComponent(old_acl);
                var new_acl = druid.getAcl(old_acl.Levels);
                dryad_features.AddComponent(new_acl);
                dryad_features.RemoveComponents<AddFacts>();
                dryad_features.AddComponent(Helpers.CreateAddFacts(druid.getFeatures(old_acl.Levels)));
            }


            //nugrah
            {
                var druid_melee = Profiles.ProfileManager.getProfile("DruidMelee");
                var brain = library.Get<BlueprintBrain>("84889317d828e884ca11d04b213f642a");
                brain.Actions = druid_melee.brain.Actions;
                var nugrah = library.Get<BlueprintUnit>("f9d8cbf3340d7a24e96bb498732bf531");
                var old_acl = nugrah.GetComponent<AddClassLevels>();
                Profiles.ProfileManager.replaceAcl(old_acl, druid_melee.getAcl(old_acl.Levels));
                nugrah.AddFacts = druid_melee.getFeatures(old_acl.Levels);
            }
        }


        static void updateAttackConsiderations()
        {
            var ac_consideration = Helpers.Create<NewConsiderations.AcConsideration>();
            ac_consideration.name = "ACConsideration";
            library.AddAsset(ac_consideration, "");

            var unit_far_consideration = Helpers.Create<Kingmaker.Controllers.Brain.Blueprints.Considerations.DistanceConsideration>();
            unit_far_consideration.name = "TargetIsFarConsideration";
            unit_far_consideration.BaseScoreModifier = 1.0f;
            unit_far_consideration.MinDistance = 2.0f; //~6 feet
            unit_far_consideration.MaxDistance = 6.0f; //~18 feet
            unit_far_consideration.MinDistanceScore = 0.0f;
            unit_far_consideration.MaxDistanceScore = 1.0f;
            library.AddAsset(unit_far_consideration, "2e7554ef535d4485960424c779b5de58");

            var attack_actions = library.GetAllBlueprints().OfType<BlueprintAiAction>().Where(f => f.name.Contains("AttackAiAction")).ToArray();
            var charge_action = library.Get<BlueprintAiCastSpell>("05003725a881c10419530387b6de5c9a");
            charge_action.BaseScore = 1.3f;
            charge_action.TargetConsiderations = charge_action.TargetConsiderations.AddToArray(unit_far_consideration);
            attack_actions.AddToArray(charge_action);
            foreach (var attack_action in attack_actions)
            {
                attack_action.TargetConsiderations = attack_action.TargetConsiderations.AddToArray(ac_consideration);
            }
            var brains = library.GetAllBlueprints().OfType<BlueprintBrain>().ToArray();
            attack_actions = attack_actions.RemoveFromArray(charge_action);
            foreach (var b in brains)
            {
                var first_attack = b.Actions.Where(a => attack_actions.Contains(a)).FirstOrDefault();

                if (first_attack != null && !attack_actions.Contains(charge_action))
                {
                    b.Actions = b.Actions.AddToArray(charge_action);
                }
            }



        }

        static void fixBanditTransmuters()
        {
            var transmuter = Profiles.ProfileManager.getProfile("WizardTransmuter");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditTransmuterFeatureListLevel")).ToArray();


            var brain = library.Get<BlueprintBrain>("bf90f2053c06375418c119115122ae3d");
            brain.Actions = transmuter.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                f.RemoveComponent(old_acl);
                f.AddComponent(transmuter.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(transmuter.getFeatures(old_acl.Levels)));
            }
        }


        static void fixMaestroJanush()
        {
            var transmuter = Profiles.ProfileManager.getProfile("WizardTransmuter");
            //increase level of janush to allow him to cast baleful polymorph and tar pool
            var unit = library.Get<BlueprintUnit>("14d442fea70487047bedb82d86b55451");

            var new_feature_list = library.CopyAndAdd<BlueprintFeature>("8bcac5e1bd4f254438fa8797337137a6", "MaestroJanushKingmakerAiFeatureList", "");
            new_feature_list.ReplaceComponent<AddClassLevels>(a =>
                                                                {
                                                                    transmuter.getAcl(12);
                                                                }
                                                                );
            new_feature_list.ReplaceComponent<AddFacts>(a => a.Facts = transmuter.getFeatures(12));

            unit.AddFacts[0] = new_feature_list;
            unit.GetComponent<Experience>().CR = 10;
        }

        static void fixBanditConjurers()
        {
            var conjurer = Profiles.ProfileManager.getProfile("WizardConjurer");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditConjurerFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("fde24a9130c94f74baa7f166ca1b8fcb");
            brain.Actions = conjurer.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                f.RemoveComponent(old_acl);
                f.AddComponent(conjurer.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(conjurer.getFeatures(old_acl.Levels)));
            }
        }


        static void fixBanditNecromancers()
        {
            var necromancer = Profiles.ProfileManager.getProfile("WizardNecromancer");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditNecromancerFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("775dc58da494c1240ab4508697135ebd");
            brain.Actions = necromancer.brain.Actions;
            
            var brain_techno_league = library.Get<BlueprintBrain>("1c4fed7a9a2861d49906abdf6fbfdf0c");
            brain_techno_league.Actions = necromancer.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                f.RemoveComponent(old_acl);
                f.AddComponent(necromancer.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(necromancer.getFeatures(old_acl.Levels)));
            }

            
            var assassins_leader = library.Get<BlueprintUnit>("58afb0334ae74f842a1699028d3b7401");
            var acl = assassins_leader.GetComponent<AddClassLevels>();
            assassins_leader.Brain = necromancer.brain;
            assassins_leader.RemoveComponent(acl);
            assassins_leader.AddComponent(necromancer.getAcl(acl.Levels));
            assassins_leader.AddFacts = necromancer.getFeatures(acl.Levels);
        }


        static void fixKoboldsDragonSorcerers()
        {
            var dragon_srocerer = Profiles.ProfileManager.getProfile("SorcererRedDragon");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("KoboldShamanFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("6b724c3dccf221842925c0abbcc6ad8e");
            brain.Actions = dragon_srocerer.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();

                Profiles.ProfileManager.replaceAcl(old_acl, dragon_srocerer.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(dragon_srocerer.getFeatures(old_acl.Levels)));
            }


            var tartuk_tartucio = library.Get<BlueprintUnit>("203d8959cd0b23b46b20014d8e537255");
            tartuk_tartucio.Brain.Actions = dragon_srocerer.brain.Actions;
            var tartuk_acl = tartuk_tartucio.GetComponent<AddClassLevels>();
            Profiles.ProfileManager.replaceAcl(tartuk_acl, dragon_srocerer.getAcl(tartuk_acl.Levels));
            tartuk_tartucio.AddFacts = tartuk_tartucio.AddFacts.AddToArray(dragon_srocerer.getFeatures(tartuk_acl.Levels));


            var tartuk_troll_fortress = library.Get<BlueprintUnit>("f2d11f187f76f6a4eb23f0ec1395f888");
            tartuk_troll_fortress.Brain.Actions = dragon_srocerer.brain.Actions;
            var tartuk_troll_fortress_acl = tartuk_troll_fortress.GetComponent<AddClassLevels>();
            Profiles.ProfileManager.replaceAcl(tartuk_troll_fortress_acl, dragon_srocerer.getAcl(tartuk_troll_fortress_acl.Levels));
            tartuk_troll_fortress.AddFacts = tartuk_troll_fortress.AddFacts.AddToArray(dragon_srocerer.getFeatures(tartuk_troll_fortress_acl.Levels));
            tartuk_troll_fortress.RemoveComponents<AddAbilityToCharacterComponent>(); //remove abilities since they will be cast as spells
        }


        static void fixKoboldsUndeadSorcerers()
        {
            var undead_srocerer = Profiles.ProfileManager.getProfile("SorcererUndead");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("KoboldShamanNecromancerFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("434fe616b01681b4f876083046337922");
            brain.Actions = undead_srocerer.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();

                Profiles.ProfileManager.replaceAcl(old_acl, undead_srocerer.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(undead_srocerer.getFeatures(old_acl.Levels)));
            }
        }


        static void fixDLC2CultistNecromancerBoss()
        {
            var necromancer = Profiles.ProfileManager.getProfile("WizardNecromancer");
            var unit = library.Get<BlueprintUnit>("c71f32cb01138b54a8e9747f84c435b8");



            unit.ReplaceComponent<AddClassLevels>(necromancer.getAcl(9));
            unit.AddFacts = necromancer.getFeatures(9);

            unit.Brain = necromancer.brain;
        }


        static void fixBanditIllusionists()
        {
            var illusionist = Profiles.ProfileManager.getProfile("WizardIllusionist");
            var features = library.GetAllBlueprints().OfType<BlueprintFeature>().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditIllusionistFeatureListLevel")).ToArray();

            var brain = library.Get<BlueprintBrain>("8ab8db551f38f0a48acda15dc00123ad");
            brain.Actions = illusionist.brain.Actions;

            foreach (var f in features)
            {
                var old_acl = f.GetComponent<AddClassLevels>();
                f.RemoveComponent(old_acl);
                f.AddComponent(illusionist.getAcl(old_acl.Levels));
                f.RemoveComponents<AddFacts>();
                f.AddComponent(Helpers.CreateAddFacts(illusionist.getFeatures(old_acl.Levels)));
            }
        }
    }
}
