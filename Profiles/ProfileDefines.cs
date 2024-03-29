﻿using CallOfTheWild;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Brain.Blueprints;
using Kingmaker.Controllers.Brain.Blueprints.Considerations;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetType = Kingmaker.Controllers.Brain.Blueprints.TargetType;

namespace KingmakerAI.Profiles
{
    public partial class ProfileManager
    {
        [Flags]
        enum AbilityUseType
        {
            OnAlly =  1,
            OnEnemy = 2,
            OnSelf = 4,
            Aoe = 8,
            Precast = 16
        }

        static float aoe_multiplier = 1.25f;
        static float precast_boost = 100.0f;


        public static class Classes
        {
            static internal BlueprintCharacterClass alchemist = library.Get<BlueprintCharacterClass>("0937bec61c0dabc468428f496580c721");
            static internal BlueprintCharacterClass barbarian = library.Get<BlueprintCharacterClass>("f7d7eb166b3dd594fb330d085df41853");
            static internal BlueprintCharacterClass bard = library.Get<BlueprintCharacterClass>("772c83a25e2268e448e841dcd548235f");
            static internal BlueprintCharacterClass paladin = library.Get<BlueprintCharacterClass>("bfa11238e7ae3544bbeb4d0b92e897ec");
            static internal BlueprintCharacterClass cleric = library.Get<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
            static internal BlueprintCharacterClass druid = library.Get<BlueprintCharacterClass>("610d836f3a3a9ed42a4349b62f002e96");
            static internal BlueprintCharacterClass fighter = library.Get<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            static internal BlueprintCharacterClass inquistor = library.Get<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");
            static internal BlueprintCharacterClass kineticist = library.Get<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391");
            static internal BlueprintCharacterClass magus = library.Get<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            static internal BlueprintCharacterClass rogue = library.Get<BlueprintCharacterClass>("299aa766dee3cbf4790da4efb8c72484");
            static internal BlueprintCharacterClass sorceror = library.Get<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            static internal BlueprintCharacterClass wizard = library.Get<BlueprintCharacterClass>("ba34257984f4c41408ce1dc2004e342e");
            static internal BlueprintCharacterClass ranger = library.Get<BlueprintCharacterClass>("cda0615668a6df14eb36ba19ee881af6");
            static internal BlueprintCharacterClass slayer = library.Get<BlueprintCharacterClass>("c75e0971973957d4dbad24bc7957e4fb");
            static internal BlueprintCharacterClass monk = library.Get<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124");
            static internal BlueprintCharacterClass animal = library.Get<BlueprintCharacterClass>("4cd1757a0eea7694ba5c933729a53920");
        }


        public static class Archetypes
        {
            static internal BlueprintArchetype eldritch_archer = library.Get<BlueprintArchetype>("44388c01eb4a29d4d90a25cc0574320d");
        }


        public static class AiActions
        {
            static public BlueprintAiCastSpell acid_splash_ai_action = library.Get<BlueprintAiCastSpell>("8cf4732cf870f8f4cbf760331c8f2696");
            static public BlueprintAiAction attack_action = library.Get<BlueprintAiAction>("866ffa6c34000cd4a86fb1671f86c7d8");
        }


        public static class Considerations
        {
             public static UnitsAroundConsideration aoe_at_least_one_enemy_considertion => library.Get<UnitsAroundConsideration>("96a5a05d98be03446bbf21e217270b06");
             public static UnitsAroundConsideration aoe_more_enemies_considertion => library.Get<UnitsAroundConsideration>("b2490b137b8b53a4e950c1d79d1c5c1d");
             public static ComplexConsideration attack_target_prioriteis = library.Get<ComplexConsideration>("7a2b25dcc09cd244db261ce0a70cca84");
             public static CommandCooldownConsideration swift_action_available = library.Get<CommandCooldownConsideration>("c2b7d2f9a5cb8d04d9e1aa4bf3d3c598");
             public static CommandCooldownConsideration no_standard_action = library.Get<CommandCooldownConsideration>("eb52264e87de14842b44b362da4e0673");
             public static UnitsAroundConsideration avoid_friends = library.Get<UnitsAroundConsideration>("8e6f34026b34c3d4ba831bb94548904a");
             public static UnitsAroundConsideration choose_more_friends;
             public static Consideration injury_around_consideration = library.Get<HealthAroundConsideration>("2a2cfff1d585f3142aadaafe0c1a74e6");

             public static Consideration[] harmful_enemy_ally_aoe_target_consideration = new Consideration[] {avoid_friends, aoe_at_least_one_enemy_considertion, aoe_more_enemies_considertion, attack_target_prioriteis };
             public static Consideration[] harmful_enemy_ally_aoe_target_consideration_small = new Consideration[] { avoid_friends, aoe_at_least_one_enemy_considertion, attack_target_prioriteis };
             public static Consideration[] harmful_enemy_aoe_target_consideration = new Consideration[] { aoe_at_least_one_enemy_considertion, aoe_more_enemies_considertion, attack_target_prioriteis};
             public static Consideration[] harmful_enemy_aoe_target_consideration_small = new Consideration[] { aoe_at_least_one_enemy_considertion, attack_target_prioriteis };

             public static ArmorTypeConsideration light_armor_consideration;
             public static ArmorTypeConsideration heavy_armor_consideration;
             public static Consideration light_armor_around_enemies_consideration;
             public static Consideration heavy_armor_around_enemies_consideration;

             public static Consideration target_self_consideration = library.Get<TargetSelfConsideration>("83e2dd97b82d769498394c3edf0d260e");
             public static Consideration target_other_consideration = library.Get<TargetSelfConsideration>("f4be6fc6f46b61044a44715f99f1918d");

             public static Consideration unconcious_penalty = library.Get<LifeStateConsideration>("bf033da33480da84f8265f5f8bcdd467");
             public static Consideration stand_still = library.Get<InRangeConsideration>("da2c75bc93165b749b18654b21759ea3");
             public static Consideration in_range = library.Get<InRangeConsideration>("f083257df5834aa459be020f9b291f21");

             public static NewConsiderations.TargetFactionConsideration is_enemy;
             public static NewConsiderations.TargetFactionConsideration is_ally;

             public static NewConsiderations.BabPartConsideration higher_bab;
             public static NewConsiderations.UnitPolymorphed not_polymorphed;
        }


        public static class AlchemistAbilities
        {
            public static BlueprintAbility fire_bomb = library.Get<BlueprintAbility>("5fa0111ac60ed194db82d3110a9d0352");
            public static BlueprintAbility force_bomb = library.Get<BlueprintAbility>("557898e059f5ff644848b0a4df087391");
            public static BlueprintAbility chocking_bomb = library.Get<BlueprintAbility>("0fcbed386deb18b428825605796d5a15");
            public static BlueprintAbility blinding_bomb = library.Get<BlueprintAbility>("bd05918a568c41e49aed7b9526ba596b");
            public static BlueprintAbility dex_mutagen = library.Get<BlueprintAbility>("b11d314d60f7a38498d1ed6933fe1638");
            public static BlueprintAbility int_cognatogen = library.Get<BlueprintAbility>("14230cf35ac2b5b45a93b13cfe478585");
            public static BlueprintAbility int_greater_cognatogen = library.Get<BlueprintAbility>("1c46157abba66934aace22a9a909dd13");
            public static BlueprintAbility int_grand_cognatogen = library.Get<BlueprintAbility>("210218888fd34884583576d5035f46ea");
        }

