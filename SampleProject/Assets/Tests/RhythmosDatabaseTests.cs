using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RhythmosEngine;
using UnityEditor;

public class RhythmosDatabaseTests
{

    // A Test behaves as an ordinary method
    [Test]
    public void LoadsRhythmosDatabaseSucessfully()
    {
        TextAsset textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Tests/Files/RhythmosDatabaseV1.xml", typeof(TextAsset));

        RhythmosDatabase rhythmosDatabase = RhythmosDatabase.Load(textAsset);

        Assert.IsNotNull(rhythmosDatabase);
    }

    
}
