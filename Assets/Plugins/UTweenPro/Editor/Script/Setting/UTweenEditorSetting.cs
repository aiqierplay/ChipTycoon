#if UNITY_EDITOR
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    [CreateAssetMenu(fileName = "UTweenEditorSetting", menuName = "UTween Pro/UTween Editor Setting")]
    public class UTweenEditorSetting : UTweenEditorSettingBase<UTweenEditorSetting>
    {
        [Header("Color")]

        public Color EnableColor;
        public Color DisableColor;
        public Color ErrorColor;
        public Color SelectedColor;

        public Color ProgressColor;
        public Color ProgressBackColor;
        public Color SubProgressColor;
        public Color SubProgressHoldColor;
        public Color ProgressDisableColor;

        public Color SpecialValueReminderColor;

        [Header("Inspector")]
        public bool ShowGroupReminder;
        public bool ShowTargetTypeInInspector;
        public bool ShowTargetTypeInCreateMenu;
        public float GroupReminderWidth;
        public bool HideFullSubProgress;

        [Header("Documentation")]
        public Color DocumentationColor;
        public bool ShowDocumentation;

        [Header("Group")]
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3
        [TableList]
#endif
        public List<TweenGroupEditorData> GroupDataList;

        public Dictionary<string, TweenGroupEditorData> GroupDataDic { get; set; }

        public override void Init()
        {
            base.Init();

            GroupDataDic = new Dictionary<string, TweenGroupEditorData>();
            foreach (var data in GroupDataList)
            {
                GroupDataDic.Add(data.Name, data);
            }
        }

        public TweenGroupEditorData GetGroupData(string groupName)
        {
            if (!GroupDataDic.TryGetValue(groupName, out var groupData))
            {
                var tempData = new TweenGroupEditorData()
                {
                    Name = groupName,
                    Color = Color.white,
                    IconPath = null
                };

                return tempData;
            }

            return groupData;
        }
    }
}
#endif