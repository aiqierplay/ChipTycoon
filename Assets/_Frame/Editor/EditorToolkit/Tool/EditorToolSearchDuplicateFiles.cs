#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Object = UnityEngine.Object;

[EditorTool]
public class EditorToolSearchDuplicateFiles : EditorToolBase
{
    [Serializable]
    [LabelWidth(80)]
    public class FileData
    {
        public string FileName;
        public string FilePath;
        public Object Asset;
    }

    public override string GetTitle() => "Files/Duplicate Files";
    public override SdfIconType GetIcon() => SdfIconType.Files;

    protected bool IsToggled;
    protected int MaxCountInternal;
    private IEnumerator<FileInfo> _fileInfoIEnumerator;

    [Title("Search Folder", "Default Asset Folder", titleAlignment: TitleAlignments.Split)]
    [FolderPath(ParentFolder = "Assets", RequireExistingPath = true, AbsolutePath = true)]
    public string TargetFolder;

    [NonSerialized]
    public Dictionary<string, List<FileInfo>> SameMd5CacheGroup = new Dictionary<string, List<FileInfo>>();
    [NonSerialized]
    public Dictionary<string, List<FileInfo>> SameNameCacheGroup = new Dictionary<string, List<FileInfo>>();

    [TitleGroup("Duplicate Files")]
    [HorizontalGroup("Duplicate Files/Result")]
    [BoxGroup("Duplicate Files/Result/Same Md5", CenterLabel = true)]
    [PropertyOrder(1000)]
    [InfoBox("Find Same Md5 Files.", InfoMessageType.Error, nameof(CheckSameMd5ResultGroup))]
    [ShowIf(nameof(CheckSameMd5ResultGroup))]
    [DictionaryDrawerSettings(KeyLabel = "Md5", ValueLabel = "File")]
    public Dictionary<string, List<FileData>> SameMd5Result5Group = new Dictionary<string, List<FileData>>();


    [BoxGroup("Duplicate Files/Result/Same Name", CenterLabel = true)]
    [PropertyOrder(1000)]
    [InfoBox("Find Same Name Files.", InfoMessageType.Error, nameof(CheckSameNameResultGroup))]
    [ShowIf(nameof(CheckSameNameResultGroup))]
    [DictionaryDrawerSettings(KeyLabel = "File Name", ValueLabel = "File")]
    public Dictionary<string, List<FileData>> SameNameResultGroup = new Dictionary<string, List<FileData>>();

    public bool CheckSameMd5ResultGroup()
    {
        return SameMd5Result5Group.Count > 0;
    }

    private bool CheckSameNameResultGroup()
    {
        return SameNameResultGroup.Count > 0;
    }

    [PropertySpace(10, 20)]
    [HideIf(nameof(IsToggled))]
    [Button(ButtonSizes.Large)]
    public void StartSearch()
    {
        if (string.IsNullOrEmpty(TargetFolder))
        {
            TargetFolder = Application.dataPath;
        }

        ResetData();
        var directoryInfo = new DirectoryInfo(TargetFolder);
        var filesGroup = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Where(x => x.Extension != ".meta");

        MaxCountInternal = filesGroup.Count();
        _fileInfoIEnumerator = filesGroup.GetEnumerator();
        IsToggled = true;
        EditorApplication.update += Update;
    }

    private void ResetData()
    {
        MaxCountInternal = 0;
        MaxCount = 0;
        SameMd5CacheGroup.Clear();
        SameNameCacheGroup.Clear();
        SameMd5Result5Group.Clear();
        SameNameResultGroup.Clear();
        _fileInfoIEnumerator = null;
    }

    private void FilterDictionary()
    {
        var projectPath = Application.dataPath;
        SameMd5Result5Group = SameMd5CacheGroup
            .Where(f => f.Value.Count > 1)
            .ToDictionary(kv => kv.Key, list =>
            {
                var result = new List<FileData>();
                foreach (var file in list.Value)
                {
                    var assetPath = "Assets" + file.FullName.Substring(projectPath.Length);
                    var fileData = new FileData()
                    {
                        FileName = file.Name,
                        FilePath = file.FullName,
                        Asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object))
                    };

                    result.Add(fileData);
                }

                return result;
            });
        SameNameResultGroup = SameNameCacheGroup
            .Where(f => f.Value.Count > 1)
            .ToDictionary(kv => kv.Key, list =>
            {
                var result = new List<FileData>();
                foreach (var file in list.Value)
                {
                    var assetPath = "Assets" + file.FullName.Substring(projectPath.Length);
                    var fileData = new FileData()
                    {
                        FileName = file.Name,
                        FilePath = file.FullName,
                        Asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object))
                    };

                    result.Add(fileData);
                }

                return result;
            });
    }

    //绘制进度条
    [ReadOnly]
    [ProgressBar(0, nameof(MaxCountInternal), DrawValueLabel = true, ValueLabelAlignment = TextAlignment.Center, ColorGetter = nameof(GetProgressBarColor), Height = 30)]
    [ShowInInspector]
    [HideLabel]
    [ShowIf(nameof(IsToggled))]
    public int MaxCount { get; set; } 

    private Color GetProgressBarColor(int value)
    {
        return Color.white * 0.9f;
    }

    public void Update()
    {
        if (!IsToggled) return;
        if (_fileInfoIEnumerator.MoveNext())
        {
            if (_fileInfoIEnumerator.Current == null) return;
            var hashValue = GetMd5HashFromFile(_fileInfoIEnumerator.Current.FullName);
            if (!SameMd5CacheGroup.ContainsKey(hashValue))
            {
                SameMd5CacheGroup[hashValue] = new List<FileInfo>();
            }

            SameMd5CacheGroup[hashValue].Add(_fileInfoIEnumerator.Current);

            var fileName = _fileInfoIEnumerator.Current.Name;
            if (!SameNameCacheGroup.ContainsKey(fileName))
            {
                SameNameCacheGroup[fileName] = new List<FileInfo>();
            }

            SameNameCacheGroup[fileName].Add(_fileInfoIEnumerator.Current);

            MaxCount++;
        }
        else
        {
            EditorApplication.update -= Update;
            IsToggled = false;
            FilterDictionary();
        }
    }

    public string GetMd5HashFromFile(string fileFullName)
    {
        var file = new FileStream(fileFullName, FileMode.Open);
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        var retVal = md5.ComputeHash(file);
        file.Close();
        return BitConverter.ToString(retVal).ToLower().Replace("-", "");
    }
}
#endif