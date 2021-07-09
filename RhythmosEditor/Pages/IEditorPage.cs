using UnityEngine;
using RhythmosEditor.Settings;
using System;

namespace RhythmosEditor
{

    internal interface IEditorPage
    {
        void OnLoad();
        void OnPageSelect(RhythmosConfig config);
        void OnDraw(Rect pageRect);
    }
}
