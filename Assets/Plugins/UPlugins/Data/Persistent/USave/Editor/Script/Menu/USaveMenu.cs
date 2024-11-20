#if UNITY_EDITOR
using System.IO;
using Aya.Util;
using UnityEditor;

namespace Aya.Data.Persistent
{
    public static class USaveMenu
    {
        [MenuItem("Tools/Aya Game/USave/Open Save Data Folder")]
        public static void OpenSaveDataPath()
        {
            var path = USave.FilePath;
            EditorUtility.RevealInFinder(path);
        }

        [MenuItem("Tools/Aya Game/USave/Clear Save Data Path")]
        public static void ClearSaveDataPath()
        {
            var path = USave.FilePath;
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }
    }
}
#endif