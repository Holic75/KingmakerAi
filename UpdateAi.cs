using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
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

        static internal void load()
        {
            updateAttackConsiderations();
            updateTsanna();

            updateGoblinFighter();
            updateGoblinRogue();
            updateGoblinArcher();
            updateGoblinAlchemist();
            //  
            updateGoblinShaman();
            fixSaves();
        }


        static void updateGoblinShaman()
        {
            var goblin_shaman = library.Get<BlueprintUnit>("8421b6137d7765947958973526b5249b");
            goblin_shaman.AddFacts = new Kingmaker.Blueprints.Facts.BlueprintUnitFact[] { library.Get<BlueprintFeature>("9d168ca7100e9314385ce66852385451"),
                                                                                          library.Get<BlueprintBuff>("33d5368d93d949ecbd82b3528750b5a0"), //saura of doom
                                                                                          library.Get<BlueprintBuff>("5274ddc289f4a7447b7ace68ad8bebb0"), //shield of faith
                                                                                          library.Get<BlueprintFeature>("3adb2c906e031ee41a01bfc1d5fb7eea"), //channel negative
                                                                                        };
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
            var cold_ice_strike = library.Get<BlueprintAbility>("5ef85d426783a5347b420546f91a677b");

            class_levels.MemorizeSpells = new BlueprintAbility[]
            {
                //level 4
                NewSpells.aura_of_doom,
                library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0"), //fear
                library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113"), //divine favor
                library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113"), //divine favor

                //level 5
                CallOfTheWild.NewSpells.command_greater,
                flame_strike,
                flame_strike,
                flame_strike,
                flame_strike,

                //level 6
                library.Get<BlueprintAbility>("e740afbab0147944dab35d83faa0ae1c"), //sm 6
                cold_ice_strike,
                cold_ice_strike,
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
                                                                       library.Get<BlueprintAiAction>("09de02db1b07d364795f412abb557de3"), //divine favor
                                                                      library.Get<BlueprintAiAction>("866ffa6c34000cd4a86fb1671f86c7d8")); //attack
            goblin_shaman.Brain.Actions = new_actions;

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

        static void updateGoblinFighter()
        {
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinFighterFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

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
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinArcherFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

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
            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinRogueFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

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

            var units = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("_GoblinRogue")).Cast<BlueprintUnit>().ToArray();
            foreach (var u in units)
            {
                var levels = u.GetComponent<AddClassLevels>();
                levels.Selections = features[0].GetComponent<AddClassLevels>().Selections;
            }
        }


        static void updateGoblinAlchemist()
        {
            var spells = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinAlchemistSpellListLevel")).Cast<BlueprintFeature>().ToArray();

            var haste = library.Get<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98");
            foreach (var s in spells)
            {
                var learn = s.GetComponent<LearnSpells>();
                learn.Spells = learn.Spells.AddToArray(haste);
            }

            var features = library.GetAllBlueprints().Where<BlueprintScriptableObject>(f => f.name.Contains("GoblinAlchemistFeatureListLevel")).Cast<BlueprintFeature>().ToArray();

            var chocking_bombs = library.Get<BlueprintFeature>("b3c6cb76d5b11cf4c8314d7b1c7b9b8b");

            foreach (var feature in features)
            {
                var levels = feature.GetComponent<AddClassLevels>();
                levels.MemorizeSpells[7] = haste; //replace displacement with haste
                var selections = levels.Selections;
                selections[1].Features = selections[1].Features.AddToArray(chocking_bombs); //add chocking bombs
            }
            var brain = library.Get<BlueprintBrain>("53a68e3631a6e1646997de0cb50ba49f");

            var use_chocking_bomb = library.Get<BlueprintAiCastSpell>("9efb8b70ddb084445a2be809e8271259");
            var use_haste = library.Get<BlueprintAiCastSpell>("c2030f9e42b7e3d4fb08f6f05c68eae1");

            brain.Actions = brain.Actions.AddToArray(use_chocking_bomb, use_haste);
        }




        static void fixSaves()
        {
            Action<UnitDescriptor> ai_fix = delegate (UnitDescriptor u)
            {
                if (u.IsPlayerFaction)
                {
                    return;
                }
                var tr = Harmony12.Traverse.Create(u);
                tr.Property("Brain").SetValue(new UnitBrain(u));
                //u.Initialize();

                //add spells if they have changed
                foreach (var acl in u.Blueprint.GetComponents<AddClassLevels>())
                {
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

                    acl.LevelUp(u, 0);
                }
                u.Brain.RestoreAvailableActions();
            };
            SaveGameFix.save_game_actions.Add(ai_fix);
        }

        static BlueprintAiCastSpell createCastSpellAction(string name, BlueprintAbility spell, Consideration[] actor_consideration, Consideration[] target_consideration,
                                                               float base_score = 1f, BlueprintAbility variant = null, int combat_count = 0)
        {

            var action = CallOfTheWild.Helpers.Create<BlueprintAiCastSpell>();
            action.Ability = spell;
            action.Variant = variant;
            action.ActorConsiderations = actor_consideration;
            action.TargetConsiderations = target_consideration;
            action.name = name;
            action.BaseScore = base_score;
            action.CombatCount = combat_count;
            library.AddAsset(action, "");

            return action;
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
            var cold_ice_strike = library.Get<BlueprintAbility>("5ef85d426783a5347b420546f91a677b");
            foreach (var u in units)
            {
                var class_levels = u.GetComponent<AddClassLevels>();
                class_levels.Skills = new StatType[] { StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception };
                var spells = class_levels.MemorizeSpells.ToList();
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Remove(slay_living);
                spells.Add(CallOfTheWild.NewSpells.command_greater);
                spells.Add(flame_strike);
                spells.Add(flame_strike);
                spells.Add(flame_strike);
                spells.Add(cold_ice_strike);
                spells.Add(cold_ice_strike);
                spells.Add(cold_ice_strike);
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
            var cast_command = createCastSpellAction("CastCommandGreaterHaltSpellAiAction", NewSpells.command_greater, new Consideration[0], new Consideration[] { aoe_more_enemies_considertion },
                                                      base_score: 10, variant: NewSpells.command_greater.Variants[0]);

            var quick_channel = ChannelEnergyEngine.getQuickChannelVariant(library.Get<BlueprintAbility>("89df18039ef22174b81052e2e419c728"));

            var quick_channel_action = createCastSpellAction("QuickChannelNegativeClericAiAction", quick_channel.Parent, new Consideration[0], new Consideration[] { aoe_more_enemies_considertion },
                                          base_score: 20.0f, variant: quick_channel);
            quick_channel_action.CooldownRounds = 1;
            var cast_cold_ice_strike = createCastSpellAction("CastColdIceStrikeSpellAiAction", cold_ice_strike, new Consideration[0], new Consideration[] { aoe_considertion },
                                          base_score: 50.0f, variant: null);
            cast_cold_ice_strike.CooldownRounds = 1;


            brain.Actions = brain.Actions.AddToArray(cast_flame_strike, cast_command, quick_channel_action, cast_cold_ice_strike);
        }
    }

}
