using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Notify
{
    public class UNotifyManager : MonoBehaviour
    {
        public HashSet<NotifyNode> UpdateNodeList = new HashSet<NotifyNode>();

        public void AddUpdateNode(NotifyNode node)
        {
            // TODO 如何过滤重复刷新？
            if (UpdateNodeList.Contains(node)) return;
            foreach (var existNode in UpdateNodeList)
            {
                if (existNode.Key.Contains(node.Key)) return;
            }

            UpdateNodeList.Add(node);
        }

        public void Update()
        {
            if (UpdateNodeList.Count == 0) return;
            foreach (var node in UpdateNodeList)
            {
                
            }
        }
    }
}