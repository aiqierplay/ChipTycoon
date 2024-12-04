using UnityEngine;

public class LineRendererTiling : MonoBehaviour
{
    public LineRenderer lineRenderer;  // 你的LineRenderer
    public float textureScale = 1f;  // 控制纹理的平铺比例

    void Update()
    {
        // 计算LineRenderer的长度
        float totalLength = 0f;
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            totalLength += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }

        // 根据线的长度动态调整纹理平铺
        Vector2 tiling = new Vector2(totalLength * textureScale, 1f);  // 只在X轴上调整平铺
        lineRenderer.material.SetTextureScale("_MainTex", tiling);
    }
}
