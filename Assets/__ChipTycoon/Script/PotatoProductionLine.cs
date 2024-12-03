using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; // 保留 System 命名空间
using Random = UnityEngine.Random; // 将 Random 设为 UnityEngine.Random

public class PotatoProductionLine : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject potatoPrefab; // 土豆预制体
    public GameObject potatoSlicePrefab; // 土豆片预制体

    [Header("Conveyor Belt Settings")]
    public Transform conveyorBeltStart; // 传送带起点
    public Transform conveyorBeltEnd; // 传送带终点
    public Transform machinePosition; // 机器位置
    public float conveyorSpeed = 2f; // 传送带速度
    public float potatoSliceSpeed = 1.5f; // 土豆片沿传送带的移动速度

    [Header("Slice Generation Settings")]
    public float sliceSpawnDelay = 0.2f; // 每片土豆片生成的延迟
    public int output = 10; // 输出的土豆片数量

    [Header("Spawning Settings")]
    public float spawnInterval = 2f; // 每组土豆进入传送带的时间间隔
    public int input = 1; // 输入的土豆数量
    public Vector2 spawnAreaSize = new Vector2(1f, 1f); // 生成区域大小

    private bool isRunning = true; // 控制土豆生成是否继续
    private Coroutine spawnCoroutine; // 保存生成土豆的协程引用

    void Start()
    {
        // 开始定时生成土豆
        Debug.Log("生产线启动，开始生成土豆。");
        spawnCoroutine = StartCoroutine(SpawnPotatoes());
    }

    void Update()
    {
        // 通过按下空格键来暂停或恢复土豆生成
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = !isRunning; // 切换生成状态
            if (isRunning)
            {
                // 恢复生成土豆
                Debug.Log("生产线恢复运行。");
                if (spawnCoroutine == null)
                {
                    spawnCoroutine = StartCoroutine(SpawnPotatoes());
                }
            }
            else
            {
                // 停止生成土豆
                Debug.Log("生产线暂停。");
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }
            }
        }
    }

    // 定时生成土豆并开始处理
    private IEnumerator SpawnPotatoes()
    {
        while (isRunning)
        {
            Debug.Log($"开始生成一组土豆，数量：{input}");
            int groupArrived = 0; // 当前组到达的土豆数量

            // 生成 input 个土豆
            for (int i = 0; i < input; i++)
            {
                // 获取随机生成位置
                Vector3 spawnPosition = GetRandomSpawnPosition();

                GameObject potato = Instantiate(potatoPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"生成土豆 #{i + 1}，位置：{spawnPosition}");

                // 开始将土豆从起点移动到机器，并传递回调
                StartCoroutine(MovePotatoAlongConveyor(potato, () =>
                {
                    groupArrived++;
                    Debug.Log($"土豆到达机器位置。当前到达数量：{groupArrived}/{input}");
                }));
            }

            // 等待当前组所有土豆到达
            Debug.Log("等待当前组所有土豆到达机器位置...");
            yield return new WaitUntil(() => groupArrived >= input);
            Debug.Log("所有土豆已到达，开始生成土豆片。");

            // 生成土豆片
            StartCoroutine(GeneratePotatoSlices());

            // 延迟一段时间再生成下一个土豆组
            Debug.Log($"等待 {spawnInterval} 秒后生成下一组土豆。");
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 获取一个不与现有土豆重叠的随机生成位置
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        bool isOverlapping;
        int maxAttempts = 10; // 最大尝试次数，防止无限循环

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
            );
            spawnPosition = conveyorBeltStart.position + randomOffset;

            // 检测是否与现有土豆重叠
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, potatoPrefab.GetComponent<Collider>().bounds.size.x / 2);
            if (colliders.Length == 0)
            {
                return spawnPosition;
            }
            else
            {
                isOverlapping = true;
                Debug.LogWarning("生成位置与现有土豆重叠，重新生成位置。");
            }
        }

        // 如果尝试多次仍然重叠，返回最后一个位置
        Debug.LogWarning("达到最大尝试次数，返回最后一个生成位置。");
        return conveyorBeltStart.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );
    }

    // 移动土豆沿传送带，并在到达后调用回调
    private IEnumerator MovePotatoAlongConveyor(GameObject potato, Action onArrive)
    {
        Vector3 targetPosition = machinePosition.position; // 机器位置是土豆的目标位置
        Debug.Log($"土豆开始移动到机器位置，目标位置：{targetPosition}");

        // 土豆沿传送带移动
        while (Vector3.Distance(potato.transform.position, targetPosition) > 0.1f)
        {
            potato.transform.position = Vector3.MoveTowards(potato.transform.position, targetPosition, conveyorSpeed * Time.deltaTime);
            yield return null;
        }

        // 土豆到达机器位置，消失
        Debug.Log($"土豆到达目标位置，销毁土豆对象。");
        Destroy(potato);

        // 调用回调，通知土豆已到达
        onArrive?.Invoke();
    }

    // 生成土豆片
    private IEnumerator GeneratePotatoSlices()
    {
        Debug.Log($"开始生成 {output} 片土豆片。");
        // 根据 output 生成土豆片
        for (int i = 0; i < output; i++)
        {
            // 生成土豆片，并将其放置在机器位置
            GameObject slice = Instantiate(potatoSlicePrefab, machinePosition.position, Quaternion.identity);
            Debug.Log($"生成土豆片 #{i + 1}，位置：{machinePosition.position}");

            // 启动协程让土豆片沿着传送带移动
            StartCoroutine(MovePotatoSliceAlongConveyor(slice));

            // 延迟下一片土豆片的生成
            yield return new WaitForSeconds(sliceSpawnDelay);
        }
    }

    // 让土豆片沿着传送带移动
    private IEnumerator MovePotatoSliceAlongConveyor(GameObject slice)
    {
        Vector3 targetPosition = conveyorBeltEnd.position; // 土豆片的目标位置是传送带末端
        Debug.Log($"土豆片开始移动到传送带末端，目标位置：{targetPosition}");

        // 将土豆片沿着传送带移动
        while (Vector3.Distance(slice.transform.position, targetPosition) > 0.1f)
        {
            slice.transform.position = Vector3.MoveTowards(slice.transform.position, targetPosition, potatoSliceSpeed * Time.deltaTime);
            yield return null;
        }

        // 土豆片到达传送带末端时销毁
        Debug.Log($"土豆片到达传送带末端，销毁土豆片对象。");
        Destroy(slice);
    }
}
