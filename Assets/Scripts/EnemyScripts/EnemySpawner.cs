using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnCheckInterval = 2f;
    public int minEnemies = 2;
    public LayerMask enemyLayer;

    private float nextCheckTime;

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + spawnCheckInterval;

            // Count enemies on layer
            var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            int enemyCount = 0;

            foreach (var obj in allObjects)
            {
                if ((enemyLayer.value & (1 << obj.layer)) != 0)
                    enemyCount++;
            }

            // Spawn if needed
            if (enemyCount < minEnemies)
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}