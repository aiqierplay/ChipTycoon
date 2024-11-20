#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using Aya.Data.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aya.Data.Persistent
{
    [Serializable]
    [Searchable]
    [LabelWidth(120)]
    public class USaveSlotDataEditor : SerializedScriptableObject
    {
        [BoxGroup]
        public USaveSlotData SlotData;

        [TextArea(10, 25)]
        [HideLabel]
        [FoldoutGroup("Slot Data Json")]
        public string SlotDataJson;

        [TableList]
        [Searchable]
        public List<USaveKeyValueEditor> KeyValueList;

        public void Init(USaveSlotData slotData)
        {
            SlotData = slotData;
            SlotDataJson = JsonUtil.ToJson(SlotData, true);
            KeyValueList = new List<USaveKeyValueEditor>();

            var keyList = USave.GetAllKeys(SlotData.Key).ToList();
            USave.SelectSlot(SlotData.Key);

            foreach (var key in keyList)
            {
                var value = USave.GetValue<string>(key);
                var keyValue = new USaveKeyValueEditor();
                keyValue.Init(key, value);
                KeyValueList.Add(keyValue);
            }
        }
    }
}
#endif