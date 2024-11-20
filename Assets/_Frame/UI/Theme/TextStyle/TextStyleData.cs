using Sirenix.OdinInspector;
using System;
using TMPro;

[Serializable]
public class TextStyleData : ThemeStyleData
{
    public TMP_FontAsset Font;
    public float FontSize = 32;
    public FontStyles FontStyles;
}