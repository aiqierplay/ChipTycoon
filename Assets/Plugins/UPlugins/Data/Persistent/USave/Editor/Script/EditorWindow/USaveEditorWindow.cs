#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Aya.Data.Persistent
{
    public class USaveEditorWindow : OdinMenuEditorWindow
    {
        public const string Title = "USave";
        public const int ToolkitMenuSize = 220;

        [MenuItem("Tools/Aya Game/USave/Open USave Editor Window", false, 0)]
        public static void OpenWindow()
        {
            var window = GetWindow<USaveEditorWindow>();
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

            tree.Add("Setting", USaveSetting.Ins, SdfIconType.Gear);

            USave.LoadMainData();
            var mainData = USave.MainData;
            var mainDataEditor = CreateInstance<USaveMainDataEditor>();
            mainDataEditor.Init(mainData);
            tree.Add("Main Data", mainDataEditor, SdfIconType.HouseDoor);

            foreach (var slotData in USave.SlotList)
            {
                var slotDataEditor = CreateInstance<USaveSlotDataEditor>();
                slotDataEditor.Init(slotData);
                tree.Add(slotData.Key, slotDataEditor, SdfIconType.DeviceHdd);
            }

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
}
#endif