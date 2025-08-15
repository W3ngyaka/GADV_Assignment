using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnCheckInterval = 2f;
    public int minEnemies = 2;
    public LayerMask enemyLayer;

    private float nextCheckTime;

    // --- Unity lifecycle: delegate multi-line logic to a helper ---
    void Update()
    {
        PerformSpawnCheck();
    }

    // Checks the scene on a fixed interval and spawns an enemy if below the minimum
    private void PerformSpawnCheck()
    {
        // Only run when our interval elapses
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + spawnCheckInterval;

            // Count enemies on the specified layer
            var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            int enemyCount = 0;

            foreach (var obj in allObjects)
            {
                // Bitmask test: is this object on enemyLayer?
                if ((enemyLayer.value & (1 << obj.layer)) != 0)
                    enemyCount++;
            }

            // Spawn a new enemy at this spawner's position if we're under the minimum
            if (enemyCount < minEnemies)
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
