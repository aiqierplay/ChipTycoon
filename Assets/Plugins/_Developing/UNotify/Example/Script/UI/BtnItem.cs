using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aya.Sample.Notify
{
    public class BtnItem : BtnWithRedPoint
    {
        public ItemData ItemData;

        public void Init(int key)
        {
            ItemData = ItemDataManager.Get(key);
        }

        public string GetItemKey()
        {
            return ItemData.RedPointKey;
        }
    }
}