#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Aya.Data.Json;
using UnityEngine;

namespace Aya.Data.Persistent
{
    [Serializable]
    [Searchable]
    [LabelWidth(120)]
    public class USaveMainDataEditor : SerializedScriptableObject
    {
        [NonSerialized] public USaveMainData MainData;

        public List<USaveSlotData> SlotList;

        [TextArea(20, 40)]
        [HideLabel]
        [FoldoutGroup("Main Data Json")]
        public string MainDataJson;

        public void Init(USaveMainData mainData)
        {
            MainData = mainData;
            SlotList = mainData.SlotList;
            MainDataJson = JsonUtil.ToJson(MainData, true);
        }
    }
}
#endif