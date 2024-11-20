using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ColorPaletteSetting", menuName = "UI/Color Palette Setting")]
[Serializable]
public class ColorPaletteSetting : ThemeSetting<ColorPaletteSetting, ColorPalette, ColorPaletteData, UIColorPaletteAdapter>
{
}