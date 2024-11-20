using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Per-Char Color", "UGUI Text", 101)]
    [Serializable]
    public partial class TweenPerCharColor : TweenValueColor<Text>, ITextCharacterModifier
    {
        public TextPerCharEffectData EffectData = new TextPerCharEffectData();
        public ColorOverlayMode Overlay;

        public Text GetTarget => Target;
        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Color Value { get; set; }

        public override void PrepareSample()
        {
            base.PrepareSample();
            EffectData.Cache(((Tweener)this).Animation, Target, this);
        }

        public override void Sample(float factor)
        {
        }

        public void Modify(int characterIndex, ref UIVertex[] vertices)
        {
            var (index, progress) = EffectData.GetIndexAndProgress(characterIndex);
            var from = EnableFromGetter ? From.ValueGetter() : FromValue;
            var to = EnableToGetter ? To.ValueGetter() : ToValue;
            var factor = EffectData.GetFactor(progress, Factor);
            var color = ColorMode == ColorMode.FromTo ? Color.Lerp(from, to, factor) : Gradient.Evaluate(factor);
            for (var i = 0; i < vertices.Length; i++)
            {
                if (Overlay == ColorOverlayMode.Multiply)
                {
                    vertices[i].color *= color;
                }
                else if (Overlay == ColorOverlayMode.Add)
                {
                    vertices[i].color += color;
                }
                else if (Overlay == ColorOverlayMode.Minus)
                {
                    vertices[i].color -= color;
                }
            }
        }

        public override void SetDirty()
        {
            base.SetDirty();
            EffectData.SetDirty();
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

    public partial class TweenPerCharColor : TweenValueColor<Text>, ITextCharacterModifier
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty EffectDataProperty;

        public override void DrawBody()
        {
            EffectData.DrawInspector();
        }
    }

#endif

}