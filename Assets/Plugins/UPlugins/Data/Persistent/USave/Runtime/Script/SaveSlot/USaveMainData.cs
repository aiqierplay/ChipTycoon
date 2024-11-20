using System;
using System.Collections.Generic;
using System.Linq;

namespace Aya.Data.Persistent
{
    [Serializable]
    public class USaveMainData
    {
        public int LaunchCount;
        public int SaveCount;
        public DateTime CreateTime = DateTime.Now;
        public DateTime LastSaveTime = DateTime.Now;
        public float RunTime = 0;

        public List<USaveSlotData> SlotList = new List<USaveSlotData>();

        public Dictionary<string, string> CustomData = new Dictionary<string, string>();

        [NonSerialized] public Dictionary<string, USaveSlotData> SlotDic = new Dictionary<string, USaveSlotData>();
        [NonSerialized] public DateTime LoadTime;

        public void Init()
        {
            SlotDic = SlotList.ToDictionary(slot => slot.Key);
        }

        public void Add(USaveSlotData slotData)
        {
            SlotList.Add(slotData);
            SlotDic.Add(slotData.Key, slotData);
        }

        public bool Exist(string key)
        {
            return SlotDic.ContainsKey(key);
        }

        public void Remove(string key)
        {
            SlotDic.TryGetValue(key, out var slotData);
            SlotDic.Remove(key);
            SlotList.Remove(slotData);
        }
    }
}