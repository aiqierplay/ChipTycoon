#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using UnityEditor;

namespace Aya.TweenPro
{
    [CustomEditor(typeof(UTweenAnimationPreset))]
    [CanEditMultipleObjects]
    public class UTweenAnimationPresetEditor : Editor
    {
        public virtual UTweenAnimationPreset Target => target as UTweenAnimationPreset;
        public UTweenAnimationPreset TweenAnimationPreset => Target;
        public TweenAnimation Animation => Target.Animation;
        public List<Tweener> TweenerList => Animation.TweenerList;

        [NonSerialized] public SerializedProperty TweenParamProperty;
        [NonSerialized] public SerializedProperty TweenerListProperty;

        public virtual void OnEnable()
        {
            InitEditor();
        }

        public virtual void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Animation.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        public virtual void InitEditor()
        {
            TweenParamProperty = serializedObject.FindProperty(nameof(Animation));
            TweenerListProperty = TweenParamProperty.FindPropertyRelative(nameof(Animation.TweenerList));

            Animation.InitEditor(TweenEditorMode.ScriptableObject, this);
        }
    }
}
#endif