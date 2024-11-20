using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Aya.Data.Persistent
{
    [CreateAssetMenu(fileName = "USaveSetting", menuName = "USave/USaveSetting")]
    [Serializable]
#if ODIN_INSPECTOR
    [LabelWidth(150)]
#endif
    public class USaveSetting : ScriptableObject
    {
        #region Instance

        public static USaveSetting Ins
        {
            get
            {
                if (Instance == null) Instance = LoadResources();
                return Instance;
            }
        }

        protected static USaveSetting Instance;

        public static USaveSetting LoadResources()
        {
            var setting = Resources.Load<USaveSetting>(nameof(USaveSetting));
            setting.Init();
            return setting;
        }

        #endregion

#if ODIN_INSPECTOR
        [Title("USave Setting")]
        [EnumToggleButtons]
#endif
        public USaveMode Mode = USaveMode.Single;

#if ODIN_INSPECTOR
        [EnumToggleButtons]
#endif
        public USaveFormat Format = USaveFormat.PlayerPrefs;

#if ODIN_INSPECTOR
        [EnumToggleButtons]
#endif
        public USaveAutoMode AutoMode = USaveAutoMode.PauseQuit;

        public float AutoSaveInterval = 300;
        public bool AutoSaveAsync = false;
        public bool AutoLoadMainData = true;
        public bool Encrypt = false;
        public string FileExtension = ".sav";
        public string MainDataKey = "Save_Main";
        public string DefaultSlotKey = "Save_Default";

        public virtual void Init()
        {

        }
    }
}