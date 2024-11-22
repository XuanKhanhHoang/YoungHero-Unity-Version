using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemyPrefab;
    public Vector2[] spawnPoints;
    public float respawnCooldown = 10.0f;
    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnemy(spawnPoints[i]);
        }
    }

    void SpawnEnemy(Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemies.Add(newEnemy);
        newEnemy.GetComponent<Enemy>().OnEnemyDeath += () =>
        {
            enemies.Remove(newEnemy);
            Destroy(newEnemy);
            StartCoroutine(RespawnEnemy(position));
        };
    }
    private void Update()
    {
        foreach (var item in enemies)
        {
            item.SetActive(!IsOutOfView(item.transform.position));
        }
    }
    IEnumerator RespawnEnemy(Vector3 position)
    {
        yield return new WaitForSeconds(respawnCooldown);

        if (IsOutOfView(position))
        {
            SpawnEnemy(position);
        }
        else
        {
            yield return new WaitUntil(() => IsOutOfView(position));
            SpawnEnemy(position);
        }
    }

    bool IsOutOfView(Vector3 position)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }
}
