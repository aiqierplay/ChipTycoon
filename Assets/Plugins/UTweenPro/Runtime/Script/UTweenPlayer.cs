using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [ExecuteInEditMode]
    [AddComponentMenu("UTween Pro/UTween Player")]
    public partial class UTweenPlayer : MonoBehaviour
    {
        public TweenAnimation Animation = new TweenAnimation();

        public virtual void Awake()
        {
            Animation.TweenPlayer = this;
            Animation.ControlMode = TweenControlMode.Component;
            if (Application.isPlaying) Animation.Awake();
        }

        public virtual void OnEnable()
        {
            if (Application.isPlaying) Animation.OnEnable();
        }

        public virtual void Start()
        {
            if (Application.isPlaying) Animation.Start();
        }

        public virtual void OnDisable()
        {
            if (Application.isPlaying) Animation.OnDisable();
        }

        public virtual void Reset()
        {
            Animation.Reset();
        }

#if UNITY_EDITOR
        public virtual void OnDrawGizmosSelected()
        {
            Animation.OnDrawGizmos();
        }
#endif

        #region Animation Preset

        public void ApplyAsset(UTweenAnimationPreset preset)
        {
            Animation = UTweenAnimationPreset.ApplyPreset(gameObject, preset);
        }

        #endregion
    }

#if UNITY_EDITOR

    public partial class UTweenPlayer
    {
        internal Action RefreshEditorAction { get; set; } = delegate { };

        #region Editor Preview

        internal double LastTimeSinceStartup = -1f;

        internal void PreviewStart()
        {
            LastTimeSinceStartup = -1f;
            EditorApplication.update += EditorUpdate;
        }

        internal void PreviewEnd()
        {
            EditorApplication.update -= EditorUpdate;
        }

        internal void EditorUpdate()
        {
            var currentTime = EditorApplication.timeSinceStartup;
            if (LastTimeSinceStartup < 0f)
            {
                LastTimeSinceStartup = currentTime;
            }

            var deltaTime = (float)(currentTime - LastTimeSinceStartup);
            LastTimeSinceStartup = currentTime;

            Animation.Update(deltaTime);
        }

        #endregion

        #region Context Menu
       
        [ContextMenu("Fold Out All Tweener")]
        public void FoldOutAllTweener()
        {
            Undo.RegisterCompleteObjectUndo(this, "Fold Out All Tweener");
            foreach (var tweener in Animation.TweenerList)
            {
                tweener.FoldOut= true;
                tweener.SerializedObject.ApplyModifiedProperties();
            }
        }

        [ContextMenu("Fold In All Tweener")]
        public void FoldInTweener()
        {
            Undo.RegisterCompleteObjectUndo(this, "Fold In All Tweener");
            foreach (var tweener in Animation.TweenerList)
            {
                tweener.FoldOut = false;
                tweener.SerializedObject.ApplyModifiedProperties();
            }
        }

        [ContextMenu("Active All Tweener")]
        public void ActiveAllTweener()
        {
            Undo.RegisterCompleteObjectUndo(this, "Active All Tweener");
            foreach (var tweener in Animation.TweenerList)
            {
                tweener.Active = true;
                tweener.SerializedObject.ApplyModifiedProperties();
            }
        }

        [ContextMenu("DeActive All Tweener")]
        public void DeActiveAllTweener()
        {
            Undo.RegisterCompleteObjectUndo(this, "DeActive All Tweener");
            foreach (var tweener in Animation.TweenerList)
            {
                tweener.Active = false;
                tweener.SerializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }

#endif
}