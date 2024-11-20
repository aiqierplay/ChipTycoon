using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class TextStyle : Theme<TextStyleSetting, TextStyle, TextStyleData, UITextStyleAdapter>
{

}