using UnityEngine;

public class LineWithModels : MonoBehaviour
{
    public LineRenderer lineRenderer;   // 你的 LineRenderer 组件
    public GameObject modelPrefab;      // 用来替代线条的 3D 模型（例如：圆柱体、立方体等）
    public float modelSpacing = 0.5f;   // 模型之间的间隔

    void Start()
    {
        CreateLineWithModels();
    }

    void CreateLineWithModels()
    {
        int positionCount = lineRenderer.positionCount;

        // 遍历 LineRenderer 中的每一对相邻的点
        for (int i = 0; i < positionCount - 1; i++)
        {
            // 获取当前点和下一个点
            Vector3 startPoint = lineRenderer.GetPosition(i);
            Vector3 endPoint = lineRenderer.GetPosition(i + 1);

            // 计算两个点之间的方向和距离
            Vector3 direction = endPoint - startPoint;
            float distance = direction.magnitude;

            // 计算该段距离上需要多少个模型
            int modelCount = Mathf.CeilToInt(distance / modelSpacing);

            // 在每两个点之间生成模型
            for (int j = 0; j < modelCount; j++)
            {
                // 计算每个模型的实际位置
                Vector3 position = startPoint + direction.normalized * j * modelSpacing;

                // 生成模型实例
                GameObject modelInstance = Instantiate(modelPrefab, position, Quaternion.LookRotation(direction));

                // 将模型对齐到线段的方向
                modelInstance.transform.LookAt(endPoint);
            }
        }
    }
}
