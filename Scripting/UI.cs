using Kingmaker.EntitySystem.Entities;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace KingmakerAI.Scripting
{
    public class UI
    {
        static Dictionary<string, string> character_script_file_map = new Dictionary<string, string>();
        static Dictionary<string, ScriptController.UnitTags> character_tags_map = new Dictionary<string, ScriptController.UnitTags>();

        static string working_folder;
        static string party_info_file;

        class CharacterEntry
        {
            [JsonProperty]
            public string name { get; set; }
            [JsonProperty]
            public string tags { get; set; }
            [JsonProperty]
            public string script_file { get; set; }
        }


        static public void initialize(string initial_working_folder, string initial_party_info_file)
        {
            working_folder = initial_working_folder;
            party_info_file = initial_party_info_file;
            loadPartyInfo();
        }


        static void savePartyInfo()
        {
            List<CharacterEntry> char_entries = new List<CharacterEntry>();

            var all_tags = Enum.GetValues(typeof(ScriptController.UnitTags)).Cast<ScriptController.UnitTags>().ToArray();
            foreach (var c in character_script_file_map)
            {
                if (!character_tags_map.ContainsKey(c.Key))
                {
                    continue;
                }
                var ce = new CharacterEntry();
                ce.name = c.Key;
                ce.script_file = c.Value;

                ce.tags = "";
                var tag = character_tags_map[c.Key];
                for (int i =  1; i < all_tags.Length; i++)
                {
                    if ((tag & all_tags[i]) != 0)
                    {
                        ce.tags = ce.tags + ScriptController.unit_tags_char[i];
                    }
                }
                char_entries.Add(ce);
            }
            string json = JsonConvert.SerializeObject(char_entries, Formatting.Indented);

            try
            {
                System.IO.File.WriteAllText(working_folder + party_info_file, json);
            }
            catch (Exception e)
            {
                Main.logger.Log("Failed to save party info: unable to write to " + working_folder + party_info_file);
                return;
            }
        }


        static void loadPartyInfo()
        {
            try
            {
                var all_tags = Enum.GetValues(typeof(ScriptController.UnitTags)).Cast<ScriptController.UnitTags>().ToArray();
                var json = File.ReadAllText(working_folder + party_info_file);


                List<CharacterEntry> char_entries = JsonConvert.DeserializeObject<List<CharacterEntry>>(json);

                foreach (var c in char_entries)
                {
                    character_script_file_map[c.name] = c.script_file;

                    string tag_string = c.tags;
                    ScriptController.UnitTags tag = ScriptController.UnitTags.None;

                    foreach (var chr in tag_string)
                    {
                        int num = Array.FindIndex(ScriptController.unit_tags_char, val => val == chr);
                        if (num > 0 && num < all_tags.Length)
                        {
                            tag = tag | all_tags[num];
                        }
                    }

                    character_tags_map[c.name] = tag;
                }
            }
            catch (Exception e)
            {
                Main.logger.Log("Failed to load party info");
                return;
            }
        }


        static void saveSettings()
        {
            Main.settings.scripts_working_folder = working_folder;
            Main.settings.party_info_file = party_info_file;

            try
            {
                string json = JsonConvert.SerializeObject(Main.settings, Formatting.Indented);
                System.IO.File.WriteAllText(UnityModManager.modsPath + @"/KingmakerAi/settings.json", json);
            }
            catch (Exception e)
            {
                Main.logger.Log("Failed to save settings");
                return;
            }
        }


        static Dictionary<string, UnitEntityData> getPartyWithNames()
        {
            Dictionary<string, UnitEntityData> units_with_names = new Dictionary<string, UnitEntityData>();
            var party = Kingmaker.Game.Instance?.Player?.Party;
            if (party == null || party.Empty())
            {
                return units_with_names;
            }

            foreach (var c in party)
            {
                units_with_names.Add(c.CharacterName, c);
                if (c.Descriptor.Pet != null)
                {
                    units_with_names.Add(c.CharacterName + "." + c.Descriptor.Pet.CharacterName, c.Descriptor.Pet);
                }
            }
            return units_with_names;
        }


        public static void onGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Scripts");
            var party = getPartyWithNames();
            if (party.Empty())
            {
                return;
            }

            using (new GUILayout.VerticalScope())
            {
                var all_tags = Enum.GetValues(typeof(ScriptController.UnitTags)).Cast<ScriptController.UnitTags>().ToArray();
                foreach (var c in party)
                {
                    using (new GUILayout.VerticalScope())
                    {
                        var unit_name = c.Key;
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(unit_name, GUILayout.Width(100f));
                            if (!character_tags_map.ContainsKey(unit_name))
                            {
                                character_tags_map[unit_name] = ScriptController.UnitTags.None;
                            }

                            var tags = character_tags_map[unit_name];
                            for (int i = 1; i < all_tags.Length; i++)
                            {
                                bool val = GUILayout.Toggle((tags & all_tags[i]) != 0, ScriptController.unit_tags_char[i].ToString());
                                if (val)
                                {
                                    tags = tags | all_tags[i];
                                }
                                else
                                {
                                    tags = tags & (~all_tags[i]);
                                }
                            }
                            character_tags_map[unit_name] = tags;
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label("Script file:", GUILayout.Width(100f));
                            character_script_file_map[unit_name] = GUILayout.TextField(character_script_file_map.ContainsKey(unit_name) ? character_script_file_map[unit_name] : "", GUILayout.Width(600f));
                        }
                    }
                }
            }


            if (GUILayout.Button("Run Scripts", GUILayout.Width(100f)))
            {
                ScriptController.reset();
                Dictionary<UnitEntityData, ScriptController.UnitTags> party_with_tags = new Dictionary<UnitEntityData, ScriptController.UnitTags>();
                foreach (var c in party)
                {
                    if (character_tags_map.ContainsKey(c.Key))
                    {
                        party_with_tags.Add(c.Value, character_tags_map[c.Key]);
                    }
                }
                ScriptController.registerParty(party_with_tags);

                foreach (var c in character_script_file_map)
                {
                    if (!c.Value.Empty() && party.ContainsKey(c.Key))
                    {
                        ScriptController.registerScriptFile(working_folder + c.Value, party[c.Key]);
                    }
                }
                ScriptController.run();
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Party info file:", GUILayout.Width(100f));
                party_info_file = GUILayout.TextField(party_info_file, GUILayout.Width(600f));

                if (GUILayout.Button("Save", GUILayout.Width(100f)))
                {
                    savePartyInfo();
                }

                if (GUILayout.Button("Load", GUILayout.Width(100f)))
                {
                    loadPartyInfo();
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Working folder:", GUILayout.Width(100f));
                working_folder = GUILayout.TextField(working_folder, GUILayout.Width(600f));
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Save Settings", GUILayout.Width(100f)))
                {
                    saveSettings();
                }
                if (GUILayout.Button("Load Settings", GUILayout.Width(100f)))
                {
                    Main.settings = new Main.Settings();
                    initialize(Main.settings.scripts_working_folder, Main.settings.party_info_file);
                }
            }

        }
    }
}
