using UnityEngine;

namespace Aya.TweenPro
{
    public partial class TweenMaterialKeyword : TweenValueBoolean<Material>
    {
        public string Keyword;

        public override bool Value
        {
            get => Target.IsKeywordEnabled(Keyword);
            set
            {
                if (value)
                {
                    Target.EnableKeyword(Keyword);
                }
                else
                {
                    Target.DisableKeyword(Keyword);
                }
            }
        }
    }

    #region Extension

    public partial class TweenMaterialKeyword : TweenValueBoolean<Material>
    {
        public TweenMaterialKeyword SetKeyword(string keyword)
        {
            Keyword = keyword;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialKeyword Keyword(Material material, string keyword, bool to, float duration)
        {
            var tweener = Play<TweenMaterialKeyword, Material, bool>(material, to, duration)
                .SetKeyword(keyword);
            return tweener;
        }

        public static TweenMaterialKeyword Keyword(Material material, string keyword, bool from, bool to, float duration)
        {
            var tweener = Play<TweenMaterialKeyword, Material, bool>(material, from, to, duration)
                .SetKeyword(keyword);
            return tweener;
        }
    }

    public static partial class MaterialExtension
    {
        public static TweenMaterialKeyword TweenKeyword(this Material material, string keyword, bool to, float duration)
        {
            var tweener = UTween.Keyword(material, keyword, to, duration);
            return tweener;
        }

        public static TweenMaterialKeyword TweenKeyword(this Material material, string keyword, bool from, bool to, float duration)
        {
            var tweener = UTween.Keyword(material, keyword, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
