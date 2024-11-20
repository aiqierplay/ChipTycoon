#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIMenu
    {
        public static void SelectAnimationClipMenu(Animation animation, string propertyName, SerializedProperty clipProperty)
        {
            if (animation == null) return;
            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(clipProperty.stringValue)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    string displayName;
                    if (string.IsNullOrEmpty(clipProperty.stringValue))
                    {
                        clipProperty.stringValue = null;
                        displayName = EditorStyle.NoneStr;
                    }
                    else
                    {
                        displayName = clipProperty.stringValue;
                    }

                    var button = GUILayout.Button(displayName, EditorStyles.popup);
                    if (button)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(clipProperty.stringValue), () =>
                        {
                            clipProperty.stringValue = null;
                            clipProperty.serializedObject.ApplyModifiedProperties();
                        });

                        var clips = AnimationUtility.GetAnimationClips(animation.gameObject);
                        if (clips.Length > 0)
                        {
                            menu.AddSeparator("");
                            foreach (var clip in clips)
                            {
                                var clipName = clip.name;
                                menu.AddItem(clipName, clipProperty.stringValue == clipName, () =>
                                {
                                    clipProperty.stringValue = clipName;
                                    clipProperty.serializedObject.ApplyModifiedProperties();
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