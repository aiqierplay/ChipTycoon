using System;
using UnityEngine;

namespace Aya.DragDrop
{
    public abstract class DragDropBase : MonoBehaviour
    {
        [Header("Drag Drop")]
        public string GroupKey = UDragDrop.DefaultGroupKey;

        [NonSerialized] public DragGroupData Group;
        [NonSerialized] public Transform Transform;

        #region MonoBehaviour
       
        protected virtual void Awake()
        {
            Group = UDragDrop.GetGroupData(GroupKey);
            Transform = transform;
        } 

        #endregion

        #region Register / DeRegister

        public abstract void Register();

        public abstract void DeRegister();

        public virtual void ChangeGroup(string groupKey)
        {
            if (GroupKey == groupKey) return;
            DeRegister();
            GroupKey = groupKey;
            Register();
        }

        #endregion
    }
}