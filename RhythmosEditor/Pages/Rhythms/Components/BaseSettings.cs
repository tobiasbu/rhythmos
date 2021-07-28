using UnityEngine;
using UnityEditor;
using RhythmosEngine;
using RhythmosEditor.UI;
using RhythmosEditor.Commands;
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
        internal Player player;

        public void OnLoad()
        {
            if (nameInput == null)
            {
                nameInput = new TextInput();
                nameInput.Label = "Rhythm name";
            }
            nameInput.Text = "";

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
                UndoRedo.Record(new Commands.EditRhythm.RenameRhythm(controller, nameInput.Text));
            }


            Color oldGuiColor = GUI.color;
            if (player.IsPlaying)
            {
                GUI.color = Colors.Record;
            }

            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("Tempo");
            float bpm = EditorGUILayout.FloatField("Tempo (BPM)", controller.Bpm);
            if (EditorGUI.EndChangeCheck())
            {
                Rhythm rhythm = controller.Rhythm;
                if (rhythm != null)
                {
                    if (bpm < 0.1f)
                    {
                        bpm = 0.1f;
                    }

                    if (bpm > 1000)
                    {
                        bpm = 1000;
                    }

                    if (bpm != rhythm.BPM)
                    {

                        if (!player.IsPlaying)
                        {
                            if (changingBpm == false)
                            {
                                oldBpm = controller.Bpm;
                                changingBpm = true;
                            }


                            newBpm = bpm;
                            rhythm.BPM = bpm;

                            if (bpmRecorder == null)
                            {
                                bpmRecorder = new UndoRedo.DelayedRecordHolder {
                                    command = () => {
                                        changingBpm = false;
                                        return new Commands.EditRhythm.ChangeRhythmBpm(controller, newBpm, oldBpm);
                                    }
                                };
                            }
                            UndoRedo.DelayedRecord(bpmRecorder);
                        }
                        else
                        {
                            controller.PlayBpm = bpm;
                            player.Play();
                        }
                    }
                }

            }

            GUI.color = oldGuiColor;

        }
    }
}
