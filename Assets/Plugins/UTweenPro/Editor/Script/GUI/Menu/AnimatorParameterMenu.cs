#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectAnimatorParameterMenu(Animator animator, string propertyName, SerializedProperty parameterProperty, AnimatorControllerParameterType parameterType)
        {
            if (animator == null) return;
            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(parameterProperty.stringValue)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    string displayName;
                    if (string.IsNullOrEmpty(parameterProperty.stringValue))
                    {
                        parameterProperty.stringValue = null;
                        displayName = EditorStyle.NoneStr;
                    }
                    else
                    {
                        displayName = parameterProperty.stringValue;
                    }

                    var button = GUILayout.Button(displayName, EditorStyles.popup);
                    if (button)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(parameterProperty.stringValue), () =>
                        {
                            parameterProperty.stringValue = null;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        });

                        if (animator.parameterCount > 0)
                        {
                            menu.AddSeparator("");
                            for (var i = 0; i < animator.parameterCount; i++)
                            {
                                var index = i;
                                var parameter = animator.GetParameter(index);
                                if (parameter.type != parameterType) continue;
                                menu.AddItem(parameter.name, parameterProperty.stringValue == parameter.name, () =>
                                {
                                    parameterProperty.stringValue = parameter.name;
                                    parameterProperty.serializedObject.ApplyModifiedProperties();
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