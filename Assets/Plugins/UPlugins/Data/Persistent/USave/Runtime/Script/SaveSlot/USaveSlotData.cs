using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Data.Persistent
{
    [Serializable]
    public class USaveSlotData
    {
        public string Key;
        public string Title;

        public int LaunchCount;
        public int SaveCount;
        public DateTime CreateTime = DateTime.Now;
        public DateTime LastSaveTime = DateTime.Now;
        public float RunTime = 0;

        public Texture2D Preview;

        public Dictionary<string, string> CustomData = new Dictionary<string, string>();

        [NonSerialized] public DateTime LoadTime;
    }
}