        public static class Spells
        {
            public static BlueprintAbility acid_splash = library.Get<BlueprintAbility>("0c852a2405dd9f14a8bbcfaf245ff823");

            public static BlueprintAbility flare_burst = library.Get<BlueprintAbility>("39a602aa80cc96f4597778b6d4d49c0a");
            public static BlueprintAbility feather_step = library.Get<BlueprintAbility>("f3c0b267dd17a2a45a40805e31fe3cd1");
            public static BlueprintAbility longstrider = library.Get<BlueprintAbility>("14c90900b690cac429b229efdf416127");
            public static BlueprintAbility faerie_fire = library.Get<BlueprintAbility>("4d9bf81b7939b304185d58a09960f589");
            public static BlueprintAbility entnagle = library.Get<BlueprintAbility>("0fd00984a2c0e0a429cf1a911b4ec5ca");
            public static BlueprintAbility magic_fang = library.Get<BlueprintAbility>("403cf599412299a4f9d5d925c7b9fb33");

            public static BlueprintAbility ear_piercing_scream = library.Get<BlueprintAbility>("8e7cfa5f213a90549aadd18f8f6f4664");
            public static BlueprintAbility hideous_laughter = library.Get<BlueprintAbility>("fd4d9fd7f87575d47aafe2a64a6e2d8d");
            public static BlueprintAbility bless = library.Get<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638");
            public static BlueprintAbility divine_favor = library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113");
            public static BlueprintAbility remove_fear = library.Get<BlueprintAbility>("55a037e514c0ee14a8e3ed14b47061de");
            public static BlueprintAbility haze_of_dreams = library.Get<BlueprintAbility>("40ec382849b60504d88946df46a10f2d");
            public static BlueprintAbility shield_of_faith = library.Get<BlueprintAbility>("183d5bb91dea3a1489a6db6c9cb64445");
            public static BlueprintAbility stunning_barrier = library.Get<BlueprintAbility>("a5ec7892fb1c2f74598b3a82f3fd679f");
            public static BlueprintAbility snow_ball = library.Get<BlueprintAbility>("9f10909f0be1f5141bf1c102041f93d9");
            public static BlueprintAbility shield = library.Get<BlueprintAbility>("ef768022b0785eb43a18969903c537c4");


            public static BlueprintAbility aid = library.Get<BlueprintAbility>("03a9630394d10164a9410882d31572f0");
            public static BlueprintAbility hold_person = library.Get<BlueprintAbility>("c7104f7526c4c524f91474614054547e");
            public static BlueprintAbility hold_monster = library.Get<BlueprintAbility>("41e8a952da7a5c247b3ec1c2dbb73018");
            public static BlueprintAbility sound_burst = library.Get<BlueprintAbility>("c3893092a333b93499fd0a21845aa265");

            public static BlueprintAbility good_hope = library.Get<BlueprintAbility>("a5e23522eda32dc45801e32c05dc9f96");
            public static BlueprintAbility archons_aura = library.Get<BlueprintAbility>("e67efd8c84f69d24ab472c9f546fff7e");
            public static BlueprintAbility bestow_curse = library.Get<BlueprintAbility>("989ab5c44240907489aba0a8568d0603");
            public static BlueprintAbility prayer = library.Get<BlueprintAbility>("faabd2cc67efa4646ac58c7bb3e40fcc");
            public static BlueprintAbility resist_energy_communal = library.Get<BlueprintAbility>("7bb0c402f7f789d4d9fae8ca87b4c7e2");

            public static BlueprintAbility divine_power = library.Get<BlueprintAbility>("ef16771cb05d1344989519e87f25b3c5");
            public static BlueprintAbility freedom_of_movement = library.Get<BlueprintAbility>("0087fc2d64b6095478bc7b8d7d512caf");
            public static BlueprintAbility shield_of_dawn = library.Get<BlueprintAbility>("62888999171921e4dafb46de83f4d67d");
            public static BlueprintAbility dismissal = library.Get<BlueprintAbility>("95f7cdcec94e293489a85afdf5af1fd7");
            public static BlueprintAbility protection_from_energy_communal = library.Get<BlueprintAbility>("76a629d019275b94184a1a8733cac45e");

            public static BlueprintAbility angleic_aspect = library.Get<BlueprintAbility>("75a10d5a635986641bfbcceceec87217");
            public static BlueprintAbility angleic_aspect_greater = library.Get<BlueprintAbility>("b1c7576bd06812b42bda3f09ab202f14");
            public static BlueprintAbility serenity = library.Get<BlueprintAbility>("d316d3d94d20c674db2c24d7de96f6a7");
            public static BlueprintAbility burst_of_glory = library.Get<BlueprintAbility>("1bc83efec9f8c4b42a46162d72cbf494");
            public static BlueprintAbility constricting_coils = library.Get<BlueprintAbility>("3fce8e988a51a2a4ea366324d6153001");
            public static BlueprintAbility true_seeing = library.Get<BlueprintAbility>("4cf3d0fae3239ec478f51e86f49161cb");
            public static BlueprintAbility see_invisibility_communal = library.Get<BlueprintAbility>("1a045f845778dc54db1c2be33a8c3c0a");

            public static BlueprintAbility shout = library.Get<BlueprintAbility>("f09453607e683784c8fca646eec49162");
            public static BlueprintAbility dominate_person = library.Get<BlueprintAbility>("d7cbd2004ce66a042aeab2e95a3c5c61");
            public static BlueprintAbility chains_of_light = library.Get<BlueprintAbility>("f8cea58227f59c64399044a82c9735c4");
            public static BlueprintAbility eaglesoul = library.Get<BlueprintAbility>("332ad68273db9704ab0e92518f2efd1c");
            public static BlueprintAbility blade_barrier = library.Get<BlueprintAbility>("36c8971e91f1745418cc3ffdfac17b74");
            public static BlueprintAbility harm = library.Get<BlueprintAbility>("cc09224ecc9af79449816c45bc5be218");
            public static BlueprintAbility cacaphonous_call_mass = library.Get<BlueprintAbility>("1262284b6fa45b9458b8c3693edbd676");
            public static BlueprintAbility righteous_might = library.Get<BlueprintAbility>("90810e5cf53bf854293cbd5ea1066252");
            public static BlueprintAbility magic_fang_greater = library.Get<BlueprintAbility>("f1100650705a69c4384d3edd88ba0f52");
            public static BlueprintAbility thorn_body = library.Get<BlueprintAbility>("2daf9c5112f16d54ab3cd6904c705c59");
            public static BlueprintAbility cape_of_wasps = library.Get<BlueprintAbility>("e418c20c8ce362943a8025d82c865c1c");
            public static BlueprintAbility blessing_of_the_salamander = library.Get<BlueprintAbility>("9256a86aec14ad14e9497f6b60e26f3f");

