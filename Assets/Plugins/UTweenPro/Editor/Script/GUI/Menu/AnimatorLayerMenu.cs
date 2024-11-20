#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectAnimatorLayerMenu(Animator animator, string propertyName, SerializedProperty layerProperty)
        {
            if (animator == null) return;
            var controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null) return;
            var layerCount = controller.layers.Length;
            using (GUIErrorColorArea.Create(layerProperty.intValue < 0))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    string displayName;
                    if (layerProperty.intValue < 0)
                    {
                        displayName = EditorStyle.NoneStr;
                    }
                    else if (layerProperty.intValue >= layerCount)
                    {
                        layerProperty.intValue = -1;
                        displayName = EditorStyle.NoneStr;
                    }
                    else
                    {
                        displayName = controller.layers[layerProperty.intValue].name;
                    }

                    var button = GUILayout.Button(displayName, EditorStyles.popup);
                    if (button)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, layerProperty.intValue == -1, () =>
                        {
                            layerProperty.intValue = -1;
                            layerProperty.serializedObject.ApplyModifiedProperties();
                        });

                        var layers = controller.layers;
                        if (layers.Length > 0)
                        {
                            menu.AddSeparator("");
                            for (var i = 0; i < layers.Length; i++)
                            {
                                var index = i;
                                var layerName = layers[index].name;
                                menu.AddItem(layerName, layerProperty.intValue == index, () =>
                                {
                                    layerProperty.intValue = index;
                                    layerProperty.serializedObject.ApplyModifiedProperties();
                                });
                            }
                        }

                        menu.ShowAsContext();
                    }
                }
            }
        }
    }
}
#endif