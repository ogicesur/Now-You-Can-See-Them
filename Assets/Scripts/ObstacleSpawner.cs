using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // 预制体
    public float spawnInterval = 2f;  // 障碍物生成的时间间隔
                                      //public Vector2 spawnAreaMin;      // 生成区域的最小边界
                                      // public Vector2 spawnAreaMax;      // 生成区域的最大边界
    public Vector3 moveDirection1 = Vector3.forward; // 障碍物移动方向
    public Vector3 moveDirection2 = Vector3.back;
    public float moveSpeed = 20f; // 障碍物移动速度


    void Start()
    {
        // 开始重复调用 SpawnObstacle 方法
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // 随机选择生成时间
            float randomDelay = Random.Range(1f * spawnInterval, 2f * spawnInterval);
            yield return new WaitForSeconds(randomDelay);

            // 生成障碍物
            Vector3 spawnPosition = new Vector3(
            Random.Range(-3, 3),
            0f,
            Random.Range(10,20)
        );

            // 实例化第一个障碍物并初始化其移动方向
            GameObject obstacle1 = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle1.AddComponent<ObstacleMovement>().Initialize(moveDirection1, moveSpeed);

            // 实例化第二个障碍物并初始化其移动方向
            GameObject obstacle2 = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle2.AddComponent<ObstacleMovement>().Initialize(moveDirection2, moveSpeed);
        }
    }


}