            public static BlueprintAbility destruction = library.Get<BlueprintAbility>("3b646e1db3403b940bf620e01d2ce0c7");
            public static BlueprintAbility waves_of_ecstasy = library.Get<BlueprintAbility>("1e2d1489781b10a45a3b70192bba9be3");
            public static BlueprintAbility shout_greater = library.Get<BlueprintAbility>("fd0d3840c48cafb44bb29e8eb74df204");
            public static BlueprintAbility blasphemy = library.Get<BlueprintAbility>("bd10c534a09f44f4ea632c8b8ae97145");

            public static BlueprintAbility fire_storm = library.Get<BlueprintAbility>("e3d0dfe1c8527934294f241e0ae96a8d");
            public static BlueprintAbility euphoric_tranquility = library.Get<BlueprintAbility>("740d943e42b60f64a8de74926ba6ddf7");
            public static BlueprintAbility death_clutch = library.Get<BlueprintAbility>("c3d2294a6740bc147870fff652f3ced5");
            public static BlueprintAbility frightful_aspect = library.Get<BlueprintAbility>("e788b02f8d21014488067bdd3ba7b325");
            public static BlueprintAbility rift_of_ruin = library.Get<BlueprintAbility>("dd3dacafcf40a0145a5824c838e2698d");
            public static BlueprintAbility strom_bolts = library.Get<BlueprintAbility>("7cfbefe0931257344b2cb7ddc4cdff6f");

            public static BlueprintAbility heal_mass = library.Get<BlueprintAbility>("867524328b54f25488d371214eea0d90");
            public static BlueprintAbility polar_midnight = library.Get<BlueprintAbility>("ba48abb52b142164eba309fd09898856");

            public static BlueprintAbility color_spray = library.Get<BlueprintAbility>("91da41b9793a4624797921f221db653c");
            public static BlueprintAbility cause_fear = library.Get<BlueprintAbility>("bd81a3931aa285a4f9844585b5d97e51");
            public static BlueprintAbility ray_of_enfeeblement = library.Get<BlueprintAbility>("450af0402422b0b4980d9c2175869612");
            public static BlueprintAbility magic_missile = library.Get<BlueprintAbility>("4ac47ddb9fa1eaf43a1b6809980cfbd2");
            public static BlueprintAbility enlarge_person = library.Get<BlueprintAbility>("c60969e7f264e6d4b84a1499fdcf9039");
            public static BlueprintAbility reduce_person = library.Get<BlueprintAbility>("4e0e9aba6447d514f88eff1464cc4763");          
            public static BlueprintAbility mage_armor = library.Get<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568");
            public static BlueprintAbility mage_shield = library.Get<BlueprintAbility>("ef768022b0785eb43a18969903c537c4");
            public static BlueprintAbility sleep = library.Get<BlueprintAbility>("bb7ecad2d3d2c8247a38f44855c99061");
            public static BlueprintAbility grease = library.Get<BlueprintAbility>("95851f6e85fe87d4190675db0419d112");
            public static BlueprintAbility burning_hands = library.Get<BlueprintAbility>("4783c3709a74a794dbe7c8e7e0b1b038");

            public static BlueprintAbility barkskin = library.Get<BlueprintAbility>("5b77d7cc65b8ab74688e74a37fc2f553");
            public static BlueprintAbility delay_poison = library.Get<BlueprintAbility>("b48b4c5ffb4eab0469feba27fc86a023");
            public static BlueprintAbility sickening_entnaglement = library.Get<BlueprintAbility>("6c7467f0344004d48848a43d8c078bf8");

            public static BlueprintAbility bulls_strength = library.Get<BlueprintAbility>("4c3d08935262b6544ae97599b3a9556d");
            public static BlueprintAbility bears_endurance = library.Get<BlueprintAbility>("a900628aea19aa74aad0ece0e65d091a");
            public static BlueprintAbility foxs_cunning = library.Get<BlueprintAbility>("ae4d3ad6a8fda1542acf2e9bbc13d113");
            public static BlueprintAbility owls_wisdom = library.Get<BlueprintAbility>("f0455c9295b53904f9e02fc571dd2ce1");
            public static BlueprintAbility cats_grace = library.Get<BlueprintAbility>("de7a025d48ad5da4991e7d3c682cf69d");
            public static BlueprintAbility eagles_splendor = library.Get<BlueprintAbility>("446f7bf201dc1934f96ac0a26e324803");
            public static BlueprintAbility mirror_image = library.Get<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564");
            public static BlueprintAbility create_pit = library.Get<BlueprintAbility>("29ccc62632178d344ad0be0865fd3113");
            public static BlueprintAbility glitter_dust = library.Get<BlueprintAbility>("ce7dad2b25acf85429b6c9550787b2d9");
            public static BlueprintAbility acid_arrow = library.Get<BlueprintAbility>("9a46dfd390f943647ab4395fc997936d");
            public static BlueprintAbility scare = library.Get<BlueprintAbility>("08cb5f4c3b2695e44971bf5c45205df0");
            public static BlueprintAbility blindness = library.Get<BlueprintAbility>("46fd02ad56c35224c9c91c88cd457791");
            public static BlueprintAbility blur = library.Get<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1");
            public static BlueprintAbility burning_arc = library.Get<BlueprintAbility>("eaac3d36e0336cb479209a6f65e25e7c");
            public static BlueprintAbility scorching_ray = library.Get<BlueprintAbility>("cdb106d53c65bbc4086183d54c3b97c7");
            public static BlueprintAbility boneshaker = library.Get<BlueprintAbility>("b7731c2b4fa1c9844a092329177be4c3");

            public static BlueprintAbility delay_poison_communal = library.Get<BlueprintAbility>("04e820e1ce3a66f47a50ad5074d3ae40");
            public static BlueprintAbility feather_step_mass = library.Get<BlueprintAbility>("d219494150ac1f24f9ce14a3d4f66d26");

            public static BlueprintAbility displacement = library.Get<BlueprintAbility>("903092f6488f9ce45a80943923576ab3");

