﻿using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Per-Char Scale", "UGUI Text", 105)]
    [Serializable]
    public partial class TweenPerCharScale : TweenValueVector3<Text>, ITextCharacterModifier
    {
        public TextPerCharEffectData EffectData = new TextPerCharEffectData();
        public CharacterSpaceMode CharacterSpace;

        public Text GetTarget => Target;
        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Vector3 Value { get; set; }

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
            var scale = Vector3.LerpUnclamped(from, to, factor);

            if (CharacterSpace == CharacterSpaceMode.Character)
            {
                var center = Vector3.zero;
                for (var i = 0; i < vertices.Length; i++)
                {
                    center += vertices[i].position;
                }

                center /= vertices.Length;
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i].position.x = Mathf.LerpUnclamped(center.x, vertices[i].position.x, scale.x);
                    vertices[i].position.y = Mathf.LerpUnclamped(center.y, vertices[i].position.y, scale.y);
                    vertices[i].position.z = Mathf.LerpUnclamped(center.z, vertices[i].position.z, scale.z);
                }
            }
            else if (CharacterSpace == CharacterSpaceMode.Text)
            {
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i].position.x *= scale.x;
                    vertices[i].position.y *= scale.y;
                    vertices[i].position.z *= scale.z;
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
            EffectData.Reset();
            CharacterSpace = CharacterSpaceMode.Character;
        }
    }

#if UNITY_EDITOR

    public partial class TweenPerCharScale : TweenValueVector3<Text>, ITextCharacterModifier
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty EffectDataProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CharacterSpaceProperty;

        public override void DrawBody()
        {
            EffectData.DrawInspector();
        }

        public override void DrawAppend()
        {
            base.DrawAppend();
            GUIUtil.DrawToolbarEnum(CharacterSpaceProperty, nameof(Space), typeof(CharacterSpaceMode));
        }
    }

#endif

}