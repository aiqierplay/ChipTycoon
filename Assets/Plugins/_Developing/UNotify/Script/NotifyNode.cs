using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aya.Notify
{
    [Serializable]
    public class NotifyNode
    {
        public string Key;

        // Cache
        public string[] KeyPath;
        public string ParentKey;

        public int PathDeep => KeyPath.Length;
        public NotifyChecker Checker;
        public bool IsRegistered;

        [NonSerialized] public List<NotifyNode> Parents = new List<NotifyNode>();
        [NonSerialized] public List<NotifyNode> Children = new List<NotifyNode>();

        public bool IsRoot => Parents.Count == 0 && PathDeep == 1;
        public bool IsLeaf => Children.Count == 0;

        public Action OnActive = delegate { };
        public Action OnDeActive = delegate { };

        public virtual void Init(string key, Action<bool> setNotifyAction = null, Func<bool> getNotifyAction = null)
        {
            Key = key;
            KeyPath = Key.Split(UNotify.Split);
            ParentKey = null;
            Parents.Clear();
            Children.Clear();
            if (KeyPath.Length > 1)
            {
                ParentKey = Key.Substring(0, Key.LastIndexOf(KeyPath[KeyPath.Length - 1], StringComparison.Ordinal) - 1);
            }

            InitChecker(setNotifyAction, getNotifyAction);
            IsRegistered = false;
        }

        public virtual void InitChecker(Action<bool> setNotifyAction = null, Func<bool> getNotifyAction = null)
        {
            Checker = new NotifyChecker();
            Checker.Init(Key, setNotifyAction, getNotifyAction);
        }

        public virtual void Register()
        {
            if (IsRegistered) return;
            if (IsRoot)
            {
                UNotify.RootDic.Add(Key, this);
            }
            else
            {
                var node = this;
                while (!node.IsRoot)
                {
                    var parentNode = UNotify.GetNode(ParentKey);
                    var addResult = parentNode.AddChild(node);
                    if (!addResult) break;
                    node = parentNode;
                }
            }


            // TODO.. 创建节点树

        }

        public virtual void DeRegister()
        {
            if (IsRoot) UNotify.RootDic.Remove(Key);
            UNotify.NodeDic.Remove(Key);
            foreach (var parent in Parents)
            {
                parent.Children.Remove(this);
            }

            foreach (var child in Children)
            {
                child.Parents.Remove(this);
            }
        }

        public virtual bool GetNotify()
        {
            if (IsLeaf) return Checker.GetNotify();
            foreach (var child in Children)
            {
               var active = child.GetNotify();
               if (active) return true;
            }

            return false;
        }

        public virtual void SetNotify(bool active)
        {
            var current = GetNotify();
            if (IsLeaf)
            {
                if (current == active) return;
                Checker.SetNotify(active);
                foreach (var parent in Parents)
                {
                    parent.SetNotify(active);
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.SetNotify(active);
                }
            }

            if (active != current)
            {
                if (active) OnActive();
                else OnDeActive();
            }
        }

        public virtual int GetNotifyCount()
        {
            if (IsLeaf) return GetNotify() ? 1 : 0;
            var count = 0;
            foreach (var child in Children)
            {
                count += child.GetNotifyCount();
            }

            return count;
        }

        public bool AddChild(NotifyNode childNode)
        {
            if (Children.Contains(childNode)) return false;
            Children.Add(childNode);
            childNode.Parents.Add(this);
            return true;
        }

        public void RemoveChild(NotifyNode chiNode)
        {
            if (!Children.Contains(chiNode)) return;
            Children.Remove(chiNode);
            chiNode.Parents.Remove(this);
        }
    }
}
