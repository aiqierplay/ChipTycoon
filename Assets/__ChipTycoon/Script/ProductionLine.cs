using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionLine : MonoBehaviour
{
    public GameObject inputProductPrefab; // 输入产品预制体
    public GameObject outputProductPrefab; // 输出产品预制体
    public Transform conveyorBeltStart; // 传送带起点
    public Transform conveyorBeltEnd; // 传送带终点
    public Transform machinePosition; // 机器位置
    public float conveyorSpeed = 2f; // 传送带速度
    public float productSpawnDelay = 0.2f; // 每个输出产品生成的延迟
    public float outputProductSpeed = 1.5f; // 输出产品沿传送带的移动速度
    public float spawnInterval = 2f; // 每个输入产品进入传送带的时间间隔

    public int input = 5; // 输入产品数量
    public int output = 10; // 输出产品数量

    [Header("Spawn Area Settings")]
    public Vector2 spawnAreaSize = new Vector2(1f, 1f); // 范围宽度和高度

    private bool isRunning = true; // 控制产品生成是否继续
    private Coroutine spawnCoroutine; // 保存生成输入产品的协程引用
    private int currentInputCount = 0; // 当前到达机器位置的输入产品数量

    void Start()
    {
        // 启动新的协程之前，确保先停止旧的协程
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        // 开始定时生成输入产品
        spawnCoroutine = StartCoroutine(SpawnInputProducts());
    }

    void Update()
    {
        // 通过按下空格键来暂停或恢复输入产品生成
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = !isRunning; // 切换生成状态
            if (isRunning)
            {
                // 恢复生成输入产品
                spawnCoroutine = StartCoroutine(SpawnInputProducts());
            }
            else
            {
                // 停止生成输入产品
                StopCoroutine(spawnCoroutine);
            }
        }
    }

    // 定时生成输入产品并开始处理
    private IEnumerator SpawnInputProducts()
    {
        while (isRunning)
        {
            for (int i = 0; i < input; i++) // 生成 input 个输入产品
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                // 生成一个新的输入产品
                GameObject inputProduct = Instantiate(inputProductPrefab, spawnPosition, Quaternion.identity);

                // 开始将输入产品从起点移动到机器
                StartCoroutine(MoveProductAlongConveyor(inputProduct));
            }

            // 等待输入产品达到机器位置并且数量达到设定的输入量后再生成输出产品
            yield return new WaitUntil(() => currentInputCount >= input);

            // 生成输出产品
            StartCoroutine(GenerateOutputProducts());

            // 重置输入产品计数
            currentInputCount = 0;

            // 延迟一段时间再生成下一个输入产品组
            yield return new WaitForSeconds(spawnInterval); // 这里应该是最新的 spawnInterval
        }
    }

    // 计算随机生成的位置
    private Vector3 GetRandomSpawnPosition()
    {
        // 生成一个在 spawnAreaSize 范围内的随机偏移
        float offsetX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float offsetY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        // 假设传送带是水平的，调整偏移轴根据你的实际情况
        return conveyorBeltStart.position + new Vector3(offsetX, 0, offsetY);
    }

    private IEnumerator MoveProductAlongConveyor(GameObject product)
    {
        Vector3 targetPosition = machinePosition.position; // 机器位置是输入产品的目标位置

        // 输入产品沿传送带移动
        while (Vector3.Distance(product.transform.position, targetPosition) > 0.1f)
        {
            product.transform.position = Vector3.MoveTowards(product.transform.position, targetPosition, conveyorSpeed * Time.deltaTime);
            yield return null;
        }

        // 输入产品到达机器位置，增加到达计数
        currentInputCount++;

        // 输入产品到达机器位置，销毁
        Destroy(product);
    }

    private IEnumerator GenerateOutputProducts()
    {
        // 根据 output 生成输出产品
        for (int i = 0; i < output; i++)
        {
            // 生成输出产品，并将其放置在机器位置
            GameObject outputProduct = Instantiate(outputProductPrefab, machinePosition.position, Quaternion.identity);
            // 启动协程让输出产品沿着传送带移动
            StartCoroutine(MoveOutputProductAlongConveyor(outputProduct));

            // 延迟下一件输出产品的生成
            yield return new WaitForSeconds(productSpawnDelay);
        }
    }

    // 让输出产品沿着传送带移动
    private IEnumerator MoveOutputProductAlongConveyor(GameObject product)
    {
        Vector3 targetPosition = conveyorBeltEnd.position; // 输出产品的目标位置是传送带末端

        // 将输出产品沿着传送带移动
        while (Vector3.Distance(product.transform.position, targetPosition) > 0.1f)
        {
            product.transform.position = Vector3.MoveTowards(product.transform.position, targetPosition, outputProductSpeed * Time.deltaTime);
            yield return null;
        }

        // 输出产品到达传送带末端时销毁
        Destroy(product);
    }
}
