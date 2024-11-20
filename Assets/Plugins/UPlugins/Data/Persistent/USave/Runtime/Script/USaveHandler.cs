using System;
using UnityEngine;

namespace Aya.Data.Persistent
{
    public class USaveHandler : MonoBehaviour
    {
        #region Singleton

        internal static bool ApplicationIsQuitting = false;
        internal static USaveHandler Instance;

        public static USaveHandler Ins
        {
            get
            {
                if (Application.isPlaying && ApplicationIsQuitting)
                {
                    return null;
                }

                if (Instance != null) return Instance;
                Instance = FindObjectOfType<USaveHandler>();
                if (Instance != null)
                {
                    if (Application.isPlaying) DontDestroyOnLoad(Instance);
                    return Instance;
                }

                var hideFlag = HideFlags.None;
                var insName = nameof(USave);
                if (!Application.isPlaying)
                {
                    insName += " (Editor)";
                }

                var obj = new GameObject
                {
                    name = insName,
                    hideFlags = hideFlag,
                };

                Instance = obj.AddComponent<USaveHandler>();
                if (Application.isPlaying) DontDestroyOnLoad(Instance);
                return Instance;
            }
        }

        #endregion

        [NonSerialized] internal bool IsFirstLaunch = true;
        [NonSerialized] internal float AutoSaveTimer = 0f;

        public void Init()
        {

        }

        public void LateUpdate()
        {
            SaveIfRequired();

            if (USaveSetting.Ins.AutoMode == USaveAutoMode.Interval)
            {
                AutoSaveTimer += Time.unscaledDeltaTime;
                if (AutoSaveTimer >= USaveSetting.Ins.AutoSaveInterval)
                {
                    AutoSaveTimer -= USaveSetting.Ins.AutoSaveInterval;
                    AutoSave();
                }
            }
        }

        public void OnApplicationFocus(bool isFocusOn)
        {
            if (isFocusOn) return;
            if (USaveSetting.Ins.AutoMode != USaveAutoMode.Manual) USave.SaveImmediately();
        }

        public void OnApplicationPause()
        {
            if (IsFirstLaunch)
            {
                IsFirstLaunch = false;
                return;
            }

            if (USaveSetting.Ins.AutoMode != USaveAutoMode.Manual) USave.SaveImmediately();
        }

        public void OnApplicationQuit()
        {
            if (USaveSetting.Ins.AutoMode != USaveAutoMode.Manual) USave.SaveImmediately();
        }

        public void SaveIfRequired()
        {
            if (!USave.RequireSave) return;
            if (!USave.DataChanged)
            {
                USave.RequireSave = false;
                return;
            }

            USave.SaveImmediately();
            USave.RequireSave = false;
        }

        public void AutoSave()
        {
            if (!USave.DataChanged) return;
            if (USaveSetting.Ins.AutoSaveAsync)
            {
                USave.SaveAsync(() =>
                {

                });
            }
            else
            {
                USave.SaveImmediately();
            }
        }
    }
}