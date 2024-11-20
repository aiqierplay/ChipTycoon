#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector;

namespace Aya.Data.Persistent
{
    [Serializable]
    [LabelWidth(120)]
    public class USaveKeyValueEditor
    {
        [ReadOnly]
        [TableColumnWidth(100)]
        public string Key;

        [TableColumnWidth(400)]
        public string Value;

        public void Init(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
#endif