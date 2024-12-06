using UnityEngine;

public class UVScroller : MonoBehaviour
{
    // UV滚动速度（X方向与Y方向）
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;

    // 记录材质
    private Renderer rend;
    // 当前的UV偏移量
    private Vector2 currentOffset;

    void Start()
    {
        // 获取当前物体的Renderer
        rend = GetComponent<Renderer>();

        // 获取当前材质的UV偏移起始值（通常为0,0）
        currentOffset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        // 根据时间增量计算偏移量变化
        float offsetX = currentOffset.x + scrollSpeedX * Time.deltaTime;
        float offsetY = currentOffset.y + scrollSpeedY * Time.deltaTime;

        // 将新的偏移值赋回材质
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);

        // 更新currentOffset
        currentOffset = rend.material.mainTextureOffset;
    }
}
