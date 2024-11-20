using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.UI.Scroll
{
    public abstract class ScrollViewBase : ScrollBase
    {
        public ScrollContent Content;
        public RectOffset Padding;
        public Vector2 Spacing;

        public List<ScrollItem> ItemList;

        public override void Calculate()
        {
            base.Calculate();
            if (Content != null) Content.Calculate();

           
        }
    }
}