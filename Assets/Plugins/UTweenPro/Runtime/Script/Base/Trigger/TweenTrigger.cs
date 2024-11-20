using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public enum TriggerActionType
    {
        Play = 0,
        PlayBackward = 1,
        Pause = 2,
        Resume = 3,
        Stop = 4,
    }

    [Serializable]
    public abstract partial class TweenTrigger : MonoBehaviour
    {
        public UTweenPlayer Target;
        public TriggerActionType Action;

        public TweenAnimation Animation => Target?.Animation;

        public virtual void Awake()
        {
            
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnTrigger()
        {
            if (Target.Animation == null) return;
            switch (Action)
            {
                case TriggerActionType.Play:
                    Target.Animation.Play();
                    break;
                case TriggerActionType.PlayBackward:
                    Target.Animation.PlayBackward();
                    break;
                case TriggerActionType.Pause:
                    Target.Animation.Pause();
                    break;
                case TriggerActionType.Resume:
                    Target.Animation.Resume();
                    break;
                case TriggerActionType.Stop:
                    Target.Animation.Stop();
                    break;
            }
        }

        public virtual void Reset()
        {
            Target = GetComponentInChildren<UTweenPlayer>();
            Action = TriggerActionType.Play;
        }
    }

#if UNITY_EDITOR
    public abstract partial class TweenTrigger : MonoBehaviour
    {
        [NonSerialized] public SerializedObject SerializedObject;

        [TweenerProperty, NonSerialized] public SerializedProperty TargetProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty ActionProperty;

        public virtual void InitEditor()
        {
            TweenerPropertyAttribute.CacheProperty(this, SerializedObject);
        }

        public virtual void DrawTrigger()
        {
            using (GUILabelWidthArea.Create(EditorStyle.LabelWidth))
            {
                DrawInfo();
                DrawBody();
            }
        }

        public virtual void DrawInfo()
        {
            using (GUIHorizontal.Create())
            {
                EditorGUILayout.ObjectField(TargetProperty, new GUIContent(nameof(Target)), GUILayout.MinWidth(0));
                EditorGUILayout.PropertyField(ActionProperty);
            }
        }

        public virtual void DrawBody()
        {
        }
    }

    [CustomEditor(typeof(TweenTrigger))]
    [CanEditMultipleObjects]
    public abstract partial class TweenTriggerEditor<T> : Editor
        where T : TweenTrigger
    {
        public virtual T Target => target as T;

        public virtual void OnEnable()
        {
            Target.SerializedObject = serializedObject;
            Target.InitEditor();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Target.DrawTrigger();
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}