using System;
using UnityEngine;

namespace Aya.Notify
{
    public class NotifyChecker
    {
        public string Key;

        public Action<bool> SetNotifyAction;
        public Func<bool> GetNotifyAction;

        public virtual void Init(string key, Action<bool> setNotifyAction = null, Func<bool> getNotifyAction = null)
        {
            Key = key;
            if (setNotifyAction == null || getNotifyAction == null)
            {
                SaveKey = nameof(UNotify) + "_" + Key;
                SetNotifyAction = SetNotifyDefault;
                GetNotifyAction = GetNotifyDefault;
            }
            else
            {
                SetNotifyAction = setNotifyAction;
                GetNotifyAction = getNotifyAction;
            }
        }

        public virtual void SetNotify(bool active)
        {
            SetNotifyAction(active);
        }

        public virtual bool GetNotify()
        {
            return GetNotifyAction();
        }

        #region Default Impl
       
        protected string SaveKey;

        protected virtual void SetNotifyDefault(bool active)
        {
            PlayerPrefs.SetInt(SaveKey, active ? 1 : 0);
            PlayerPrefs.Save();
        }

        protected virtual bool GetNotifyDefault()
        {
            return PlayerPrefs.GetInt(SaveKey, 0) == 1;
        } 

        #endregion
    }
}