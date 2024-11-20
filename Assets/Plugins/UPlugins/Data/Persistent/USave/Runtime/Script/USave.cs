using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Aya.Data.Persistent
{
    public static class USave
    {
        public static string FilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_savePath)) return _savePath;
                _savePath = Path.Combine(Application.persistentDataPath, "Save");
                if (!Directory.Exists(_savePath))
                {
                    Directory.CreateDirectory(_savePath);
                }

                return _savePath;
            }
        }

        private static string _savePath;

        public static USaveMainData MainData { get; internal set; }
        public static string CurrentSlotKey { get; internal set; }
        public static bool IsAsyncWorking { get; internal set; } = false;
        public static bool RequireSave { get; internal set; } = false;
        public static bool DataChanged { get; internal set; } = false;
        public static List<USaveSlotData> SlotList => MainData.SlotList;

        public static USaveSlotData CurrentSlot
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentSlotKey)) return default;
                return MainData.SlotDic[CurrentSlotKey];
            }
        }

        [NonSerialized] internal static USaveBase SaveInstance;
        [NonSerialized] internal static bool IsApplicationPlayingCache = false;

        #region Init

        static USave()
        {
            if (Application.isPlaying)
            {
                IsApplicationPlayingCache = true;
                USaveHandler.Ins.Init();
            }
           
            CurrentSlotKey = null;
            SaveInstance = Create(USaveSetting.Ins.Format);
            if (USaveSetting.Ins.AutoLoadMainData) LoadMainData();
            switch (USaveSetting.Ins.Mode)
            {
                case USaveMode.Single:
                    {
                        if (SlotList.Count == 0)
                        {
                            CreateSlot(USaveSetting.Ins.DefaultSlotKey);
                        }

                        SelectSlot(USaveSetting.Ins.DefaultSlotKey);
                        Load();
                        break;
                    }
                case USaveMode.Multi:
                    break;
            }
        }

        internal static USaveBase Create(USaveFormat saveFormat)
        {
            switch (saveFormat)
            {
                case USaveFormat.Json:
                    return new USaveJson();
                case USaveFormat.PlayerPrefs:
                    return new USavePlayerPrefs();
            }

            return default;
        } 

        #endregion

        #region Main Data (Always Automatic)

        public static void LoadMainData()
        {
            if (MainData != null) return;
            MainData = SaveInstance.GetObject<USaveMainData>(USaveSetting.Ins.MainDataKey, nameof(USaveMainData));
            if (MainData == null)
            {
                MainData = new USaveMainData();
            }

            MainData.Init();
            if (IsApplicationPlayingCache)
            {
                MainData.LaunchCount++;
                MainData.LoadTime = DateTime.Now;
            }
        }

        public static void SaveMainData()
        {
            if (IsApplicationPlayingCache)
            {
                MainData.LastSaveTime = DateTime.Now;
                MainData.SaveCount++;
                MainData.RunTime += (float)(MainData.LastSaveTime - MainData.LoadTime).TotalSeconds;
            }

            SaveInstance.SetObject(USaveSetting.Ins.MainDataKey, nameof(USaveMainData), MainData);
            DataChanged = true;
        }

        #endregion

        #region Slot List (Single save mode is automatic)

        public static USaveSlotData CreateSlot(string slotKey)
        {
            var slotData = new USaveSlotData()
            {
                Key = slotKey,
                CreateTime = DateTime.Now,
            };

            MainData.Add(slotData);
            DataChanged = true;
            return slotData;
        }

        public static bool ExistSlot(string slotKey)
        {
            return MainData.Exist(slotKey);
        }

        public static void SelectSlot(string slotKey)
        {
            if (!ExistSlot(slotKey)) return;
            CurrentSlotKey = slotKey;
        }

        public static void DeleteSlot(string slotKey)
        {
            if (!ExistSlot(slotKey)) return;
            if (CurrentSlotKey == slotKey) CurrentSlotKey = null;
            SaveInstance.DeleteAll(slotKey);
            MainData.Remove(slotKey);
            DataChanged = true;
        }

        public static void ResetSlot(string slotKey)
        {
            if (!ExistSlot(slotKey)) return;
            DeleteSlot(USave.CurrentSlotKey);
            CreateSlot(USaveSetting.Ins.DefaultSlotKey);
            SelectSlot(USaveSetting.Ins.DefaultSlotKey);
            DataChanged = true;
        }

        #endregion

        #region Key List

        public static IEnumerable<string> GetAllKeys(string slotKey)
        {
            return SaveInstance.GetAllKeys(slotKey);
        }

        #endregion

        #region Load / Save

        public static void Load()
        {
            if (MainData == null) LoadMainData();
            if (!ExistSlot(CurrentSlotKey)) return;
            SaveInstance.Load();
            if (IsApplicationPlayingCache)
            {
                CurrentSlot.LaunchCount++;
                CurrentSlot.LoadTime = DateTime.Now;
            }
        }

        public static void Save()
        {
            RequireSave = true;
        }

        public static void SaveImmediately()
        {
            if (!ExistSlot(CurrentSlotKey)) return;
            SaveMainData();
            SaveInstance.Save(USaveSetting.Ins.MainDataKey);

            if (IsApplicationPlayingCache)
            {
                CurrentSlot.LastSaveTime = DateTime.Now;
                CurrentSlot.SaveCount++;
                CurrentSlot.RunTime += (float)(CurrentSlot.LastSaveTime - CurrentSlot.LoadTime).TotalSeconds;
            }

            SaveInstance.Save(CurrentSlotKey);
            DataChanged = false;
        }

        #endregion

        #region Load / Save Async

        public static async void LoadAsync(Action done)
        {
            if (SaveInstance.SupportAsync)
            {
                if (IsAsyncWorking) return;
                IsAsyncWorking = true;
                await Task.Run(Load);
                done?.Invoke();
                IsAsyncWorking = false;
                
            }
            else
            {
                Load();
                done?.Invoke();
            }
        }

        public static async void SaveAsync(Action done)
        {
            if (SaveInstance.SupportAsync)
            {
                if (IsAsyncWorking) return;
                IsAsyncWorking = true;
                await Task.Run(SaveImmediately);
                done?.Invoke();
                IsAsyncWorking = false;
            }
            else
            {
                SaveImmediately();
                done?.Invoke();
            }
        }

        #endregion

        #region Get / Set Value

        public static T GetValue<T>(string valueKey, T defaultValue = default(T))
        {
            return SaveInstance.GetValue(valueKey, defaultValue);
        }

        public static void SetValue<T>(string valueKey, T value)
        {
            SaveInstance.SetValue(valueKey, value);
            DataChanged = true;
        }

        #endregion

        #region Get / Set Object

        public static T GetObject<T>(string valueKey, T defaultValue = default(T))
        {
            return SaveInstance.GetObject(valueKey, defaultValue);
        }

        public static void SetObject<T>(string valueKey, T value)
        {
            SaveInstance.SetObject(valueKey, value);
            DataChanged = true;
        }

        #endregion

        #region Exist Key

        public static bool ExistKey(string valueKey)
        {
            return SaveInstance.ExistKey(valueKey);
        }

        #endregion

        #region Delete

        public static bool DeleteKey(string valueKey)
        {
            var result = SaveInstance.DeleteKey(valueKey);
            if (result) DataChanged = true;
            return result;
        }

        public static void DeleteAll()
        {
            SaveInstance.DeleteAll();
            DataChanged = true;
        }

        #endregion
    }
}