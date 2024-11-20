#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities;

public class EditorToolkit : OdinMenuEditorWindow
{
    public const string Title = "Editor Toolkit";
    public const int ToolkitMenuSize = 220;

    [MenuItem("UPlugins/" + Title)]
    private static void OpenWindow()
    {
        var window = GetWindow<EditorToolkit>();
        var screenRect = EditorGUIUtility.GetMainWindowPosition();
        window.position = screenRect.AlignCenter(1280, 720);
    }

    protected override void Initialize()
    {
        MenuWidth = ToolkitMenuSize;
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(false)
        {
            Config =
            {
                DrawSearchToolbar = true,
            },
            DefaultMenuStyle = ReturnOdinMenuStyle()
        };

        titleContent = new GUIContent(Title);

        #region Toolbox  List

        tree.Add("Menu Style", tree.DefaultMenuStyle, SdfIconType.MenuButtonWideFill);
        var toolTypeList = TypeCache.GetTypesWithAttribute<EditorToolAttribute>();
        foreach (var toolType in toolTypeList)
        {
            var toolInstance = CreateInstance(toolType);

            var getTitleMethod = toolType.GetMethod(nameof(EditorToolBase.GetTitle));
            if (getTitleMethod == null) continue;
            var toolTitle = getTitleMethod.Invoke(toolInstance, null).ToString();

            var getIconMethod = toolType.GetMethod(nameof(EditorToolBase.GetIcon));
            if (getIconMethod == null) continue;
            var toolIcon = (SdfIconType)getIconMethod.Invoke(toolInstance, null);

            tree.Add(toolTitle, toolInstance, toolIcon);
        }

        #endregion

        return tree;
    }

    public OdinMenuStyle ReturnOdinMenuStyle()
    {
        var odinMenuStyle = new OdinMenuStyle()
        {
            Height = 32,
            Offset = 18,
            IndentAmount = 15.00f,
            IconSize = 24f,
            IconOffset = -5f,
            NotSelectedIconAlpha = 0.6f,
            IconPadding = 0.00f,
            TriangleSize = 16.00f,
            TrianglePadding = 0.00f,
            AlignTriangleLeft = true,
            Borders = true,
            BorderPadding = 0.00f,
            BorderAlpha = 0.32f,
            SelectedColorDarkSkin = new Color(0.243f, 0.373f, 0.588f, 1.000f),
            SelectedColorLightSkin = new Color(0.243f, 0.490f, 0.900f, 1.000f)
        };

        return odinMenuStyle;
    }
}
#endif