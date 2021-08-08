# Downloads and Installation Guide

You can get the Rhythmos Engine in the following links:

| Version | Supported Unity Version          | Link |
| ------- | -------------------------------- | --------------------- |
| v1.3    | >= 2020.3.13f1             | [Download here](https://github.com/tobiasbu/rhythmos-engine/releases/tag/v1.3) |
| v1.1    | 3.6, 4.6.0f, 5 and 2017 | [Get in Unity Asset Store](https://assetstore.unity.com/packages/tools/audio/rhythmos-engine-39835) |

## Installation

The Rhythmos Engine has two main files:

- **RhythmosEngine.dll**: contains a collection of classes and structures to assist the using of rhythms in game runtime. 
Installation location on Unity: `Assets/Plugins/RhythmosEngine`

- **RhythmosEditor.dll**: It's an Unity Editor window to create your own rhythms and notes database. This file must be in `Assets/Editor/RhythmosEditor` directory. 
    To open the Rhythmos Editor, in Unity Toolbar go to **Tools** > **Rhythmos Editor**


## Important Notes

- All notes sounds - the AudioClips - need to be located in the `Assets/Resources` directory to play sounds during the game.
- The rhythms database is on `XML` format. It brings together the entire contents of rhythms and notes that you have created. If you want to load this file in runtime with `TextAsset`, the database must be in the `Assets/Resources` directory too.
- RhythmosEditor.dll save a configuration file in `Assets/Editor` directory only to load the last edited database.
