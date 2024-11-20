using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// https://docs.unity3d.com/Manual/shader-keywords.html

namespace Aya.TweenPro
{
    [Tweener("Keyword", nameof(Material), "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyKeyword : TweenValueBoolean<Renderer>
    {
        public TweenMaterialData MaterialData = new TweenMaterialData();

        public override bool Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetKeyword();
            }
            set
            {
                MaterialData.Cache(Target);
                MaterialData.SetKeyword(value);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyKeyword : TweenValueBoolean<Renderer>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty MaterialDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            MaterialData.Tweener = this;
            MaterialData.SelectKeyword = true;
            base.InitEditor(index, animation, tweenerProperty);
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            MaterialData.DrawInspector();
        }
    }

#endif

    #region Extension
    
    public partial class TweenMaterialPropertyKeyword : TweenValueBoolean<Renderer>
    {
        public TweenMaterialPropertyKeyword SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }
    
        public TweenMaterialPropertyKeyword SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }
    
        public TweenMaterialPropertyKeyword SetKeyword(string keyword)
        {
            MaterialData.Property = keyword;
            return this;
        }
    }
    
    public static partial class UTween
    {
        public static TweenMaterialPropertyKeyword Keyword(Renderer renderer, string keyword, bool to, float duration)
        {
            var tweener = Play<TweenMaterialPropertyKeyword, Renderer, bool>(renderer, to, duration)
                .SetMaterialIndex(0)
                .SetKeyword(keyword);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword Keyword(Renderer renderer, int materialIndex, string keyword, bool to, float duration)
        {
            var tweener = Play<TweenMaterialPropertyKeyword, Renderer, bool>(renderer, to, duration)
                .SetMaterialIndex(materialIndex)
                .SetKeyword(keyword);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword Keyword(Renderer renderer, string keyword, bool from, bool to, float duration)
        {
            var tweener = Play<TweenMaterialPropertyKeyword, Renderer, bool>(renderer, from, to, duration)
                .SetMaterialIndex(0)
                .SetKeyword(keyword);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword Keyword(Renderer renderer, int materialIndex, string keyword, bool from, bool to, float duration)
        {
            var tweener = Play<TweenMaterialPropertyKeyword, Renderer, bool>(renderer, from, to, duration)
                .SetMaterialIndex(materialIndex)
                .SetKeyword(keyword);
            return tweener;
        }
    }
    
    public static partial class RendererExtension
    {
        public static TweenMaterialPropertyKeyword TweenKeyword(this Renderer renderer, string keyword, bool to, float duration)
        {
            var tweener = UTween.Keyword(renderer, keyword, to, duration);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword TweenKeyword(this Renderer renderer, int materialIndex, string keyword, bool to, float duration)
        {
            var tweener = UTween.Keyword(renderer, materialIndex, keyword, to, duration);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword TweenKeyword(this Renderer renderer, string keyword, bool from, bool to, float duration)
        {
            var tweener = UTween.Keyword(renderer, keyword, from, to, duration);
            return tweener;
        }
    
        public static TweenMaterialPropertyKeyword TweenKeyword(this Renderer renderer, int materialIndex, string keyword, bool from, bool to, float duration)
        {
            var tweener = UTween.Keyword(renderer, materialIndex, keyword, from, to, duration);
            return tweener;
        }
    }
    
    #endregion
}