            public static BlueprintAbility magical_vestement = library.Get<BlueprintAbility>("2d4263d80f5136b4296d6eb43a221d7d");
            public static BlueprintAbility magical_vestement_armor = library.Get<BlueprintAbility>("956309af83352714aa7ee89fb4ecf201");
            public static BlueprintAbility haste = library.Get<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98");
            public static BlueprintAbility greater_invisibility = library.Get<BlueprintAbility>("ecaa0def35b38f949bd1976a6c9539e0");
            public static BlueprintAbility slow = library.Get<BlueprintAbility>("f492622e473d34747806bdb39356eb89");
            public static BlueprintAbility deep_slumber = library.Get<BlueprintAbility>("7658b74f626c56a49939d9c20580885e");
            public static BlueprintAbility spiked_pit = library.Get<BlueprintAbility>("46097f610219ac445b4d6403fc596b9f");
            public static BlueprintAbility stinking_cloud = library.Get<BlueprintAbility>("68a9e6d7256f1354289a39003a46d826");
            public static BlueprintAbility acid_pit = library.Get<BlueprintAbility>("1407fb5054d087d47a4c40134c809f12");
            public static BlueprintAbility controlled_fireball = library.Get<BlueprintAbility>("f72f8f03bf0136c4180cd1d70eb773a5");
            public static BlueprintAbility enervation = library.Get<BlueprintAbility>("f34fb78eaaec141469079af124bcfa0f");
            public static BlueprintAbility bone_shatter = library.Get<BlueprintAbility>("f2f1efac32ea2884e84ecaf14657298b");
            public static BlueprintAbility banshee_blast = library.Get<BlueprintAbility>("d42c6d3f29e07b6409d670792d72bc82");
            public static BlueprintAbility phantasmal_killer = library.Get<BlueprintAbility>("6717dbaef00c0eb4897a1c908a75dfe5");
            public static BlueprintAbility rainbow_pattern = library.Get<BlueprintAbility>("4b8265132f9c8174f87ce7fa6d0fe47b");
            public static BlueprintAbility heroism = library.Get<BlueprintAbility>("5ab0d42fb68c9e34abae4921822b9d63");
            public static BlueprintAbility dispel_magic = library.Get<BlueprintAbility>("92681f181b507b34ea87018e8f7a528a");
            public static BlueprintAbility stone_skin = library.Get<BlueprintAbility>("c66e86905f7606c4eaa5c774f0357b2b");
            public static BlueprintAbility protection_from_arrows_communal = library.Get<BlueprintAbility>("96c9d98b6a9a7c249b6c4572e4977157");

            public static BlueprintAbility enlarge_person_mass = library.Get<BlueprintAbility>("66dc49bf154863148bd217287079245e");
            public static BlueprintAbility dragon_breath = library.Get<BlueprintAbility>("5e826bcdfde7f82468776b55315b2403");
            public static BlueprintAbility flame_strike = library.Get<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");
            public static BlueprintAbility animal_growth = library.Get<BlueprintAbility>("56923211d2ac95e43b8ac5031bab74d8");
            public static BlueprintAbility vine_trap = library.Get<BlueprintAbility>("6d1d48a939ce475409f06e1b376bc386");

            public static BlueprintAbility elemental_body1 = library.Get<BlueprintAbility>("690c90a82bf2e58449c6b541cb8ea004");
            public static BlueprintAbility elemental_body2 = library.Get<BlueprintAbility>("6d437be73b459594ab103acdcae5b9e2");
            public static BlueprintAbility elemental_body3 = library.Get<BlueprintAbility>("459e6d5aab080a14499e13b407eb3b85");
            public static BlueprintAbility elemental_body4 = library.Get<BlueprintAbility>("376db0590f3ca4945a8b6dc16ed14975");

            public static BlueprintAbility form_of_the_dragon1 = library.Get<BlueprintAbility>("f767399367df54645ac620ef7b2062bb");
            public static BlueprintAbility form_of_the_dragon2 = library.Get<BlueprintAbility>("666556ded3a32f34885e8c318c3a0ced");
            public static BlueprintAbility form_of_the_dragon3 = library.Get<BlueprintAbility>("1cdc4ad4c208246419b98a35539eafa6");

            public static BlueprintAbility summon_monster4 = library.Get<BlueprintAbility>("7ed74a3ec8c458d4fb50b192fd7be6ef");
            public static BlueprintAbility summon_monster4_d3 = library.Get<BlueprintAbility>("e73c4562e99c7764a9207710facc61d2");
            public static BlueprintAbility summon_monster5 = library.Get<BlueprintAbility>("630c8b85d9f07a64f917d79cb5905741");
            public static BlueprintAbility summon_monster5_d3 = library.Get<BlueprintAbility>("715f208d545be2f4aa2d3693e6347a5a");
            public static BlueprintAbility summon_monster7 = library.Get<BlueprintAbility>("ab167fd8203c1314bac6568932f1752f");
            public static BlueprintAbility summon_monster7_d3 = library.Get<BlueprintAbility>("43f763d347eb2744caed9c656ba89531");
            public static BlueprintAbility summon_monster8 = library.Get<BlueprintAbility>("d3ac756a229830243a72e84f3ab050d0");
            public static BlueprintAbility summon_monster8_d3 = library.Get<BlueprintAbility>("ddc1d195a4434374e860b1568cfc7d11");
            public static BlueprintAbility summon_monster9 = library.Get<BlueprintAbility>("52b5df2a97df18242aec67610616ded0");
            public static BlueprintAbility summon_monster9_d3 = library.Get<BlueprintAbility>("4988b2e622c6f2d4b897894e3be13f09");
            public static BlueprintAbility create_undead = library.Get<BlueprintAbility>("76a11b460be25e44ca85904d6806e5a3");
            public static BlueprintAbility summon_elder_worm = library.Get<BlueprintAbility>("954f1469ed62843409783c9fa7472998");
            public static BlueprintAbility summon_nature_ally_7 = library.Get<BlueprintAbility>("051b979e7d7f8ec41b9fa35d04746b33");
            public static BlueprintAbility summon_nature_ally_8 = library.Get<BlueprintAbility>("ea78c04f0bd13d049a1cce5daf8d83e0");

            public static BlueprintAbility acidic_spray = library.Get<BlueprintAbility>("c543eef6d725b184ea8669dd09b3894c");
            public static BlueprintAbility stoneskin_communal = library.Get<BlueprintAbility>("7c5d556b9a5883048bf030e20daebe31");
            public static BlueprintAbility echolocation = library.Get<BlueprintAbility>("20b548bf09bb3ea4bafea78dcb4f3db6");
            public static BlueprintAbility cloudkill = library.Get<BlueprintAbility>("548d339ba87ee56459c98e80167bdf10");
            public static BlueprintAbility hungry_pit = library.Get<BlueprintAbility>("f63f4d1806b78604a952b3958892ce1c");
            public static BlueprintAbility waves_of_fatigue = library.Get<BlueprintAbility>("8878d0c46dfbd564e9d5756349d5e439");

            public static BlueprintAbility phantasmal_putrefaction = library.Get<BlueprintAbility>("1f2e6019ece86d64baa5effa15e81ecc");
            public static BlueprintAbility cold_ice_strike = library.Get<BlueprintAbility>("5ef85d426783a5347b420546f91a677b");
            public static BlueprintAbility mind_fog = library.Get<BlueprintAbility>("eabf94e4edc6e714cabd96aa69f8b207");

