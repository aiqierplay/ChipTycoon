using System.Collections.Generic;
using System.Linq;
using Aya.Data.Json;
using UnityEngine;

namespace Aya.Data.Persistent
{
    public class USavePlayerPrefs : USaveBase
    {
        public override bool SupportAsync => false;

        public virtual string GetValueKey(string slotKey, string valueKey)
        {
            var result = $"{slotKey}_{valueKey}";
            return result;
        }

        #region PlayerPrefs Key

        public Dictionary<string, HashSet<string>> KeyListDic = new Dictionary<string, HashSet<string>>();

        public virtual string GetPlayerPrefsKeyListKey(string slotKey)
        {
            var result = $"{slotKey}_PlayerPrefsKeyList";
            return result;
        }

        public virtual HashSet<string> LoadPlayerPrefsKeyList(string slotKey)
        {
            if (KeyListDic.TryGetValue(slotKey, out var keyList)) return keyList;
            var key = GetPlayerPrefsKeyListKey(slotKey);
            keyList = new HashSet<string>();
            var saveKeyList = GetObject<List<string>>(key);
            if (saveKeyList != null)
            {
                foreach (var valueKey in saveKeyList)
                {
                    keyList.Add(valueKey);
                }
            }

            KeyListDic.Add(slotKey, keyList);
            return keyList;
        }

        public virtual void SavePlayerPrefsKeyList(string slotKey)
        {
            var keyList = LoadPlayerPrefsKeyList(slotKey).ToList();
            var key = GetPlayerPrefsKeyListKey(slotKey);
            SetObject(key, keyList);
        } 

        #endregion

        #region Key List

        public override IEnumerable<string> GetAllKeys(string slotKey)
        {
            return LoadPlayerPrefsKeyList(slotKey);
        }

        #endregion

        #region Get / Set Value

        public override T GetValue<T>(string slotKey, string valueKey, T defaultValue = default(T))
        {
            var key = GetValueKey(slotKey, valueKey);
            var value = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(value)) return defaultValue;
            if (USaveSetting.Ins.Encrypt) value = USaveInterface.EncryptFunc(value);
            var result = USaveInterface.CastType<T>(value);
            return result;
        }

        public override void SetValue<T>(string slotKey, string valueKey, T value)
        {
            var key = GetValueKey(slotKey, valueKey);
            if (value != null)
            {
                var saveValue = value.ToString();
                if (USaveSetting.Ins.Encrypt) saveValue = USaveInterface.DecryptFunc(saveValue);
                PlayerPrefs.SetString(key, saveValue);
            }
            else
            {
                PlayerPrefs.SetString(key, null);
            }

            var keyList = LoadPlayerPrefsKeyList(slotKey);
            keyList.Add(valueKey);
        }

        #endregion

        #region Get / Set Object

        public override T GetObject<T>(string slotKey, string valueKey, T defaultValue = default(T))
        {
            var key = GetValueKey(slotKey, valueKey);
            var value = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(value)) return defaultValue;
            if (USaveSetting.Ins.Encrypt) value = USaveInterface.EncryptFunc(value);
            var result = JsonUtil.ToObject<T>(value, defaultValue);
            return result;
        }

        public override void SetObject<T>(string slotKey, string valueKey, T value)
        {
            var key = GetValueKey(slotKey, valueKey);
            if (value != null)
            {
                var saveValue = JsonUtil.ToJson(value);
                if (USaveSetting.Ins.Encrypt) saveValue = USaveInterface.DecryptFunc(saveValue);
                PlayerPrefs.SetString(key, saveValue);
            }
            else
            {
                PlayerPrefs.SetString(key, null);
            }

            var keyList = LoadPlayerPrefsKeyList(slotKey);
            keyList.Add(valueKey);
        }

        #endregion

        #region Exsit Key

        public override bool ExistKey(string slotKey, string valueKey)
        {
            var key = GetValueKey(slotKey, valueKey);
            return PlayerPrefs.HasKey(key);
        }

        #endregion

        #region Delete

        public override bool DeleteKey(string slotKey, string valueKey)
        {
            var exist = ExistKey(slotKey, valueKey);
            if (!exist) return false;
            var key = GetValueKey(slotKey, valueKey);
            PlayerPrefs.DeleteKey(key);
            var keyList = LoadPlayerPrefsKeyList(slotKey);
            keyList.Remove(key);
            return true;
        }

        #endregion

        #region Load / Save

        public override void Load(string slotKey)
        {
            LoadPlayerPrefsKeyList(slotKey);
        }

        public override void Save(string slotKey)
        {
            SavePlayerPrefsKeyList(slotKey);
            PlayerPrefs.Save();
        }

        #endregion
    }
}