#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    [CustomEditor(typeof(UTweenManager))]
    public class UTweenManagerEditor : Editor
    {
        public virtual UTweenManager Target => target as UTweenManager;
        public UTweenManager TweenManager => Target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var btnMonitor = GUILayout.Button("Open Monitor");
            if (btnMonitor)
            {
                UTweenMonitorWindow.OpenWindow();
            }

            DrawPlayStats();
            DrawPoolStats();

            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        public void DrawPlayStats()
        {
            GUILayout.Label($"Play:{Target.PlayingList.Count}");
        }

        public void DrawPoolStats()
        {
            using (GUIVertical.Create())
            {
                foreach (var kv in UTweenPool.PoolListDic)
                {
                    var type = kv.Key;
                    var poolList = kv.Value;

                    GUILayout.Label(type.Name);
                    GUILayout.Label($"<color=green>{poolList.ActiveCount}</color>/<color=yellow>{poolList.DeActiveCount}</color>/<color=white>{poolList.Count}</color>", EditorStyle.RichLabel);
                }
            }
        }
    }
}
#endif