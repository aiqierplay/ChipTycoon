#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = nameof(UpgradeDataCsvGenerator), menuName = "Editor Setting/Upgrade Data Csv Generator")]
public class UpgradeDataCsvGenerator : EditorSettingBase<UpgradeDataCsvGenerator>
{
    public Object ConfigFolder;

    [Button("Generate")]
    public void Generate()
    {
        var outputPath = AssetDatabase.GetAssetPath(ConfigFolder).RemoveLeft("Assets/".Length);
        outputPath = Path.Combine(Application.dataPath, outputPath);
        var typeList = typeof(ConfigData).GetSubTypes();
        foreach (var type in typeList)
        {
            if (type.IsAbstract) continue;
            GenerateCsv(type, outputPath);
        }

        AssetDatabase.Refresh();
    }

    public void GenerateCsv(Type type, string outputPath)
    {
        var fileName = type.Name + ".csv";
        var filePath = Path.Combine(outputPath, fileName);
        var exist = File.Exists(filePath);
        if (exist) return;
        var text = "";
        var fieldList = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        fieldList.SortAsc(f =>
        {
            var orderAttribute = f.GetCustomAttribute<DataFieldOrderAttribute>();
            if (orderAttribute != null) return orderAttribute.Order;
            return 100;
        }, f => f.Name);

        for (var i = 0; i < fieldList.Length; i++)
        {
            var fieldInfo = fieldList[i];
            if (fieldInfo.GetCustomAttribute<NonSerializedAttribute>() != null) continue;
            text += fieldInfo.Name;
            if (i < fieldList.Length - 1) text += ",";
        }

        text += "\r\n";

        File.WriteAllText(filePath, text);
        Debug.Log("Generate " + fileName);
    }

}
#endif