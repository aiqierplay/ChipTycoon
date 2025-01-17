#if UNITY_EDITOR
using UnityEditor.IMGUI.Controls;

namespace Aya.TweenPro
{
    public class SearchableDropdownItem : AdvancedDropdownItem
    {
        public object Value;

        public SearchableDropdownItem(string name, object value = null) : base(name)
        {
            Value = value;
        }
    }
}
#endif