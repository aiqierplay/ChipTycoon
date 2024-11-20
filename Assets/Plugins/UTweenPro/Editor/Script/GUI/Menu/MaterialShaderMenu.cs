#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectMaterialMenu(Renderer renderer, string propertyName, SerializedProperty materialIndexProperty)
        {
            if (renderer == null) return;
            using (GUIErrorColorArea.Create(materialIndexProperty.intValue < 0))
            {
                string displayName;
                var material = renderer.GetMaterial(materialIndexProperty.intValue);
                if (material == null)
                {
                    materialIndexProperty.intValue = -1;
                    displayName = EditorStyle.NoneStr;
                }
                else
                {
                    displayName = material.name;
                }

                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    var btn = GUILayout.Button(displayName, EditorStyles.popup);
                    if (btn)
                    {
                        var menu = CreateMaterialMenu(renderer, materialIndexProperty);
                        menu.ShowAsContext();
                    }
                }
            }
        }

        internal static GenericMenu CreateMaterialMenu(Renderer renderer, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, property.intValue < 0, () =>
            {
                property.intValue = -1;
                property.serializedObject.ApplyModifiedProperties();
            });

            if (renderer == null) return menu;

            menu.AddSeparator("");
            for (var i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                var material = renderer.sharedMaterials[i];
                var index = i;
                menu.AddItem(material.name, property.intValue == index, () =>
                {
                    property.intValue = index;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            return menu;
        }

        public static void SelectMaterialShaderMenu(Renderer renderer, string propertyName, int materialIndex,
            SerializedProperty propertyNameProperty,
            ShaderUtil.ShaderPropertyType propertyType,
            bool selectKeyword = false)
        {
            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(propertyNameProperty.stringValue)))
            {
                string displayName;
                var material = renderer.GetMaterial(materialIndex);
                if (material == null || string.IsNullOrEmpty(propertyNameProperty.stringValue) ||
                    (!material.shader.ContainsProperty(propertyNameProperty.stringValue) && !selectKeyword))
                {
                    propertyNameProperty.stringValue = "";
                    displayName = EditorStyle.NoneStr;
                }
                else
                {
                    displayName = propertyNameProperty.stringValue;
                }

                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    var btn = GUILayout.Button(displayName, EditorStyles.popup);
                    if (btn)
                    {
                        var menu = CreateMaterialPropertyMenu(renderer, materialIndex, propertyNameProperty, propertyType, selectKeyword);
                        menu.ShowAsContext();
                    }
                }
            }
        }

        internal static GenericMenu CreateMaterialPropertyMenu(Renderer renderer, int materialIndex
            , SerializedProperty property
            , ShaderUtil.ShaderPropertyType propertyType,
            bool selectKeyword)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(property.stringValue), () =>
            {
                property.stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
            });

            if (renderer == null) return menu;

            menu.AddSeparator("");
            var material = renderer.GetMaterial(materialIndex);
            if (material == null) return menu;

            var shader = material.shader;
            if (!selectKeyword)
            {
                var shaderCount = ShaderUtil.GetPropertyCount(shader);
                for (var i = 0; i < shaderCount; i++)
                {
                    var index = i;
                    var hidden = ShaderUtil.IsShaderPropertyHidden(shader, index);
                    var shaderPropertyType = ShaderUtil.GetPropertyType(shader, index);
                    if (propertyType == ShaderUtil.ShaderPropertyType.Float)
                    {
                        if (shaderPropertyType != ShaderUtil.ShaderPropertyType.Float && shaderPropertyType != ShaderUtil.ShaderPropertyType.Range) continue;
                    }
                    else if (shaderPropertyType != propertyType) continue;

                    var shaderPropertyName = ShaderUtil.GetPropertyName(shader, index);
                    var shaderPropertyDescription = ShaderUtil.GetPropertyDescription(shader, index);
                    if (shaderPropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        shaderPropertyName += "_ST";
                    }

                    var displayName = shaderPropertyName + "\t\t" + shaderPropertyDescription;
                    menu.AddItem(!hidden, displayName, property.stringValue == shaderPropertyName, () =>
                    {
                        property.stringValue = shaderPropertyName;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            else
            {
                var getKeywordsMethod = typeof(ShaderUtil).GetMethod("GetShaderGlobalKeywords", BindingFlags.Static | BindingFlags.NonPublic);
                var keywords = (string[])getKeywordsMethod.Invoke(null, new object[] { shader });
                for (var i = 0; i < keywords.Length; i++)
                {
                    var index = i;
                    var displayName = keywords[index];
                    menu.AddItem(displayName, property.stringValue == displayName, () =>
                    {
                        property.stringValue = displayName;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            return menu;
        }
    }
}
#endif