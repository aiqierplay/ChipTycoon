#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    [CustomEditor(typeof(UTweenPlayer))]
    [CanEditMultipleObjects]
    public class UTweenPlayerEditor : Editor
    {
        public virtual UTweenPlayer Target => target as UTweenPlayer;
        public UTweenPlayer TweenPlayer => Target;
        public TweenAnimation Animation => Target.Animation;
        public List<Tweener> TweenerList => Animation.TweenerList;

        [NonSerialized] public SerializedProperty TweenParamProperty;
        [NonSerialized] public SerializedProperty TweenerListProperty;

        public virtual void OnEnable()
        {
            if (Target == null) return;
            InitEditor();
            Target.RefreshEditorAction = InitEditor;
            Animation.RecordObject();
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnDestroy()
        {
            if (Animation.IsSubAnimation) return;
            if (Animation.PreviewSampled)
            {
                if (Animation.IsPlaying) Animation.Stop();
                Animation.RestoreObject();
            }

            if (!Application.isPlaying && UTweenManager.Ins != null)
            {
                DestroyImmediate(UTweenManager.Ins.gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Animation.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();

            if (Animation.IsInProgress)
            {
                Repaint();
            }
        }

        public void OnSceneGUI()
        {
            Animation.OnSceneGUI();
        }

        public virtual void InitEditor()
        {
            Animation.EditorNormalizedProgress = 0f;
            Animation.TweenPlayer = Target;

            TweenParamProperty = serializedObject.FindProperty(nameof(Animation));
            TweenerListProperty = TweenParamProperty.FindPropertyRelative(nameof(Animation.TweenerList));

            Animation.InitEditor(TweenEditorMode.Component, this);
        }
    }
}
#endif