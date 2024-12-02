using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePotatoTransform : MonoBehaviour
{
    // 配置缩放范围
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    // 配置旋转范围
    public Vector3 minRotation = new Vector3(0f, 0f, 0f);
    public Vector3 maxRotation = new Vector3(360f, 360f, 360f);

    // 配置位移范围
    public Vector3 minPositionOffset = new Vector3(-1f, -1f, -1f);
    public Vector3 maxPositionOffset = new Vector3(1f, 1f, 1f);

    void Start()
    {
        // 在Start中调用方法确保执行
        RandomizeTransforms();
    }

    [ContextMenu("Randomize Potato Transforms")]
    public void RandomizeTransforms()
    {
        bool foundPotato = false;
        Transform[] allTransforms = GetComponentsInChildren<Transform>(true);

        // 输出所有物体名称，帮助调试
        foreach (Transform t in allTransforms)
        {
            Debug.Log($"Found object: {t.name}");
        }

        foreach (Transform t in allTransforms)
        {
            // 判断名称是否为Potato
            if (t.name == "Potato")
            {
                foundPotato = true;
                ApplyRandomTransform(t);
            }
        }

        if (!foundPotato)
        {
            Debug.LogWarning("No object named 'Potato' found in this Prefab.");
        }
        else
        {
            Debug.Log("Randomized transforms for all 'Potato' objects.");
        }
    }

    private void ApplyRandomTransform(Transform target)
    {
        // 随机缩放
        float randomScaleX = Random.Range(minScale.x, maxScale.x);
        float randomScaleY = Random.Range(minScale.y, maxScale.y);
        float randomScaleZ = Random.Range(minScale.z, maxScale.z);
        target.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);

        // 随机旋转
        float randomRotationX = Random.Range(minRotation.x, maxRotation.x);
        float randomRotationY = Random.Range(minRotation.y, maxRotation.y);
        float randomRotationZ = Random.Range(minRotation.z, maxRotation.z);
        target.localRotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);

        // 随机位移
        float randomOffsetX = Random.Range(minPositionOffset.x, maxPositionOffset.x);
        float randomOffsetY = Random.Range(minPositionOffset.y, maxPositionOffset.y);
        float randomOffsetZ = Random.Range(minPositionOffset.z, maxPositionOffset.z);
        target.localPosition += new Vector3(randomOffsetX, randomOffsetY, randomOffsetZ);

        // 输出日志，确认随机化效果
        Debug.Log($"Applied random transform to {target.name}: " +
                  $"Scale={target.localScale}, Rotation={target.localRotation.eulerAngles}, Position={target.localPosition}");
    }
}
