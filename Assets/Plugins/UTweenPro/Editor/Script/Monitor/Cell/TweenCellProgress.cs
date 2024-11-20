#if UNITY_EDITOR
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenCellProgress : GUITableCell<TweenRowData, Tweener, float>
    {
        public override float GetValue()
        {
            return Data.Animation.NormalizedProgress;
        }

        public override void DrawValue()
        {
            if (!RowData.Active) return;
            using (GUIVertical.Create())
            {
                GUILayout.Space(2);
                GUIUtil.DrawFromToDraggableProgressBar(null, RowData.GetRowHeight() - 4,
                    Data.DurationFromNormalized, Data.DurationToNormalized, Data.Animation.NormalizedProgress,
                    Data.HoldStart, Data.HoldEnd,
                    (from, to) =>
                    {
                    });
            
                GUILayout.Space(2);
            }
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            return 0;
        }
    }
}
#endif