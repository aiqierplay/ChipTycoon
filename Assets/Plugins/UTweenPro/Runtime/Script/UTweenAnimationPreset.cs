using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [CreateAssetMenu(menuName = "UTween Pro/UTween Animation Preset", fileName = "UTweenAnimationPreset")]
    public partial class UTweenAnimationPreset : ScriptableObject
    {
        public TweenAnimation Animation = new TweenAnimation();

        public void OnEnable()
        {
#if UNITY_EDITOR
            if (!AssetDatabase.Contains(this)) return;
            var icon = UTweenEditorSetting.Ins.IconPreset;
            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(this, icon);
            }
#endif
        }

        public virtual void Reset()
        {
            Animation.Reset();
        }

        public static TweenAnimation ApplyPreset(GameObject target, UTweenAnimationPreset preset)
        {
            if (preset == null) return null;
            var assetInstance = Instantiate(preset);
            var animation = assetInstance.Animation;
            for (var i = animation.TweenerList.Count - 1; i >= 0; i--)
            {
                var tweener = animation.TweenerList[i];
                var tweenerType = tweener.GetType();
                var tweenerTargetField = tweenerType.GetField("Target");
                if (tweenerTargetField == null)
                {
                    animation.TweenerList.Remove(tweener);
                    continue;
                }

                var targetComponent = target.GetComponentInChildren(tweenerTargetField.FieldType);
                if (targetComponent == null)
                {
                    animation.TweenerList.Remove(tweener);
                    continue;
                }

                tweenerTargetField.SetValue(tweener, targetComponent);
            }

            if (Application.isPlaying) Destroy(assetInstance);
            else DestroyImmediate(assetInstance);

            return animation;
        }
    }

#if UNITY_EDITOR

    public partial class UTweenAnimationPreset
    {
        [ContextMenu("Fold Out All Tweener")]
        public void FoldOutAllTweener()
        {
            Undo.RegisterCompleteObjectUndo(this, "Fold Out All Tweener");
            foreach (var tweener in Animation.TweenerList)
            {
                tweener.FoldOut = true;
                tweener.SerializedObject.ApplyModifiedProperties();
            }
        }

        [ContextMenu("Fold In All Tweener")]
        public void FoldInAllTweener()
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
    }
#endif
}
