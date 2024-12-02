using UnityEngine;

public class AddChildToCurrentObject : MonoBehaviour
{
    public GameObject childPrefab; // 要添加的子物体Prefab
    public string targetName = "dig"; // 搜索的目标名称关键字

    void Start()
    {
        AddChildToCurrent(); // 游戏开始时自动执行
    }

    public void AddChildToCurrent()
    {
        if (childPrefab == null)
        {
            Debug.LogError("Child Prefab is not assigned!");
            return;
        }

        // 获取当前物体及其所有子物体
        Transform[] allTransforms = GetComponentsInChildren<Transform>(true);
        bool foundTarget = false;

        foreach (Transform t in allTransforms)
        {
            if (t.name.Contains(targetName))
            {
                foundTarget = true;
                AddChildObject(t);
            }
        }

        if (!foundTarget)
        {
            Debug.LogWarning($"No object found with the name containing '{targetName}' under {gameObject.name}");
        }
        else
        {
            Debug.Log("Finished adding child objects!");
        }
    }

    private void AddChildObject(Transform target)
    {
        // 检查是否重复添加
        if (target.Find(childPrefab.name) != null)
        {
            Debug.LogWarning($"Object '{childPrefab.name}' already exists under '{target.name}', skipping...");
            return;
        }

        // 创建子物体
        GameObject child = Instantiate(childPrefab);
        child.name = childPrefab.name; // 确保名称一致
        child.transform.SetParent(target);
        child.transform.localPosition = Vector3.zero; // 可根据需求调整
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        Debug.Log($"Added '{childPrefab.name}' to '{target.name}'");
    }
}
