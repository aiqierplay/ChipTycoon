#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[EditorTool]
public class EditorToolFileReference : EditorToolBase
{
    public override string GetTitle() => "Files/File Reference";
    public override SdfIconType GetIcon() => SdfIconType.Link45deg;

    [BoxGroup("File")]
    [AssetsOnly]
    public Object TargetFile;

    [BoxGroup("File")]
    [Button("Find selected file reference", ButtonSizes.Large)]
    public void FindSelectedFileReference()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        var path = AssetDatabase.GetAssetPath(TargetFile);

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Selected file is NULL");
            return;
        }

        var guid = AssetDatabase.AssetPathToGUID(path);
        var extensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
        var files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
            .Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        var startIndex = 0;

        EditorApplication.update = delegate()
        {
            var file = files[startIndex];
            var isCancel = EditorUtility.DisplayCancelableProgressBar("Searching...", file,
                (float)startIndex / (float)files.Length);
            if (Regex.IsMatch(File.ReadAllText(file), guid))
            {
                Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
            }

            startIndex++;
            if (isCancel || startIndex >= files.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                startIndex = 0;
                Debug.Log("Search Complete");
            }
        };
    }

    [BoxGroup("Guid")] public string TargetGuid;

    [BoxGroup("Guid")]
    [Button("Find file Guid reference", ButtonSizes.Large)]
    public void FindFileGuidReference()
    {
        var findGuid = TargetGuid;
        if (string.IsNullOrEmpty(findGuid))
        {
            Debug.LogError("Guid is NULL");
            return;
        }

        EditorSettings.serializationMode = SerializationMode.ForceText;
        var extensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset", ".meta" };
        var directoryPath = Application.dataPath;
        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        var startIndex = 0;

        EditorApplication.update = delegate()
        {
            var file = files[startIndex];
            var isCancel =
                EditorUtility.DisplayCancelableProgressBar("Searching...", file,
                    (float)startIndex / (float)files.Length);
            if (Regex.IsMatch(File.ReadAllText(file), findGuid))
            {
                Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
            }

            startIndex++;
            if (isCancel || startIndex >= files.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                startIndex = 0;
                Debug.Log("Search Complete");
            }
        };
    }

    [BoxGroup("Guid")]
    [Button("Find file Guid reference in PackageCache", ButtonSizes.Large)]
    public void FindFileGuidPackageCacheReference()
    {
        var findGuid = TargetGuid;
        if (string.IsNullOrEmpty(findGuid))
        {
            Debug.LogError("Guid is NULL");
            return;
        }

        EditorSettings.serializationMode = SerializationMode.ForceText;
        var extensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset", ".meta" };
        var directoryPath = Application.dataPath.Replace("Assets", "") + "Library\\PackageCache";
        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        var startIndex = 0;

        EditorApplication.update = delegate()
        {
            var file = files[startIndex];
            var isCancel =
                EditorUtility.DisplayCancelableProgressBar("Searching...", file,
                    (float)startIndex / (float)files.Length);

            if (Regex.IsMatch(File.ReadAllText(file), findGuid))
            {
                Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
            }

            startIndex++;
            if (isCancel || startIndex >= files.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                startIndex = 0;
                Debug.Log("Search Complete");
            }
        };
    }

    [BoxGroup("Guid")]
    [Button("Find file with Guid", ButtonSizes.Large)]
    public void FindFileWithGuid()
    {
        var findGuid = TargetGuid;
        if (string.IsNullOrEmpty(findGuid))
        {
            Debug.LogError("Guid is NULL");
            return;
        }

        var path = AssetDatabase.GUIDToAssetPath(findGuid);
        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log(path, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(path)));
        }
        else
        {
            Debug.LogError("Not found " + findGuid);
        }
    }

    public string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }

    [BoxGroup("Prefab")]
    [AssetsOnly]
    public Object TargetPrefab;

    /// <summary>
    /// 查找预制在所有场景中的引用
    /// </summary>
    [BoxGroup("Prefab")]
    [Button("Find prefab reference in scene", ButtonSizes.Large)]
    public void FindPrefabUsingInAllScenes()
    {
        if (TargetPrefab == null)
        {
            Debug.LogError("Selected GameObject is NULL");
            return;
        }

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.AdditiveWithoutLoading);
            var gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
            foreach (var go in gos)
            {
                if (PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular)
                {
                    var parentObject = PrefabUtility.GetCorrespondingObjectFromSource(go);
                    if (parentObject == TargetPrefab)
                    {
                        Debug.Log(scene.path + "/" + GetGameObjectPath(go));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 查找预制在所有场景中的引用(包含子节点)
    /// </summary>
    [BoxGroup("Prefab")]
    [Button("Find prefab reference in scene (Include Child)", ButtonSizes.Large)]
    public void FindPrefabAllUsingInAllScenes()
    {
        if (TargetPrefab == null)
        {
            Debug.LogError("Selected GameObject is NULL");
            return;
        }

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.AdditiveWithoutLoading);
            var gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
            foreach (var go in gos)
            {
                if (PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular)
                {
                    var parentObject = PrefabUtility.GetCorrespondingObjectFromSource(go);
                    var path = AssetDatabase.GetAssetPath(parentObject);
                    if (path == AssetDatabase.GetAssetPath(TargetPrefab))
                    {
                        Debug.Log(scene.path + "/" + GetGameObjectPath(go));
                    }
                }
            }
        }
    }

    public string GetGameObjectPath(GameObject obj)
    {
        var path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }

        return path;
    }
}
#endif