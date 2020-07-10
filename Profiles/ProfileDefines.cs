using CallOfTheWild;
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


        public static class AiActions
        {
            static public BlueprintAiCastSpell acid_splash_ai_action = library.Get<BlueprintAiCastSpell>("8cf4732cf870f8f4cbf760331c8f2696");
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
             public static Consideration[] harmful_enemy_ally_aoe_target_consideration = new Consideration[] {avoid_friends, aoe_at_least_one_enemy_considertion, aoe_more_enemies_considertion, attack_target_prioriteis };
             public static Consideration[] harmful_enemy_aoe_target_consideration = new Consideration[] { aoe_at_least_one_enemy_considertion, aoe_more_enemies_considertion, attack_target_prioriteis};

             public static Consideration light_armor_consideration = library.Get<ArmorTypeConsideration>("2ba801c8a6f585749b7fd636e843e6f0");
             public static Consideration heavy_armor_consideration = library.Get<ArmorTypeConsideration>("c376d918c01838b48befcb711cc528ff");

             public static Consideration target_self_consideration = library.Get<TargetSelfConsideration>("83e2dd97b82d769498394c3edf0d260e");
             public static Consideration target_other_consideration = library.Get<TargetSelfConsideration>("f4be6fc6f46b61044a44715f99f1918d");

             public static Consideration unconcious_penalty = library.Get<LifeStateConsideration>("bf033da33480da84f8265f5f8bcdd467");
             public static Consideration stand_still = library.Get<InRangeConsideration>("da2c75bc93165b749b18654b21759ea3");
             public static Consideration in_range = library.Get<InRangeConsideration>("804d2cf0fb1815948b04ec697cbfdbf4");

             public static NewConsiderations.TargetFactionConsideration is_enemy;
             public static NewConsiderations.TargetFactionConsideration is_ally;
        }

        public static class Spells
        {
            public static BlueprintAbility acid_splash = library.Get<BlueprintAbility>("0c852a2405dd9f14a8bbcfaf245ff823");

            public static BlueprintAbility magic_missile = library.Get<BlueprintAbility>("4ac47ddb9fa1eaf43a1b6809980cfbd2");
            public static BlueprintAbility bless = library.Get<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638");
            public static BlueprintAbility shield_of_faith = library.Get<BlueprintAbility>("183d5bb91dea3a1489a6db6c9cb64445");
            public static BlueprintAbility enlarge_person = library.Get<BlueprintAbility>("c60969e7f264e6d4b84a1499fdcf9039");
            public static BlueprintAbility reduce_person = library.Get<BlueprintAbility>("4e0e9aba6447d514f88eff1464cc4763");
            public static BlueprintAbility divine_favor = library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113");
            public static BlueprintAbility mage_armor = library.Get<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568");
            public static BlueprintAbility mage_shield = library.Get<BlueprintAbility>("ef768022b0785eb43a18969903c537c4");
            public static BlueprintAbility grease = library.Get<BlueprintAbility>("95851f6e85fe87d4190675db0419d112");

            public static BlueprintAbility bulls_strength = library.Get<BlueprintAbility>("4c3d08935262b6544ae97599b3a9556d");
            public static BlueprintAbility foxs_cunning = library.Get<BlueprintAbility>("ae4d3ad6a8fda1542acf2e9bbc13d113");
            public static BlueprintAbility owls_wisdom = library.Get<BlueprintAbility>("f0455c9295b53904f9e02fc571dd2ce1");
            public static BlueprintAbility cats_grace = library.Get<BlueprintAbility>("de7a025d48ad5da4991e7d3c682cf69d");
            public static BlueprintAbility eagles_splendor = library.Get<BlueprintAbility>("446f7bf201dc1934f96ac0a26e324803");
            public static BlueprintAbility mirror_image = library.Get<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564");
            public static BlueprintAbility create_pit = library.Get<BlueprintAbility>("29ccc62632178d344ad0be0865fd3113");
            public static BlueprintAbility glitter_dust = library.Get<BlueprintAbility>("ce7dad2b25acf85429b6c9550787b2d9");
            public static BlueprintAbility acid_arrow = library.Get<BlueprintAbility>("9a46dfd390f943647ab4395fc997936d");

            public static BlueprintAbility divine_power = library.Get<BlueprintAbility>("ef16771cb05d1344989519e87f25b3c5");
            public static BlueprintAbility prayer = library.Get<BlueprintAbility>("faabd2cc67efa4646ac58c7bb3e40fcc");
            public static BlueprintAbility haste = library.Get<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98");
            public static BlueprintAbility slow = library.Get<BlueprintAbility>("f492622e473d34747806bdb39356eb89");
            public static BlueprintAbility spiked_pit = library.Get<BlueprintAbility>("46097f610219ac445b4d6403fc596b9f");
            public static BlueprintAbility stinking_cloud = library.Get<BlueprintAbility>("68a9e6d7256f1354289a39003a46d826");
            public static BlueprintAbility acid_pit = library.Get<BlueprintAbility>("1407fb5054d087d47a4c40134c809f12");
            public static BlueprintAbility controlled_fireball = library.Get<BlueprintAbility>("f72f8f03bf0136c4180cd1d70eb773a5");

            public static BlueprintAbility elemental_body1 = library.Get<BlueprintAbility>("690c90a82bf2e58449c6b541cb8ea004");
            public static BlueprintAbility elemental_body2 = library.Get<BlueprintAbility>("6d437be73b459594ab103acdcae5b9e2");
            public static BlueprintAbility elemental_body3 = library.Get<BlueprintAbility>("459e6d5aab080a14499e13b407eb3b85");
            public static BlueprintAbility elemental_body4 = library.Get<BlueprintAbility>("376db0590f3ca4945a8b6dc16ed14975");

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

            public static BlueprintAbility acidic_spray = library.Get<BlueprintAbility>("c543eef6d725b184ea8669dd09b3894c");
            public static BlueprintAbility cloudkill = library.Get<BlueprintAbility>("548d339ba87ee56459c98e80167bdf10");
            public static BlueprintAbility hungry_pit = library.Get<BlueprintAbility>("f63f4d1806b78604a952b3958892ce1c");

            public static BlueprintAbility phantasmal_putrefaction = library.Get<BlueprintAbility>("1f2e6019ece86d64baa5effa15e81ecc");
            public static BlueprintAbility cold_ice_strike = library.Get<BlueprintAbility>("5ef85d426783a5347b420546f91a677b");
            public static BlueprintAbility mind_fog = library.Get<BlueprintAbility>("eabf94e4edc6e714cabd96aa69f8b207");
            public static BlueprintAbility phantasmal_killer = library.Get<BlueprintAbility>("6717dbaef00c0eb4897a1c908a75dfe5");
            public static BlueprintAbility phantasmal_web = library.Get<BlueprintAbility>("12fb4a4c22549c74d949e2916a2f0b6a");
            public static BlueprintAbility fireball = library.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            public static BlueprintAbility acid_fog = library.Get<BlueprintAbility>("dbf99b00cd35d0a4491c6cc9e771b487");
            public static BlueprintAbility sirocco = library.Get<BlueprintAbility>("093ed1d67a539ad4c939d9d05cfe192c");
            public static BlueprintAbility chains_of_light = library.Get<BlueprintAbility>("f8cea58227f59c64399044a82c9735c4");

            public static BlueprintAbility serenity = library.Get<BlueprintAbility>("d316d3d94d20c674db2c24d7de96f6a7");
            public static BlueprintAbility cloak_of_dreams = library.Get<BlueprintAbility>("7f71a70d822af94458dc1a235507e972");
            public static BlueprintAbility confusion = library.Get<BlueprintAbility>("cf6c901fb7acc904e85c63b342e9c949");
            public static BlueprintAbility fear = library.Get<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");
            public static BlueprintAbility false_life = library.Get<BlueprintAbility>("7a5b5bf845779a941a67251539545762");
            public static BlueprintAbility false_life_greater = library.Get<BlueprintAbility>("dc6af3b4fd149f841912d8a3ce0983de");

            public static BlueprintAbility chain_lightning = library.Get<BlueprintAbility>("645558d63604747428d55f0dd3a4cb58");
            public static BlueprintAbility resonating_word = library.Get<BlueprintAbility>("df7d13c967bce6a40bec3ba7c9f0e64c");
            public static BlueprintAbility bears_endurance_mass = library.Get<BlueprintAbility>("f6bcea6db14f0814d99b54856e918b92");

            public static BlueprintAbility frightful_aspect = library.Get<BlueprintAbility>("e788b02f8d21014488067bdd3ba7b325");
            public static BlueprintAbility rift_of_ruin = library.Get<BlueprintAbility>("dd3dacafcf40a0145a5824c838e2698d");
            public static BlueprintAbility sea_mantle = library.Get<BlueprintAbility>("7ef49f184922063499b8f1346fb7f521");

            public static BlueprintAbility clashing_rocks = library.Get<BlueprintAbility>("01300baad090d634cb1a1b2defe068d6");
            public static BlueprintAbility tsunami = library.Get<BlueprintAbility>("d8144161e352ca846a73cf90e85bf9ac");

            public static BlueprintAbility spike_stones = library.Get<BlueprintAbility>("d1afa8bc28c99104da7d784115552de5");
            public static BlueprintAbility slowing_mud = library.Get<BlueprintAbility>("6b30813c3709fc44b92dc8fd8191f345");
            public static BlueprintAbility obsidian_flow = library.Get<BlueprintAbility>("e48638596c955a74c8a32dbc90b518c1");
            public static BlueprintAbility flame_strike = library.Get<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");

            public static BlueprintAbility fire_snake = library.Get<BlueprintAbility>("ebade19998e1f8542a1b55bd4da766b3");
            public static BlueprintAbility fire_storm = library.Get<BlueprintAbility>("e3d0dfe1c8527934294f241e0ae96a8d");
            public static BlueprintAbility storm_bolts = library.Get<BlueprintAbility>("7cfbefe0931257344b2cb7ddc4cdff6f");

            public static BlueprintAbility fiery_body = library.Get<BlueprintAbility>("08ccad78cac525040919d51963f9ac39");

            public static BlueprintAbility baleful_polymorph = library.Get<BlueprintAbility>("3105d6e9febdc3f41a08d2b7dda1fe74");
            public static BlueprintAbility tar_pool = library.Get<BlueprintAbility>("7d700cdf260d36e48bb7af3a8ca5031f");
        }


        static public class Feats
        {
            public static BlueprintFeature augment_summoning = library.Get<BlueprintFeature>("38155ca9e4055bb48a89240a2055dcc3");
            public static BlueprintFeature superior_summoning = library.Get<BlueprintFeature>("0477936c0f74841498b5c8753a8062a3");
            public static BlueprintFeature toughness = library.Get<BlueprintFeature>("d09b20029e9abfe4480b356c92095623");
            public static BlueprintFeature combat_casting = library.Get<BlueprintFeature>("06964d468fde1dc4aa71a92ea04d930d");
            public static BlueprintFeature dodge = library.Get<BlueprintFeature>("97e216dbb46ae3c4faef90cf6bbe6fd5");
            public static BlueprintFeature improved_initiative = library.Get<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74");
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
        }


        static public class FeatSelections
        {
            public static BlueprintFeatureSelection basic_feat = library.Get<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            public static BlueprintFeatureSelection wizard_bonus_feat = library.Get<BlueprintFeatureSelection>("8c3102c2ff3b69444b139a98521a4899");
            public static BlueprintFeatureSelection elemental_focus = library.Get<BlueprintFeatureSelection>("bb24cc01319528849b09a3ae8eec0b31");
            public static BlueprintFeatureSelection greater_elemental_focus = library.Get<BlueprintFeatureSelection>("1c17446a3eb744f438488711b792ca4d");
            public static BlueprintFeatureSelection school_specialization = library.Get<BlueprintFeatureSelection>("5f838049069f1ac4d804ce0862ab5110");
            public static BlueprintFeatureSelection opposition_school = library.Get<BlueprintFeatureSelection>("6c29030e9fea36949877c43a6f94ff31");
            public static BlueprintFeatureSelection arcane_bond_selection = library.Get<BlueprintFeatureSelection>("03a1781486ba98043afddaabf6b7d8ff");
        }

        static public class ClassAbilities
        {
            public static BlueprintFeature hare_familiar = library.Get<BlueprintFeature>("97dff21a036e80948b07097ad3df2b30");
            public static BlueprintFeature transmutation_specialization = library.Get<BlueprintFeature>("c459c8200e666ef4c990873d3e501b91");
            public static BlueprintFeature conjuration_specialization = library.Get<BlueprintFeature>("567801abe990faf4080df566fadcd038");
            public static BlueprintFeature opposition_necromancy = library.Get<BlueprintFeature>("a9bb3dcb2e8d44a49ac36c393c114bd9");
            public static BlueprintFeature opposition_divination = library.Get<BlueprintFeature>("09595544116fe5349953f939aeba7611");
        }


        static void initializeDefines()
        {
            Considerations.is_enemy = Helpers.Create<NewConsiderations.TargetFactionConsideration>();
            Considerations.is_enemy.enemy_score = 1.00f;
            Considerations.is_enemy.name = "IsEnemyConsideration";
            library.AddAsset(Considerations.is_enemy, "");

            Considerations.is_ally = Helpers.Create<NewConsiderations.TargetFactionConsideration>();
            Considerations.is_ally.ally_score = 1.00f;
            Considerations.is_ally.name = "IsAllyConsideration";
            library.AddAsset(Considerations.is_ally, "");


            Considerations.choose_more_friends = library.CopyAndAdd<UnitsAroundConsideration>("96a5a05d98be03446bbf21e217270b06", "ChooseAtLeastOnAllyConsiderationAi", "");
            Considerations.choose_more_friends.Filter = Kingmaker.Controllers.Brain.Blueprints.TargetType.Friend;
        }

    }
}
