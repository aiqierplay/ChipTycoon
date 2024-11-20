#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Per-Char Color", "TextMeshPro", UTweenTMP.IconPath, 101)]
    [Serializable]
    public partial class TweenTMPPerCharColor : TweenValueColor<TMP_Text>, ITMPCharacterModifier
    {
        public TMPPerCharEffectData EffectData = new TMPPerCharEffectData();
        public ColorOverlayMode Overlay;

        public TMP_Text GetTarget => Target;
        public bool ChangeGeometry => false;
        public bool ChangeColor => true;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Color Value { get; set; }

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
            var color = ColorMode == ColorMode.FromTo ? Color.Lerp(from, to, factor) : Gradient.Evaluate(factor);
            for (var i = startIndex; i < startIndex + 4; i++)
            {
                if (Overlay == ColorOverlayMode.Multiply)
                {
                    colors[i] *= color;
                }
                else if (Overlay == ColorOverlayMode.Add)
                {
                    colors[i] += color;
                }
                else if (Overlay == ColorOverlayMode.Minus)
                {
                    colors[i] -= color;
                }
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
            Overlay = ColorOverlayMode.Multiply;
            EffectData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenTMPPerCharColor : TweenValueColor<TMP_Text>, ITMPCharacterModifier
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty EffectDataProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty OverlayProperty;
        
        public override void DrawBody()
        {
            GUIUtil.DrawToolbarEnum(OverlayProperty, nameof(Overlay), typeof(ColorOverlayMode));
            EffectData.DrawInspector();
        }
    }

#endif

}
#endif