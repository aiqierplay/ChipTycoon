using System;
using System.Collections.Generic;

namespace Aya.Notify
{
    public static class UNotify
    {
        public const char Split = '.';

        public static Dictionary<string, NotifyNode> RootDic = new Dictionary<string, NotifyNode>();
        public static Dictionary<string, NotifyNode> NodeDic = new Dictionary<string, NotifyNode>();

        public static NotifyNode CreateNode(string key, Action<bool> setNotifyAction = null, Func<bool> getNotifyAction = null)
        {
            var node = new NotifyNode();
            node.Init(key, setNotifyAction, getNotifyAction);
            node.Register();
            return node;
        }

        public static void RemoveNode(string key)
        {
            if (NodeDic.TryGetValue(key, out var node))
            {
                node.DeRegister();
            }
        }

        public static NotifyNode GetNode(string key, Action<bool> setNotifyAction = null, Func<bool> getNotifyAction = null)
        {
            if (NodeDic.TryGetValue(key, out var node)) return node;
            node = CreateNode(key, setNotifyAction, getNotifyAction);
            return node;
        }
    }
}