            public static BlueprintAbility circle_of_death = library.Get<BlueprintAbility>("a89dcbbab8f40e44e920cc60636097cf");
            public static BlueprintAbility phantasmal_web = library.Get<BlueprintAbility>("12fb4a4c22549c74d949e2916a2f0b6a");
            public static BlueprintAbility fireball = library.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            public static BlueprintAbility acid_fog = library.Get<BlueprintAbility>("dbf99b00cd35d0a4491c6cc9e771b487");
            public static BlueprintAbility sirocco = library.Get<BlueprintAbility>("093ed1d67a539ad4c939d9d05cfe192c");
            public static BlueprintAbility plague_storm = library.Get<BlueprintAbility>("82a5b848c05e3f342b893dedb1f9b446");
            public static BlueprintAbility waves_of_exhaustion = library.Get<BlueprintAbility>("3e4d3b9a5bd03734d9b053b9067c2f38");
            public static BlueprintAbility finger_of_death = library.Get<BlueprintAbility>("6f1dcf6cfa92d1948a740195707c0dbe");
            public static BlueprintAbility disintegrate = library.Get<BlueprintAbility>("4aa7942c3e62a164387a73184bca3fc1");

            public static BlueprintAbility thudnering_drums = library.Get<BlueprintAbility>("c26eeeeabf732914ba723f2b67fe9b9d");
            public static BlueprintAbility cloak_of_dreams = library.Get<BlueprintAbility>("7f71a70d822af94458dc1a235507e972");
            public static BlueprintAbility confusion = library.Get<BlueprintAbility>("cf6c901fb7acc904e85c63b342e9c949");
            public static BlueprintAbility fear = library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");
            public static BlueprintAbility false_life = library.Get<BlueprintAbility>("7a5b5bf845779a941a67251539545762");
            public static BlueprintAbility false_life_greater = library.Get<BlueprintAbility>("dc6af3b4fd149f841912d8a3ce0983de");
            public static BlueprintAbility spell_resistance = library.Get<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889");

            public static BlueprintAbility chain_lightning = library.Get<BlueprintAbility>("645558d63604747428d55f0dd3a4cb58");
            public static BlueprintAbility resonating_word = library.Get<BlueprintAbility>("df7d13c967bce6a40bec3ba7c9f0e64c");
            public static BlueprintAbility bears_endurance_mass = library.Get<BlueprintAbility>("f6bcea6db14f0814d99b54856e918b92");
            public static BlueprintAbility bulls_Strength_mass = library.Get<BlueprintAbility>("6a234c6dcde7ae94e94e9c36fd1163a7");
            public static BlueprintAbility owls_strength_mass = library.Get<BlueprintAbility>("9f5ada581af3db4419b54db77f44e430");
            public static BlueprintAbility cats_grace_mass = library.Get<BlueprintAbility>("1f6c94d56f178b84ead4c02f1b1e1c48");
            public static BlueprintAbility hell_fire_ray = library.Get<BlueprintAbility>("700cfcbd0cb2975419bcab7dbb8c6210");
            public static BlueprintAbility heroism_greater = library.Get<BlueprintAbility>("e15e5e7045fda2244b98c8f010adfe31");
            public static BlueprintAbility heroic_invocation = library.Get<BlueprintAbility>("43740dab07286fe4aa00a6ee104ce7c1");
            public static BlueprintAbility creeping_doom = library.Get<BlueprintAbility>("b974af13e45639a41a04843ce1c9aa12");

            public static BlueprintAbility sea_mantle = library.Get<BlueprintAbility>("7ef49f184922063499b8f1346fb7f521");
            public static BlueprintAbility legendary_proportions = library.Get<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28");
            public static BlueprintAbility summon_elemental_greater = library.Get<BlueprintAbility>("8eb769e3b583f594faabe1cfdb0bb696");
            public static BlueprintAbility power_word_blind = library.Get<BlueprintAbility>("261e1788bfc5ac1419eec68b1d485dbc");

            public static BlueprintAbility clashing_rocks = library.Get<BlueprintAbility>("01300baad090d634cb1a1b2defe068d6");
            public static BlueprintAbility tsunami = library.Get<BlueprintAbility>("d8144161e352ca846a73cf90e85bf9ac");

            public static BlueprintAbility spike_stones = library.Get<BlueprintAbility>("d1afa8bc28c99104da7d784115552de5");
            public static BlueprintAbility slowing_mud = library.Get<BlueprintAbility>("6b30813c3709fc44b92dc8fd8191f345");
            public static BlueprintAbility obsidian_flow = library.Get<BlueprintAbility>("e48638596c955a74c8a32dbc90b518c1");
            public static BlueprintAbility crashing_despair = library.Get<BlueprintAbility>("4baf4109145de4345861fe0f2209d903");
            public static BlueprintAbility horrid_wilting = library.Get<BlueprintAbility>("08323922485f7e246acb3d2276515526");
            public static BlueprintAbility shadow_evcation_greater = library.Get<BlueprintAbility>("3c4a2d4181482e84d9cd752ef8edc3b6");
            public static BlueprintAbility shadow_evcation = library.Get<BlueprintAbility>("237427308e48c3341b3d532b9d3a001f");
            public static BlueprintAbility prismatic_spray = library.Get<BlueprintAbility>("b22fd434bdb60fb4ba1068206402c4cf");

            public static BlueprintAbility summon_elemental_elder = library.Get<BlueprintAbility>("8a7f8c1223bda1541b42fd0320cdbe2b");
            public static BlueprintAbility power_word_stun = library.Get<BlueprintAbility>("f958ef62eea5050418fb92dfa944c631");

            public static BlueprintAbility fire_snake = library.Get<BlueprintAbility>("ebade19998e1f8542a1b55bd4da766b3");
            public static BlueprintAbility storm_bolts = library.Get<BlueprintAbility>("7cfbefe0931257344b2cb7ddc4cdff6f");

            public static BlueprintAbility fiery_body = library.Get<BlueprintAbility>("08ccad78cac525040919d51963f9ac39");

            public static BlueprintAbility baleful_polymorph = library.Get<BlueprintAbility>("3105d6e9febdc3f41a08d2b7dda1fe74");
            public static BlueprintAbility tar_pool = library.Get<BlueprintAbility>("7d700cdf260d36e48bb7af3a8ca5031f");

            public static BlueprintAbility wail_of_banshee = library.Get<BlueprintAbility>("b24583190f36a8442b212e45226c54fc");
            public static BlueprintAbility energy_drain = library.Get<BlueprintAbility>("37302f72b06ced1408bf5bb965766d46");
            public static BlueprintAbility weird = library.Get<BlueprintAbility>("870af83be6572594d84d276d7fc583e0");
            public static BlueprintAbility overwhelming_presence = library.Get<BlueprintAbility>("41cf93453b027b94886901dbfc680cb9");

            public static BlueprintAbility heal = library.Get<BlueprintAbility>("5da172c4c89f9eb4cbb614f3a67357d3");
            public static BlueprintAbility change_staff = library.Get<BlueprintAbility>("26be70c4664d07446bdfe83504c1d757");

            public static BlueprintAbility foresight = library.Get<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd");
            public static BlueprintAbility elemental_swarm = library.Get<BlueprintAbility>("0340fe43f35e7a448981b646c638c83d");

            public static BlueprintAbility shapechange_ability = library.Get<BlueprintAbility>("a8237e938cc946688eb7606804b27b3c");
            public static BlueprintAbility shapechange_ability_silver_dragon_variant = library.Get<BlueprintAbility>("c31bdfd64235410aaae4fb552ef31ce3");
        }


