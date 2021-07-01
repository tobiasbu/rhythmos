using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Commands.RhythmsList
{
    using RhythmListView = ListView<Rhythm>;

    internal static class Commons
    {
        public static void AddRhythmAt(RhythmListView rhythmListView, Rhythm rhythm, int index)
        {
            IList<Rhythm> list = rhythmListView.List;
            int insertAt = Mathf.Clamp(index, 0, list.Count);

            if (list.Count > 0)
            {
                list.Insert(insertAt, rhythm);
            }
            else
            {
                list.Add(rhythm);
            }
            rhythmListView.Select(insertAt);
        }

        public static void RemoveRhythmAt(RhythmListView rhythmListView, Rhythm rhythm, int index)
        {
            IList<Rhythm> list = rhythmListView.List;
            list.Remove(rhythm);

            if (list.Count > 0)
            {
                int selectAt = Mathf.Clamp(index, 0, list.Count - 1);
                rhythmListView.Select(selectAt);
            }
            else
            {
                rhythmListView.UnSelect();
            }
        }

    }
}
