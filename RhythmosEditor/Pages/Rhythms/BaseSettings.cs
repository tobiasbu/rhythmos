using UnityEditor;
using RhythmosEditor.Commands;
using RhythmosEditor.Utils;
using UnityEngine;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class BaseSettings
    {
        private float oldBpm;
        private float newBpm;
        private bool changingBpm = false;
        private UndoRedo.DelayedRecordHolder bpmRecorder;

        private TextInput nameInput;

        internal EditController controller;

        public void OnLoad()
        {
            if (nameInput == null)
            {
                nameInput = new TextInput();
                nameInput.Label = "Rhythm name";
            }

        }

        public void OnRhythmChange()
        {
            nameInput.Text = controller.Name;
            changingBpm = false;
            bpmRecorder?.Apply();
            bpmRecorder = null;
        }

        internal void Draw()
        {
            

            if (nameInput.Draw())
            {
                UndoRedo.Record(new Commands.EditRhythm.Rename(controller, nameInput.Text));
            }

            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("Tempo");
            float bpm = EditorGUILayout.FloatField("Tempo (BPM)", controller.BPM);
            if (EditorGUI.EndChangeCheck())
            {
                if (bpm != controller.BPM)
                {
                    if (changingBpm == false)
                    {
                        oldBpm = controller.BPM;
                        changingBpm = true;
                    }

                    if (bpm < 0.01f)
                    {
                        bpm = 0.01f;
                    }

                    if (bpm > 1000)
                    {
                        bpm = 1000;
                    }

                    newBpm = bpm;
                    controller.Rhythm.BPM = bpm;

                    if (bpmRecorder == null)
                    {
                        bpmRecorder = new UndoRedo.DelayedRecordHolder
                        {
                            command = () =>
                            {
                                changingBpm = false;
                                return new Commands.EditRhythm.ChangeBPM(controller, newBpm, oldBpm);
                            }
                        };
                    }
                    UndoRedo.DelayedRecord(bpmRecorder);
                }

               
            }

        }
    }
}