        static public class Feats
        {
            public static BlueprintFeature natural_spell = library.Get<BlueprintFeature>("c806103e27cce6f429e5bf47067966cf");
            public static BlueprintFeature martial_weapon_proficiency = library.Get<BlueprintFeature>("203992ef5b35c864390b4e4a1e200629");
            public static BlueprintFeature augment_summoning = library.Get<BlueprintFeature>("38155ca9e4055bb48a89240a2055dcc3");
            public static BlueprintFeature superior_summoning = library.Get<BlueprintFeature>("0477936c0f74841498b5c8753a8062a3");
            public static BlueprintFeature toughness = library.Get<BlueprintFeature>("d09b20029e9abfe4480b356c92095623");
            public static BlueprintFeature combat_casting = library.Get<BlueprintFeature>("06964d468fde1dc4aa71a92ea04d930d");
            public static BlueprintFeature dodge = library.Get<BlueprintFeature>("97e216dbb46ae3c4faef90cf6bbe6fd5");
            public static BlueprintFeature power_attack = library.Get<BlueprintFeature>("9972f33f977fc724c838e59641b2fca5");
            public static BlueprintFeature cornugon_smash = library.Get<BlueprintFeature>("ceea53555d83f2547ae5fc47e0399e14");
            public static BlueprintFeature improved_initiative = library.Get<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74");
            public static BlueprintFeature arcane_strike = library.Get<BlueprintFeature>("0ab2f21a922feee4dab116238e3150b4");
            public static BlueprintParametrizedFeature spell_focus = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            public static BlueprintParametrizedFeature greater_spell_focus = library.Get<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");
            public static BlueprintFeature spell_penetration = library.Get<BlueprintFeature>("ee7dc126939e4d9438357fbd5980d459");
            public static BlueprintFeature greater_spell_penetration = library.Get<BlueprintFeature>("1978c3f91cfbbc24b9c9b0d017f4beec");

            public static BlueprintFeature elemental_focus_acid = library.Get<BlueprintFeature>("52135eada006e9045a848cd659749608");
            public static BlueprintFeature elemental_focus_cold = library.Get<BlueprintFeature>("2ed9d8bf76412ba4a8afe38fa9925fca");
            public static BlueprintFeature elemental_focus_fire = library.Get<BlueprintFeature>("13bdf8d542811ac4ca228a53aa108145");
            public static BlueprintFeature elemental_focus_elec = library.Get<BlueprintFeature>("d439691f37d17804890bd9c263ae1e80");

            public static BlueprintFeature greater_elemental_focus_acid = library.Get<BlueprintFeature>("49926dc94aca16145b6a608277b6f31c");
            public static BlueprintFeature greater_elemental_focus_cold = library.Get<BlueprintFeature>("f37a210a77d769c4ea2b23c22c07b83a");
            public static BlueprintFeature greater_elemental_focus_fire = library.Get<BlueprintFeature>("7a722c3e782aa5349a867c3516a2a4cf");
            public static BlueprintFeature greater_elemental_focus_elec = library.Get<BlueprintFeature>("6a3be3df06f555d44a2b9dbfbcc2df23");

            public static BlueprintFeature quicken_spell = library.Get<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
            public static BlueprintFeature empower_spell = library.Get<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
            public static BlueprintFeature point_blank_shot = library.Get<BlueprintFeature>("0da0c194d6e1d43419eb8d990b28e0ab");
            public static BlueprintFeature precise_shot = library.Get<BlueprintFeature>("8f3d1e6b4be006f4d896081f2f889665");
            public static BlueprintFeature improved_precise_shot = library.Get<BlueprintFeature>("46f970a6b9b5d2346b10892673fe6e74");

            public static BlueprintFeature great_fortitude = library.Get<BlueprintFeature>("79042cb55f030614ea29956177977c52");
            public static BlueprintFeature lightning_reflexes = library.Get<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e");
            public static BlueprintFeature iron_will = library.Get<BlueprintFeature>("175d1577bb6c9a04baf88eec99c66334");

            public static BlueprintFeature ability_focus_bombs = library.Get<BlueprintFeature>("f864101ab0cdb4b418c7d62d2b24eee5");
            public static BlueprintFeature rapid_shot = library.Get<BlueprintFeature>("9c928dc570bb9e54a9649b3ebfe47a41");
            public static BlueprintFeature many_shot = library.Get<BlueprintFeature>("adf54af2a681792489826f7fd1b62889");
            public static BlueprintFeature extra_bombs = library.Get<BlueprintFeature>("54c57ce67fa1d9044b1b3edc459e05e2");
            public static BlueprintFeature clustered_shots = library.Get<BlueprintFeature>("f7de245bb20f12f47864c7cb8b1d1abb");
            public static BlueprintParametrizedFeature weapon_focus = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            public static BlueprintParametrizedFeature weapon_specialization = library.Get<BlueprintParametrizedFeature>("31470b17e8446ae4ea0dacd6c5817d86");

            public static BlueprintFeature selective_channel = library.Get<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            public static BlueprintFeature extra_channel_cleric = library.Get<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");
        }


        static public class FeatSelections
        {
            public static BlueprintFeatureSelection deity_selection = library.Get<BlueprintFeatureSelection>("59e7a76987fe3b547b9cce045f4db3e4");
            public static BlueprintFeatureSelection channel_energy_selection = library.Get<BlueprintFeatureSelection>("d332c1748445e8f4f9e92763123e31bd");

            public static BlueprintFeatureSelection alchemist_discovery = library.Get<BlueprintFeatureSelection>("cd86c437488386f438dcc9ae727ea2a6");
            public static BlueprintFeatureSelection grand_discovery = library.Get<BlueprintFeatureSelection>("2729af328ab46274394cedc3582d6e98");
            public static BlueprintFeatureSelection bloodlines = library.Get<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            public static BlueprintFeatureSelection basic_feat = library.Get<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            public static BlueprintFeatureSelection wizard_bonus_feat = library.Get<BlueprintFeatureSelection>("8c3102c2ff3b69444b139a98521a4899");
            public static BlueprintFeatureSelection sorcerer_feat = library.Get<BlueprintFeatureSelection>("d6dd06f454b34014ab0903cb1ed2ade3");
            public static BlueprintFeatureSelection elemental_focus = library.Get<BlueprintFeatureSelection>("bb24cc01319528849b09a3ae8eec0b31");
            public static BlueprintFeatureSelection greater_elemental_focus = library.Get<BlueprintFeatureSelection>("1c17446a3eb744f438488711b792ca4d");
            public static BlueprintFeatureSelection school_specialization = library.Get<BlueprintFeatureSelection>("5f838049069f1ac4d804ce0862ab5110");
            public static BlueprintFeatureSelection opposition_school = library.Get<BlueprintFeatureSelection>("6c29030e9fea36949877c43a6f94ff31");
            public static BlueprintFeatureSelection arcane_bond_selection = library.Get<BlueprintFeatureSelection>("03a1781486ba98043afddaabf6b7d8ff");
            public static BlueprintFeatureSelection domain_selection = library.Get<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
            public static BlueprintFeatureSelection domain_selection2 = library.Get<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");
            public static BlueprintFeatureSelection war_domain_feat = library.Get<BlueprintFeatureSelection>("79c6421dbdb028c4fa0c31b8eea95f16");
            public static BlueprintFeatureSelection magus_feat = library.Get<BlueprintFeatureSelection>("66befe7b24c42dd458952e3c47c93563");
            public static BlueprintFeatureSelection magus_arcana = library.Get<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
        }

