using UnityEngine;
using System.Collections; // 需要使用 IEnumerator

public class SpawnLotsCoin : MonoBehaviour
{
    public GameObject coinPrefab; // 要生成的金币预制体
    public float totalSpawnAngle = 120f; // 发射的总角度范围 (例如120度，表示向上方左右各60度)
    public float launchForce = 10f;     // 发射金币的力度
    public float spawnInterval = 0.2f;  // 每隔多少秒发射一个金币
    public Transform spawnPoint;        // 金币的生成点 (可选, 如果不设置，则使用此脚本所在物体的位置)

    void Start()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin Prefab has not been assigned in SpawnLotsCoin script!");
            enabled = false; // 禁用此脚本，因为没有东西可以生成
            return;
        }

        if (spawnPoint == null)
        {
            spawnPoint = transform; // 如果没有指定生成点，就用当前物体的位置
        }

        // 启动生成协程
        StartCoroutine(SpawnCoinRoutine());
        Destroy(gameObject, 5f);
    }

    IEnumerator SpawnCoinRoutine()
    {
        // 无限循环，持续生成金币
        while (true)
        {
            SpawnSingleCoin();
            yield return new WaitForSeconds(spawnInterval); // 等待指定间隔
        }
    }

    void SpawnSingleCoin()
    {
        // 1. 计算随机角度
        // totalSpawnAngle 是总的锥形角度，所以我们需要取其一半作为正负偏移量
        float halfAngle = totalSpawnAngle / 2f;
        // 在 [-halfAngle, +halfAngle] 范围内随机取一个角度
        // 这个角度是相对于“正上方”的偏移
        float randomAngleOffset = Random.Range(-halfAngle, halfAngle);

        // 2. 计算发射方向
        // Vector2.up 代表世界坐标的正上方 (0, 1)
        // 如果你希望“上方”是相对于当前GameObject的上方，可以使用 transform.up
        Vector2 baseDirection = Vector2.up; // 或者 transform.up

        // 使用 Quaternion 来旋转基础方向向量
        // Quaternion.Euler(0, 0, angle) 会创建一个绕Z轴旋转指定角度的四元数
        // 对于2D，我们通常绕Z轴旋转
        Quaternion rotation = Quaternion.Euler(0, 0, randomAngleOffset);
        Vector2 launchDirection = rotation * baseDirection; // 将旋转应用到基础方向上

        // 3. 实例化金币
        // Quaternion.identity 表示无旋转，如果你的金币预制体本身就有特定朝向，或者你想让它朝向发射方向，可以调整
        GameObject newCoin = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);

        // 4. 获取金币的 Rigidbody2D 并施加力
        Rigidbody2D rb = newCoin.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 使用 AddForce 给金币一个初始的冲量
            rb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Spawned coin prefab '" + coinPrefab.name + "' does not have a Rigidbody2D component attached. Cannot apply force.");
        }
    }

    // 可选：在编辑器中绘制发射角度范围辅助线，方便调试
    void OnDrawGizmosSelected()
    {
        if (spawnPoint == null && transform != null)
        {
            spawnPoint = transform;
        }
        if (spawnPoint == null) return;


        float halfAngle = totalSpawnAngle / 2f;
        Vector2 baseDirection = Vector2.up; // 或者 transform.up (如果想可视化相对方向)
                                            // 注意：如果在运行时 transform.up 改变，这里的 Gizmo 也会改变

        Quaternion leftRotation = Quaternion.Euler(0, 0, halfAngle);
        Quaternion rightRotation = Quaternion.Euler(0, 0, -halfAngle);

        Vector2 leftLimit = leftRotation * baseDirection;
        Vector2 rightLimit = rightRotation * baseDirection;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + (Vector3)leftLimit * 3); // 乘以3仅为让线长一点
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + (Vector3)rightLimit * 3);
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + (Vector3)baseDirection * 3); // 中心线

        // 绘制一个扇形辅助线
        // UnityEditor.Handles.color = new Color(0, 1, 1, 0.1f); // Gizmos没有直接画扇形的方法，Handles可以但只在Editor脚本里
        // UnityEditor.Handles.DrawSolidArc(spawnPoint.position, Vector3.forward, rightLimit, totalSpawnAngle, 3f);
        // 上面两行需要在 using UnityEditor; 且通常在 OnDrawGizmosSelected 中可能无法直接使用 Handles.DrawSolidArc
        // 简单的 Gizmos 替代：
        int segments = 20;
        Vector3 prevPoint = spawnPoint.position + (Vector3)rightLimit * 3;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (totalSpawnAngle * i / segments);
            Quaternion currentRotation = Quaternion.Euler(0, 0, currentAngle);
            Vector3 currentPoint = spawnPoint.position + (Vector3)(currentRotation * baseDirection) * 3;
            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }
    }
}