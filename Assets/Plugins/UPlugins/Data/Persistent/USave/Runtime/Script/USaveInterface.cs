using Aya.Security;
using System;
using System.Globalization;

namespace Aya.Data.Persistent
{
    public static class USaveInterface
    {
        public static Func<string, string> EncryptFunc = RC4Util.Encrypt;
        public static Func<string, string> DecryptFunc = RC4Util.Decrypt;
        public static Func<string, string> GetMd5Func = MD5Util.GetMd5;

        public static T CastType<T>(string value)
        {
            var type = typeof(T);
            var result = (T)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            return result;
        }
    }
}