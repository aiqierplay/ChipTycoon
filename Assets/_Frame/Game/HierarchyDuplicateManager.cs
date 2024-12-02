using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyDuplicateManager : MonoBehaviour
{
    // 需要操作的目标物体列表
    public List<GameObject> targetObjects = new List<GameObject>();

    // 偏移量，用于调整复制体的位置
    public Vector3 offsetParent = new Vector3(1, 0, 0);  // 父物体复制的偏移
    public Vector3 offsetSibling = new Vector3(0, 1, 0); // 子物体复制的偏移

    // 当前逻辑状态
    private int logicState = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 按下空格键触发
        {
            if (logicState == 0)
            {
                ExecuteLogic1(); // 执行逻辑 1
                logicState = 1; // 切换到逻辑 2
            }
            else if (logicState == 1)
            {
                ExecuteLogic2(); // 执行逻辑 2
                logicState = 0; // 切换回逻辑 1
            }
        }
    }

    // 逻辑 1：复制目标物体并作为其子物体
    private void ExecuteLogic1()
    {
        Debug.Log("Executing Logic 1...");
        foreach (GameObject target in targetObjects)
        {
            // 复制目标物体并作为其子物体
            DuplicateObject(target, target.transform, offsetParent);
        }
    }

    // 逻辑 2：复制新增子物体并与其放在同一层级
    private void ExecuteLogic2()
    {
        Debug.Log("Executing Logic 2...");
        foreach (GameObject target in targetObjects)
        {
            if (target.transform.childCount > 0)
            {
                Transform firstChild = target.transform.GetChild(0); // 获取第一个子物体

                // 复制该子物体并与其放在同一层级
                DuplicateObject(firstChild.gameObject, firstChild.parent, offsetSibling);
            }
            else
            {
                Debug.LogWarning($"Target {target.name} has no child to duplicate for Logic 2.");
            }
        }
    }

    // 通用的复制方法
    private GameObject DuplicateObject(GameObject obj, Transform parent = null, Vector3 customOffset = default)
    {
        // 创建物体的副本
        GameObject duplicate = Instantiate(obj);

        // 设置复制体的位置
        duplicate.transform.position = obj.transform.position + customOffset;

        // 设置复制体的父物体
        if (parent != null)
        {
            duplicate.transform.SetParent(parent);
        }

        // 修改复制体的名称
        duplicate.name = obj.name + "_Copy";

        Debug.Log($"Duplicated {obj.name} to {duplicate.name}");
        return duplicate;
    }
}