        static public class Bloodlines
        {
            public static BlueprintProgression red_dragon = library.Get<BlueprintProgression>("8c6e5b3cf12f71e43949f52c41ae70a8");
            public static BlueprintFeatureSelection red_dragon_claws = library.Get<BlueprintFeatureSelection>("7c150d6a5f5b4ffd8eb710c79888d273");
            public static BlueprintFeatureSelection red_dragon_resistance_selection = library.Get<BlueprintFeatureSelection>("904adc5856cd4cfc89b59716d76a73a8");
            public static BlueprintFeatureSelection bloodline_feat = library.Get<BlueprintFeatureSelection>("3a60f0c0442acfb419b0c03b584e1394");
            public static BlueprintFeatureSelection bloodline_draconic_feat_selection = library.Get<BlueprintFeatureSelection>("f4b011d090e8ae543b1441bd594c7bf7");
            public static BlueprintFeature red_dragon_resistance = library.Get<BlueprintFeature>("e3f7c6de540a3ed478fa32be32c7fc02");
            public static BlueprintFeatureSelection red_dragon_breath_selection = library.Get<BlueprintFeatureSelection>("d03a69d56d7c4caaaf60448d4dacb8d3");
            

            public static BlueprintFeatureSelection bloodline_familiar = library.Get<BlueprintFeatureSelection>("31d6089040b04ee4b79075f0f0d6e91f");
            public static BlueprintProgression undead = library.Get<BlueprintProgression>("a1a8bf61cadaa4143b2d4966f2d1142e");
            public static BlueprintFeatureSelection undead_grave_touch = library.Get<BlueprintFeatureSelection>("4879dca66a6740c18d37d617c3049c7b");
            public static BlueprintFeatureSelection deaths_gift_selection = library.Get<BlueprintFeatureSelection>("e99d43d27230401aae92effabd037438");
            public static BlueprintFeatureSelection grasp_of_the_dead_selection = library.Get<BlueprintFeatureSelection>("92e4bb3448e24b6295e390c73d64540c");
            public static BlueprintFeatureSelection bloodline_undead_feat_selection = library.Get<BlueprintFeatureSelection>("a29b72a804f7cb243b01e99c42452636");
            public static BlueprintFeature grasp_of_the_dead = library.Get<BlueprintFeature>("3c0e7252c058f324482e320755898d7a");

            public static BlueprintProgression deaths_gift = library.Get<BlueprintProgression>("69ba48b4b0424a5b82aeb800cc6cb84c");

            public static BlueprintProgression bloodline_fey = library.Get<BlueprintProgression>("e8445256abbdc45488c2d90373f7dae8");
            public static BlueprintFeatureSelection laughing_touch_selection = library.Get<BlueprintFeatureSelection>("297adb5c27444e1baf6c7f81830235bd");
            public static BlueprintFeatureSelection bloodline_fey_feat_selection = library.Get<BlueprintFeatureSelection>("d7f233e417c490545b00f49c3940638c");
            public static BlueprintFeatureSelection woodland_stride_selection = library.Get<BlueprintFeatureSelection>("647d7b9810634f78915843a4f1e64c25");
            public static BlueprintFeature woodland_stride = library.Get<BlueprintFeature>("11f4072ea766a5840a46e6660894527d");
            public static BlueprintFeatureSelection fleeting_glance_selection = library.Get<BlueprintFeatureSelection>("faba161c5f474a1196683ea5af6cb321");
            public static BlueprintFeature fleeting_glance = library.Get<BlueprintFeature>("9d67add74ffa4934aa638c2d9b573336");
        }


        static public class Domains
        {
            public static BlueprintFeature chaos = library.Get<BlueprintFeature>("5a5d19c246961484a97e1e5dded98ab2");
            public static BlueprintFeature evil = library.Get<BlueprintFeature>("a8936d29b6051a1418682da1878b644e");
            public static BlueprintFeature war = library.Get<BlueprintFeature>("8d454cbb7f25070419a1c8eaf89b5be5");
            public static BlueprintFeature glory = library.Get<BlueprintFeature>("f0a61a043bcdf0f4c8efc59962afafb8");
            public static BlueprintFeature strength = library.Get<BlueprintFeature>("07854f99c8d029b4cbfdf6ae6c7bc452");

            public static BlueprintFeature war2 = library.Get<BlueprintFeature>("82b654d68ea6ce143be5f7df646d6385");
            public static BlueprintFeature strength2 = library.Get<BlueprintFeature>("2ed973db1af2c8e428ce404fb1e9a20d");
            public static BlueprintFeature chaos2 = library.Get<BlueprintFeature>("85e8db7e938d4f947a084a21d3535adf");
        }

        static public class ClassAbilities
        {
            public static BlueprintFeature hare_familiar = library.Get<BlueprintFeature>("97dff21a036e80948b07097ad3df2b30");
            public static BlueprintFeature transmutation_specialization = library.Get<BlueprintFeature>("c459c8200e666ef4c990873d3e501b91");
            public static BlueprintFeature conjuration_specialization = library.Get<BlueprintFeature>("567801abe990faf4080df566fadcd038");
            public static BlueprintFeature necromancy_specialization = library.Get<BlueprintFeature>("e9450978cc9feeb468fb8ee3a90607e3");
            public static BlueprintFeature illusion_specialization = library.Get<BlueprintFeature>("24d5402c0c1de48468b563f6174c6256");
            public static BlueprintFeature opposition_necromancy = library.Get<BlueprintFeature>("a9bb3dcb2e8d44a49ac36c393c114bd9");
            public static BlueprintFeature opposition_divination = library.Get<BlueprintFeature>("09595544116fe5349953f939aeba7611");
            public static BlueprintFeature opposition_enchantment = library.Get<BlueprintFeature>("875fff6feb84f5240bf4375cb497e395");

            public static BlueprintFeature channel_positive = library.Get<BlueprintFeature>("a79013ff4bcd4864cb669622a29ddafb");
            public static BlueprintFeature channel_negative = library.Get<BlueprintFeature>("3adb2c906e031ee41a01bfc1d5fb7eea");

            public static BlueprintFeature arcane_accuracy = library.Get<BlueprintFeature>("2eacbdbf1c4f4134aa7fea99ab8763dc");
            public static BlueprintFeature prescient_strike = library.Get<BlueprintFeature>("a3909a7293533fe49a2d7cfe051f17e4");
            public static BlueprintFeature enduring_blade = library.Get<BlueprintFeature>("7a73bf165e8eda6478b4419f857d1ab5");
        }


