#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static bool EditMode = false;

        public static void DrawTextArea(SerializedProperty textProperty)
        {
            using (GUIGroup.Create())
            {
                if (EditMode)
                {
                    Input.imeCompositionMode = IMECompositionMode.On;
                    textProperty.stringValue = EditorGUILayout.TextArea(textProperty.stringValue);
                }
                else
                {
                    GUILayout.Label(textProperty.stringValue, EditorStyle.RichLabel);
                }
            }

            using (GUIHorizontal.Create())
            {
                var btnEdit = GUILayout.Button(EditMode ? "Save" : "Edit");
                if (btnEdit)
                {
                    EditMode = !EditMode;
                }

                using (GUIEnableArea.Create(!GUIUtility.systemCopyBuffer.Equals(textProperty.stringValue)))
                {
                    var btnCopy = GUILayout.Button("Copy");
                    if (btnCopy)
                    {
                        GUIUtility.systemCopyBuffer = textProperty.stringValue;
                    }
                }

                using (GUIEnableArea.Create(!GUIUtility.systemCopyBuffer.Equals(textProperty.stringValue)))
                {
                    var btnPaste = GUILayout.Button("Paste");
                    if (btnPaste)
                    {
                        textProperty.stringValue = GUIUtility.systemCopyBuffer;
                    }
                }

                using (GUIEnableArea.Create(!string.IsNullOrEmpty(textProperty.stringValue)))
                {
                    var btnClear = GUILayout.Button("Clear");
                    if (btnClear)
                    {
                        textProperty.stringValue = "";
                    }
                }
            }
        }
    }
}
#endif