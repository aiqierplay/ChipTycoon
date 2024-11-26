#if UNITY_EDITOR
using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Aya.TweenPro
{
    public class SearchableDropdown : AdvancedDropdown
    {
        public SearchableDropdownItem Root;
        public Action<SearchableDropdownItem> OnSelected;

        public SearchableDropdown(SearchableDropdownItem root, Action<SearchableDropdownItem> onSelected = null) : base(new AdvancedDropdownState())
        {
            Root = root;
            OnSelected = onSelected;
            minimumSize = new Vector2(300, 200);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            return Root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            OnSelected?.Invoke(item as SearchableDropdownItem);
        }
    }
}
#endif