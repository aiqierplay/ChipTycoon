#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Aya.Reflection
{
    public abstract class SearchableDropdownDrawer<TValue> : PropertyDrawer
    {
        public SerializedProperty Property;

        public virtual string EmptyValue => "<None>";

        public abstract void CacheProperty(SerializedProperty property);
        public abstract TValue GetValue();
        public abstract void SetValue(TValue value);
        public abstract string GetDisplayName(TValue value);
        public abstract string GetRootName();
        public abstract IEnumerable<TValue> GetValueCollection();

        public virtual string GetDisplayNameInternal(TValue value)
        {
            if (IsValueValid(value)) return GetDisplayName(value);
            return EmptyValue;
        }

        public virtual bool IsValueValid(TValue value)
        {
            var isValid = Equals(value, default(TValue));
            return !isValid;
        }

        public virtual SearchableDropdownItem CreateDropdownMenu()
        {
            var dropdownMenu = new SearchableDropdownItem(GetRootName());
            dropdownMenu.AddChild(new SearchableDropdownItem(EmptyValue, null));
            dropdownMenu.AddSeparator();

            var valueCollection = GetValueCollection();
            if (valueCollection == null) return dropdownMenu;
            foreach (var value in valueCollection)
            {
                var dropdownItem = new SearchableDropdownItem(GetDisplayNameInternal(value), value);
                dropdownMenu.AddChild(dropdownItem);
            }

            return dropdownMenu;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Property = property;
            CacheProperty(property);

            var labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = position;
            labelRect.width = labelWidth;

            var filedRect = position;
            filedRect.x += labelWidth;
            filedRect.width -= labelWidth;

            ShowDropdownList(label.text, labelRect, filedRect);
        }

        protected virtual void ShowDropdownList(string propertyName, Rect labelRect, Rect fieldRect)
        {
            var root = CreateDropdownMenu();
            SearchableDropdownList(propertyName, labelRect, fieldRect, root);
        }

        protected virtual void SearchableDropdownList(string propertyName, Rect labelRect, Rect fieldRect, SearchableDropdownItem root)
        {
            var isValid = IsValueValid(GetValue());
            if (!isValid)
            {
                SetValue(default);
            }

            GUI.Label(labelRect, propertyName);
            var displayName = GetDisplayNameInternal(GetValue());
            var btnType = GUI.Button(fieldRect, displayName, EditorStyles.popup);
            if (btnType)
            {
                fieldRect.width = EditorGUIUtility.currentViewWidth;
                var dropdown = new SearchableDropdown(root, item =>
                {
                    if (item.Value is TValue value)
                    {
                        SetValue(value);
                    }
                    else
                    {
                        SetValue(default);
                    }

                    Property.serializedObject.ApplyModifiedProperties();
                });
                dropdown.Show(fieldRect, 500f);
            }
        }
    }
}
#endif