        static public class Discoveries
        {
            public static BlueprintFeature choking_bomb = library.Get<BlueprintFeature>("b3c6cb76d5b11cf4c8314d7b1c7b9b8b");
            public static BlueprintFeature force_bomb = library.Get<BlueprintFeature>("bfbaa0dd74b9909459e462cd8b091177");
            public static BlueprintFeature blinding_bomb = library.Get<BlueprintFeature>("c3da68b2222768b4f9352fefd29ad15c");
            public static BlueprintFeature precise_bomb = library.Get<BlueprintFeature>("5c396342f614dd644a48c3af08d79701");
            public static BlueprintFeature fast_bombs = library.Get<BlueprintFeature>("128c5fccec5ca724281a4907b1f0ac83");
            public static BlueprintFeature infusion = library.Get<BlueprintFeature>("57d5077b301ade749b840b0ea9230bb9");
            public static BlueprintFeature cognatogen = library.Get<BlueprintFeature>("e3f460ea61fcc504183c7d6818bbbf7a");
            public static BlueprintFeature greater_cognatogen = library.Get<BlueprintFeature>("18eb29676492e844eb5a55d1c855ce69");
            public static BlueprintFeature grand_cognatogen = library.Get<BlueprintFeature>("af4a320648eb5724889d6ff6255090b2");
            public static BlueprintFeature preserve_organs = library.Get<BlueprintFeature>("76b4bb8e54f3f5c418f421684c76ef4e");
            public static BlueprintFeature mummification = library.Get<BlueprintFeature>("daf854d84d442e941aa3a2fdc041b37c");

            public static BlueprintFeature awakened_intellect = library.Get<BlueprintFeature>("d5378cb816b0c7a4cb47500e6ccf8702");
        }


        static public class Deities
        {
            public static BlueprintFeature gorum = library.Get<BlueprintFeature>("8f49a5d8528a82c44b8c117a89f6b68c");
            public static BlueprintFeature callistria = library.Get<BlueprintFeature>("c7531715a3f046d4da129619be63f44c");
        }


        static class Wildshapes
        {
            public static BlueprintAbility wolf = library.Get<BlueprintAbility>("ac8811714a45a5948b27208538ce4f03");
            public static BlueprintAbility leopard = library.Get<BlueprintAbility>("92c47b04f6c9aa44abf1693b32554804");
            public static BlueprintAbility bear = library.Get<BlueprintAbility>("3f7873efe4ca499bae86489d1d993442");
            public static BlueprintAbility dire_wolf = library.Get<BlueprintAbility>("aa0f3e6715e74416b7f5cfb05fab3040");
            public static BlueprintAbility mastodon = library.Get<BlueprintAbility>("aa6e7d52069d40af969520d5cccf349e");
            public static BlueprintAbility mandragora = library.Get<BlueprintAbility>("3e91b07a95cb42efb352492a80bd3637");
            public static BlueprintAbility giant_flytrap = library.Get<BlueprintAbility>("f5e3fe2fdb5c434caa4a83a7d2bb6cc8");
            public static BlueprintAbility smilodon = library.Get<BlueprintAbility>("32f1f208ad635224f89ef158140ab509");
            public static BlueprintAbility shambling_mound = library.Get<BlueprintAbility>("943d41b6aaef1dc4e82f115118dbf902");
            public static BlueprintAbility treant = library.Get<BlueprintAbility>("6356f85d679148dd8bb064aebf85aa37");
        }


        static class Arcana
        {
            public static BlueprintAbility aracne_accuracy = library.Get<BlueprintAbility>("1b7fb8120390ca24c9da98ce87780b7f");
            public static BlueprintAbility prescient_attack = library.Get<BlueprintAbility>("fa12d155c229c134dbbbebf0d7b980f0");
            public static BlueprintAbility arcane_weapon_switch = library.Get<BlueprintAbility>("3c89dfc82c2a3f646808ea250eb91b91");
        }



        static void initializeDefines()
        {
            Considerations.light_armor_consideration = library.CopyAndAdd<ArmorTypeConsideration>("2ba801c8a6f585749b7fd636e843e6f0", "LightArmorOnlyConsideration", "");
            Considerations.light_armor_consideration.HeavyArmorScore = 0.0f;
            Considerations.heavy_armor_consideration = library.CopyAndAdd<ArmorTypeConsideration>("c376d918c01838b48befcb711cc528ff", "HeavyArmorOnlyConsideration", "");
            Considerations.heavy_armor_consideration.LightArmorScore = 0.0f;

            Considerations.is_enemy = Helpers.Create<NewConsiderations.TargetFactionConsideration>();
            Considerations.is_enemy.enemy_score = 1.00f;
            Considerations.is_enemy.name = "IsEnemyConsideration";
            library.AddAsset(Considerations.is_enemy, "");

            Considerations.is_ally = Helpers.Create<NewConsiderations.TargetFactionConsideration>();
            Considerations.is_ally.ally_score = 1.00f;
            Considerations.is_ally.name = "IsAllyConsideration";
            library.AddAsset(Considerations.is_ally, "");


            Considerations.higher_bab = Helpers.Create<NewConsiderations.BabPartConsideration>(b => { b.min_bab_part = 0.7f; });
            Considerations.higher_bab.name = "HigherBaBConsideration";
            library.AddAsset(Considerations.higher_bab, "");

            Considerations.not_polymorphed = Helpers.Create<NewConsiderations.UnitPolymorphed>(p => { p.polymorphed_score = 0.0f; p.not_polymorphed_score = 1.0f; });
            Considerations.not_polymorphed.name = "NotPolymorphedConsideration";
            library.AddAsset(Considerations.not_polymorphed, "");

            Considerations.light_armor_around_enemies_consideration = Helpers.Create<NewConsiderations.ArmorAroundConsideration>(a => { a.is_light = true; a.Filter = TargetType.Enemy; a.MaxScore = 0.99f; a.MaxCount = 3; a.BaseScoreModifier = 1.0f; a.ExtraTargetScore = 0.001f; });
            Considerations.light_armor_around_enemies_consideration.name = "LightArmorAroundEnemiesConsideration";
            library.AddAsset(Considerations.light_armor_around_enemies_consideration, "");

            Considerations.heavy_armor_around_enemies_consideration = Helpers.Create<NewConsiderations.ArmorAroundConsideration>(a => { a.is_light = true; a.Filter = TargetType.Enemy; a.MaxScore = 0.99f; a.MaxCount = 3; a.BaseScoreModifier = 1.0f; a.ExtraTargetScore = 0.001f; });
            Considerations.heavy_armor_around_enemies_consideration.name = "HeavyArmorAroundEnemiesConsideration";
            library.AddAsset(Considerations.heavy_armor_around_enemies_consideration, "");


            Considerations.choose_more_friends = library.CopyAndAdd<UnitsAroundConsideration>("96a5a05d98be03446bbf21e217270b06", "ChooseAtLeastOnAllyConsiderationAi", "");
            Considerations.choose_more_friends.Filter = Kingmaker.Controllers.Brain.Blueprints.TargetType.Friend;
        }

    }
}
