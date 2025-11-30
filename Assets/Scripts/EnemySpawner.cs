using UnityEngine;

public class EnemySpawner2D : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 10f;
    public int maxEnemies = 50;

    private float timer;
    private int count;

    void Update()
    {
        if (GameManager2D.Instance.IsGameOver) return;

        timer -= Time.deltaTime;

        if (timer <= 0f && count < maxEnemies)
        {
            timer = spawnInterval;
            count++;

            Vector2 pos = Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }
}
