#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectAnimatorStateMenu(Animator animator, int layer, string propertyName, SerializedProperty stateProperty)
        {
            if (animator == null) return;
            if (layer < 0) return;
            var controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null) return;

            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(stateProperty.stringValue)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    string displayName;
                    if (string.IsNullOrEmpty(stateProperty.stringValue))
                    {
                        stateProperty.stringValue = null;
                        displayName = EditorStyle.NoneStr;
                    }
                    else
                    {
                        displayName = stateProperty.stringValue;
                    }

                    var button = GUILayout.Button(displayName, EditorStyles.popup);
                    if (button)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(stateProperty.stringValue), () =>
                        {
                            stateProperty.stringValue = null;
                            stateProperty.serializedObject.ApplyModifiedProperties();
                        });

                        if (controller != null)
                        {
                            var states = controller.layers[layer].stateMachine.states;
                            menu.AddSeparator("");
                            foreach (var state in states)
                            {
                                var stateName = state.state.name;
                                menu.AddItem(stateName, stateProperty.stringValue == stateName, () =>
                                {
                                    stateProperty.stringValue = stateName;
                                    stateProperty.serializedObject.ApplyModifiedProperties();
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