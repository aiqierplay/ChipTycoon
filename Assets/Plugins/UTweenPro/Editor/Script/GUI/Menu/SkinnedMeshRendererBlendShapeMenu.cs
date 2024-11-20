#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectBlendShapeMenu(SkinnedMeshRenderer renderer, string propertyName, SerializedProperty blendShapeIndexProperty)
        {
            if (renderer == null) return;
            using (GUIErrorColorArea.Create(blendShapeIndexProperty.intValue < 0))
            {
                string displayName;
                if (blendShapeIndexProperty.intValue >= renderer.sharedMesh.blendShapeCount)
                {
                    blendShapeIndexProperty.intValue = -1;
                    displayName = EditorStyle.NoneStr;
                }
                else if (blendShapeIndexProperty.intValue < 0)
                {
                    displayName = EditorStyle.NoneStr;
                }
                else
                {
                    displayName = renderer.sharedMesh.GetBlendShapeName(blendShapeIndexProperty.intValue);
                }

                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    var btn = GUILayout.Button(displayName, EditorStyles.popup);
                    if (btn)
                    {
                        var menu = CreateBlendShapeMenu(renderer, blendShapeIndexProperty);
                        menu.ShowAsContext();
                    }
                }
            }
        }

        internal static GenericMenu CreateBlendShapeMenu(SkinnedMeshRenderer renderer, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, property.intValue < 0, () =>
            {
                property.intValue = -1;
                property.serializedObject.ApplyModifiedProperties();
            });

            if (renderer == null) return menu;

            menu.AddSeparator("");
            for (var i = 0; i < renderer.sharedMesh.blendShapeCount; i++)
            {
                var blendShapeName = renderer.sharedMesh.GetBlendShapeName(i);
                var index = i;
                menu.AddItem(blendShapeName, property.intValue == index, () =>
                {
                    property.intValue = index;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            return menu;
        }
    }
}
#endif