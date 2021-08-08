# Code Examples

This session has code examples to implementing the RhythmosEngine in your game. Make sure to read the installation guide.

- [Quick start](#quick-start)
- [Singleton pattern for RhythmosDatabase](#singleton-pattern-for-rhythmosdatabase)
- [Creating rhythms in runtime](#creating-rhythms-in-runtime)
- [Playing rhythms](#playing-rhythms)

## Quick Start

The `RhythmosDatabase` class loads your rhythms from XML file that you have created in RhtyhmosEditor. The code below is simplest way to load RhythmosDatabase:

```csharp
using System.Collections;
using UnityEngine;

using RhythmosEngine;

public class RhythmsComponent : MonoBehaviour {

    private RhythmosDatabase rhythmosDatabase;

    // IMPORTANT: Reference the XML file via Unity inspector
    public TextAsset rhythmosFile;

    void Awake()
    {
        try 
        {
            // Loads the RhythmosDatabase from TextAsset
            rhythmosDatabase = RhythmosDatabase.Load(rhythmosFile);
        } 
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void Start()
    {
        // Display in the console the amount of rhythms and note layouts.
        Debug.Log("Total of Rhythms: " + rhythmosDatabase.RhythmsCount);
        
        Debug.Log("Total of NoteLayout: " + rhythmosDatabase.NoteLayoutCount);
    }

}
```

## Singleton pattern for RhythmosDatabase

If your game use a lot of rhythms all of the time create a global component of rhythms. Use a single `GameObject` that's in charge of our rhythms storage class.

```csharp
using UnityEngine;
using System.Collections;

using RhythmosEngine;

public class GlobalRhythms : MonoBehaviour {

    public RhythmosDatabase rhythmosDatabase;

    private static GlobalRhythms instance;

    // get the instance of GlobalRhythms
    public static GlobalRhythms globalInstance { 
        get { 
            if (instance == null){
            // In Unity above of 4.2 or above use this line of code:
            //_instance = GameObject.FindObjectOfType<MusicManager>();
            instance = (GlobalRhythms)GameObject.FindObjectOfType(typeof(GlobalRhythms));
            // Set at unity this GameObject can't be removed.
            DontDestroyOnLoad(instance.gameObject);
            }
        return instance;
        }
    }

    void Awake() { 
        if (instance == null) {
            // If don't have a singleton and this component is the first, make this component the singleton
            instance = this;

            // load a RhythmosDatabase file from the Resources directory
            rhythmosDatabase = RhythmosDatabase.Load("Files/RhythmosDatabase");

            DontDestroyOnLoad(this);

        } else {
            // Destroy another GlobalRhythms if already exist this singleton.
            if (this != instance) {
                Destroy(this.gameObject);
            }
        }
    }
}

```

## Creating rhythms in runtime

```csharp
Rhythm CreateRhythm() {
    Rhythm rhythm = new Rhythm();

    rhythm.Name = "Jazzy Beat";
    rhythm.BPM = 120;

    // remember: layout index is a index reference to a RhythmosDatabase
    Note note1 = new Note(0.5f,false,0);
    Note note2 = new Note(0.25f,false,1);
    Note note3 = new Note(0.5f,false,0); 

    rhythm.AppendNote(note1);
    rhythm.AppendNote(note2);
    rhythm.AppendNote(note3);

    return rhythm;
}
```

## Playing rhythms

The following snippet just plays a Rhythm from loaded `RhythmosDatabase`.

```csharp
void PlayRhythm() {
    // play a rhythm from a loaded RhythmosDatabase
    RhythmosPlayer player = rhythmosDatabase.PlayRhythm("MyBeautifulRhythm", 1f);

    player.loop = true;
    player.destroyOnEnd = false;
}
```
