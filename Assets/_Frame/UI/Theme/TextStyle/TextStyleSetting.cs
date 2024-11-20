using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "TextStyleSetting", menuName = "UI/Text Style Setting")]
[Serializable]
public class TextStyleSetting : ThemeSetting<TextStyleSetting, TextStyle, TextStyleData, UITextStyleAdapter>
{

}