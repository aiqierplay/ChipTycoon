using System.Collections;
using System.Collections.Generic;
using Aya.Notify;
using UnityEngine;

namespace Aya.Sample.Notify
{
    public class ItemRedPoint : RedPoint
    {
        public BtnItem Item;
        public ItemData ItemData => Item.ItemData;

        protected override void Awake()
        {
            Key = ItemData.RedPointKey;
            Node = ItemData.NotifyNode;
        }
    }
}