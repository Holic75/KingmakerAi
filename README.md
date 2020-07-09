# Kingmaker AI

This is a mod for Pathfinder: Kingmaker that aims to make game a bit more challenging.
It requires at least Proper Flanking 2 1.19c and Call of the Wild 1.99h.


Mod also provides a limited ability to make party members execute scripts (for pre-buffing for example)
Script examples are provide in /ScriptExamples folder

### Possible commands:

- cast ability_name[.variant] target_descriptor
- move_to target_descriptor
- activate ability_name
- deactivate ability_name
 
ability_name and variant are defined by their internal names (you can use Spacehamster BlueprintDump, DataViewer or Bag of Tricks mods to find them).

target_descriptor = [1][2][3][4][5][6][7][8][9][s][p]

1 - 9 - correspond to unit tags (i.e action will be applied to all unit that have at least one tag, so 123 will be applied to anyone who has tag 1, 2 or 3)

s - apply on self

p - apply on own pet


Install
- Download and install Unity Mod Manager﻿﻿ 0.13.0 or later
- Download the mod
- Build it using Visual Studio 2017 Community Edition or use prebuilt binaries from latest Releases (just drop archive into UMM GUI)
- Run the game
