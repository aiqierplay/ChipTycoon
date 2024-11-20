#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Per-Char Alpha", "TextMeshPro", UTweenTMP.IconPath, 100)]
    [Serializable]
    public partial class TweenTMPPerCharAlpha : TweenValueFloat<TMP_Text>, ITMPCharacterModifier
    {
        public TMPPerCharEffectData EffectData = new TMPPerCharEffectData();

        public TMP_Text GetTarget => Target;
        public bool ChangeGeometry => false;
        public bool ChangeColor => true;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override float Value { get; set; }

        public override void PrepareSample()
        {
            base.PrepareSample();
            EffectData.Cache(Animation, Target, this);
        }

        public override void Sample(float factor)
        {
        }

        public void ModifyGeometry(int characterIndex, ref Vector3[] vertices, float progress)
        {
        }

        public void ModifyColor(int characterIndex, ref Color32[] colors, float progress)
        {
            var startIndex = EffectData.GetStartIndex(characterIndex) * 4;
            var from = EnableFromGetter ? From.ValueGetter() : FromValue;
            var to = EnableToGetter ? To.ValueGetter() : ToValue;
            var factor = EffectData.GetFactor(progress, Factor);
            var alpha = Mathf.LerpUnclamped(from, to, factor);
            for (var i = startIndex; i < startIndex + 4; i++)
            {
                Color color = colors[i];
                color.a = alpha;
                colors[i] = color;
            }
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            EffectData.Remove(((Tweener)this).Animation, Target, this);
        }

        public override void Reset()
        {
            base.Reset();
            EffectData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenTMPPerCharAlpha : TweenValueFloat<TMP_Text>, ITMPCharacterModifier
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty EffectDataProperty;

        public override void DrawBody()
        {
            EffectData.DrawInspector();
        }
    }

#endif

}
#endif