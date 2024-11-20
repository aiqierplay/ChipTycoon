using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnvironmentSetting", menuName = "Setting/Level Environment Setting")]
public class LevelEnvironmentSetting : SettingBase<LevelEnvironmentSetting>
{
    [TableList]
    public List<LevelEnvironmentData> DataList;

    public LevelEnvironmentData CurrentLevelEnvironment
    {
        get
        {
            if (DataList.Count == 0) return default;
            var index = SaveManager.Ins.LevelIndex.Value;
            if (index >= DataList.Count) index = DataList.Count - 1;
            return DataList[index];
        }
    }
}
