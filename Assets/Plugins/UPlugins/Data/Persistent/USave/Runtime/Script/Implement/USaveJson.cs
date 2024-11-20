using Aya.Data.Json;
using System.Collections.Generic;
using System.IO;

namespace Aya.Data.Persistent
{
    public class USaveJson : USaveBase
    {
        #region Json

        public Dictionary<string, JObject> JsonDic = new Dictionary<string, JObject>();

        public string GetFilePath(string slotKey)
        {
            var fileName = $"{slotKey}{USaveSetting.Ins.FileExtension}";
            var filePath = Path.Combine(USave.FilePath, fileName);
            return filePath;
        }

        public JObject LoadJson(string slotKey)
        {
            if (JsonDic.TryGetValue(slotKey, out var json)) return json;
            var filePath = GetFilePath(slotKey);
            if (File.Exists(filePath))
            {
                var jsonStr = File.ReadAllText(filePath);
                if (USaveSetting.Ins.Encrypt) jsonStr = USaveInterface.DecryptFunc(jsonStr);
                json = JObject.Parse(jsonStr);
            }
            else
            {
                json = new JObject();
            }

            JsonDic.Add(slotKey, json);
            return json;
        }

        public void SaveJson(string slotKey, JObject json)
        {
            var filePath = GetFilePath(slotKey);
            if (USaveSetting.Ins.Encrypt)
            {
                var text = USaveInterface.EncryptFunc(json.ToString());
                File.WriteAllText(filePath, text);
            }
            else
            {
                var text = json.ToFormatString();
                File.WriteAllText(filePath, text);
            }
        }

        #endregion

        #region Key List
        
        public override IEnumerable<string> GetAllKeys(string slotKey)
        {
            var json = LoadJson(slotKey);
            return json.Keys;
        } 

        #endregion

        #region Get / Set Value

        public override T GetValue<T>(string slotKey, string valueKey, T defaultValue = default(T))
        {
            var json = LoadJson(slotKey);
            var value = json.Get(valueKey);
            if (value == null || !value.IsValid) return defaultValue;
            var result = USaveInterface.CastType<T>(value.ToString());
            return result;
        }

        public override void SetValue<T>(string slotKey, string valueKey, T value)
        {
            var json = LoadJson(slotKey);
            if (value == null) return;
            json[valueKey] = value.ToString();
        }

        #endregion

        #region Get / Set Object

        public override T GetObject<T>(string slotKey, string valueKey, T defaultValue = default(T))
        {
            var json = LoadJson(slotKey);
            var value = json.Get(valueKey);
            if (value == null || !value.IsValid) return defaultValue;
            var result = JsonUtil.ToObject<T>(value.ToString());
            return result;
        }

        public override void SetObject<T>(string slotKey, string valueKey, T value)
        {
            var json = LoadJson(slotKey);
            if (value == null) return;
            json[valueKey] = JsonUtil.ToJson(value);
        }

        #endregion

        #region Exsit Key

        public override bool ExistKey(string slotKey, string valueKey)
        {
            var json = LoadJson(slotKey);
            var value = json.Get(valueKey);
            return value != null && value.IsValid;
        }

        #endregion

        #region Delete

        public override bool DeleteKey(string slotKey, string valueKey)
        {
            var json = LoadJson(slotKey);
            if (!ExistKey(valueKey)) return false;
            json.Remove(valueKey);
            return true;
        }

        public override void DeleteAll(string slotKey)
        {
            JsonDic[slotKey] = new JObject();
        }

        #endregion

        #region Load / Save

        public override void Load(string slotKey)
        {
            LoadJson(slotKey);
        }

        public override void Save(string slotKey)
        {
            var json = LoadJson(slotKey);
            SaveJson(slotKey, json);
        }

        #endregion
    }
}