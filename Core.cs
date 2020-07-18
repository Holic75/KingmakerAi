using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Controllers.Brain.Blueprints;
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
        }


        static void updateAttackConsiderations()
        {
            var ac_consideration = Helpers.Create<NewConsiderations.AcConsideration>();
            ac_consideration.name = "ACConsideration";
            library.AddAsset(ac_consideration, "");

            var attack_actions = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("AttackAiAction")).Cast<BlueprintAiAction>().ToArray();

            foreach (var attack_action in attack_actions)
            {
                attack_action.TargetConsiderations = attack_action.TargetConsiderations.AddToArray(ac_consideration);
            }
        }

        static void fixBanditTransmuters()
        {
            var transmuter = Profiles.ProfileManager.getProfile("WizardTransmuter");
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditTransmuterFeatureListLevel")).Cast<BlueprintFeature>().ToArray();


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
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditConjurerFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

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
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditNecromancerFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

            var brain = library.Get<BlueprintBrain>("fde24a9130c94f74baa7f166ca1b8fcb");
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
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("BanditIllusionistFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

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
