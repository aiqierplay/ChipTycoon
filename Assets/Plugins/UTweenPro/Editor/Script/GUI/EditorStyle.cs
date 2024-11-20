#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static class EditorStyle
    {
        #region String

        public const string NoneStr = "<None>";

        #endregion

        #region Param

        // public static PropertyInfo ViewWidthProperty = typeof(EditorGUIUtility).GetProperty("contextWidth", BindingFlags.NonPublic | BindingFlags.Static);
        // public static float ViewWidth => (float)ViewWidthProperty.GetValue(null, null);
        public static float ViewWidth => EditorGUIUtility.currentViewWidth;

        public static float LabelWidth => 60f;
        // public static float FieldWidth => HalfWidth - LabelWidth;
        public static float HalfWidth => (ViewWidth - 42f) / 2f;

        public static float CharacterWidth = GUI.skin.label.CalcSize(new GUIContent("X")).x;
        public static float SingleButtonWidth = EditorGUIUtility.singleLineHeight;
        // public static float SettingButtonWidth = 70f;
        public static float MinWidth => CharacterWidth;

        public static float TweenerHeaderIconSize => SingleButtonWidth * 0.9f;
        // public static float HoldButtonWidth => SingleButtonWidth * 0.9f;

        #endregion

        #region GUIStyle

        public static GUIStyle RichLabel
        {
            get
            {
                if (_richLabel == null)
                {
                    _richLabel = new GUIStyle(EditorStyles.label)
                    {
                        richText = true
                    };
                }

                return _richLabel;
            }
        }

        private static GUIStyle _richLabel;

        public static GUIStyle MultiLineLabel
        {
            get
            {
                if (_multiLineLabel == null)
                {
                    _multiLineLabel = new GUIStyle(EditorStyles.label)
                    {
                        wordWrap = true
                    };
                }

                return _multiLineLabel;
            }
        }

        private static GUIStyle _multiLineLabel;

        public static GUIStyle TitleTargetLabel
        {
            get
            {
                if (_titleTargetLabel == null)
                {
                    _titleTargetLabel = new GUIStyle(EditorStyles.label)
                    {
                        stretchWidth = true,
                        wordWrap = false
                    };
                }
        
                return _titleTargetLabel;
            }
        }
        
        private static GUIStyle _titleTargetLabel;

        public static GUIStyle ProgressHoldButton
        {
            get
            {
                if (_progressHoldButton == null)
                {
                    _progressHoldButton = new GUIStyle(EditorStyles.miniButton);
                }

                return _progressHoldButton;
            }
        }

        private static GUIStyle _progressHoldButton;


        public static GUIStyle ListSelectionButton
        {
            get
            {
                if (_listSelectionButton == null)
                {
                    _listSelectionButton = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        alignment = TextAnchor.MiddleLeft
                    };
                }

                return _listSelectionButton;
            }
        }

        private static GUIStyle _listSelectionButton;

        #endregion

        #region GUIContent

        public static GUIContent PlayButton = new GUIContent(EditorIcon.GetIcon("PlayButton"));
        public static GUIContent PlayButtonOn = new GUIContent(EditorIcon.GetIcon("PlayButton On"));

        #endregion

        #region Texture2D

        public static Texture2D CreateTexture2D(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        #endregion
    }
}
#endif