using System.Collections;
using System.Collections.Generic; // 确保引用了这个命名空间
using UnityEngine;

public class PotatoPackingLine : MonoBehaviour
{
    public GameObject potatoSlicePrefab; // 薯片预制体
    public GameObject potatoPackagePrefab; // 袋装薯片预制体
    public Transform conveyorBeltStart; // 传送带起点
    public Transform packingStation; // 打包机位置
    public Transform conveyorBeltEnd; // 传送带终点
    public int slicesPerPackage = 20; // 每包薯片的土豆片数量
    public float sliceSpawnDelay = 0.2f; // 每片薯片生成的时间间隔
    public float sliceSpeed = 1.5f; // 薯片沿传送带的移动速度
    public float packageSpeed = 2f; // 袋装薯片沿传送带的移动速度
    public float spawnInterval = 0.5f; // 每片薯片生成的间隔
    public float horizontalRandomRange = 2f; // 横向偏移范围（单位：米）

    private Queue<GameObject> potatoSlices = new Queue<GameObject>(); // 存储待处理的薯片
    private bool isPacking = false; // 控制是否开始打包
    private Coroutine spawnCoroutine; // 存储生成薯片的协程

    void Start()
    {
        // 启动生成薯片的协程
        StartSpawningPotatoSlices();
    }

    // 启动生成薯片的协程
    private void StartSpawningPotatoSlices()
    {
        // 如果已有协程在运行，则先停止它
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        // 启动新的协程来生成薯片
        spawnCoroutine = StartCoroutine(GeneratePotatoSlices());
    }

    // 定时生成薯片，并将它们送入流水线
    private IEnumerator GeneratePotatoSlices()
    {
        while (true)
        {
            // 只有在不在打包状态下生成薯片
            if (!isPacking)
            {
                // 生成一个薯片并将其添加到队列
                GameObject slice = Instantiate(potatoSlicePrefab, conveyorBeltStart.position, Quaternion.identity);
                potatoSlices.Enqueue(slice);

                // 启动协程让薯片沿传送带移动
                StartCoroutine(MovePotatoSliceAlongConveyor(slice));

                // 延迟下一片薯片的生成
                yield return new WaitForSeconds(spawnInterval);
            }

            // 检查是否已生成足够的薯片
            if (potatoSlices.Count >= slicesPerPackage)
            {
                isPacking = true; // 开始打包
                StartCoroutine(PackPotatoSlices()); // 开始打包薯片
            }
        }
    }

    // 让薯片沿传送带移动
    private IEnumerator MovePotatoSliceAlongConveyor(GameObject slice)
    {
        Vector3 targetPosition = packingStation.position; // 薯片的目标位置是打包机位置

        // 薯片沿传送带移动
        while (Vector3.Distance(slice.transform.position, targetPosition) > 0.1f)
        {
            slice.transform.position = Vector3.MoveTowards(slice.transform.position, targetPosition, sliceSpeed * Time.deltaTime);
            yield return null;
        }

        // 薯片到达打包机位置时销毁
        Destroy(slice);
    }

    // 打包薯片
    private IEnumerator PackPotatoSlices()
    {
        // 创建一个新的袋装薯片
        GameObject potatoPackage = Instantiate(potatoPackagePrefab, packingStation.position, Quaternion.identity);

        // 将队列中的所有薯片放入袋装薯片包
        foreach (GameObject slice in potatoSlices)
        {
            slice.transform.parent = potatoPackage.transform; // 设置袋装薯片的父物体
        }

        // 清空薯片队列
        potatoSlices.Clear();

        // 延迟一段时间，模拟打包过程
        yield return new WaitForSeconds(1f);

        // 移动袋装薯片到终点
        StartCoroutine(MovePackageToEnd(potatoPackage));

        // 恢复生成薯片
        isPacking = false;
    }

    // 让袋装薯片沿着传送带移动到终点
    private IEnumerator MovePackageToEnd(GameObject potatoPackage)
    {
        Vector3 targetPosition = conveyorBeltEnd.position; // 袋装薯片的目标位置是传送带末端

        // 生成一个随机的横向偏移量
        float randomXOffset = Random.Range(-horizontalRandomRange, horizontalRandomRange);
        Vector3 randomPosition = new Vector3(conveyorBeltStart.position.x + randomXOffset, conveyorBeltStart.position.y, conveyorBeltStart.position.z);
        
        // 设置袋装薯片的初始位置
        potatoPackage.transform.position = randomPosition;

        // 袋装薯片沿传送带移动
        while (Vector3.Distance(potatoPackage.transform.position, targetPosition) > 0.1f)
        {
            potatoPackage.transform.position = Vector3.MoveTowards(potatoPackage.transform.position, targetPosition, packageSpeed * Time.deltaTime);
            yield return null;
        }

        // 袋装薯片到达终点时销毁
        Destroy(potatoPackage);
    }

    // 更新方法，用于实时调整生成间隔
    public void UpdateSpawnInterval(float newInterval)
    {
        spawnInterval = newInterval;
        StartSpawningPotatoSlices(); // 重新启动生成薯片的协程
    }
}
