using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    public Transform[] spawnPoints;
    public float despawnTime = 10f;

    private MapBGMManager bgmManager;

    void Start()
    {
        bgmManager = FindObjectOfType<MapBGMManager>();
        enemy.SetActive(false);

        if (PlayerPrefs.GetInt("TimeOverGameOver", 0) == 1)
        {
            PlayerPrefs.DeleteKey("TimeOverGameOver");
            PlayerPrefs.Save();
            SpawnImmediately();
        }
        else if (PlayerPrefs.GetInt("EnemySpawnOnReturn", 0) == 1)
        {
            PlayerPrefs.DeleteKey("EnemySpawnOnReturn");
            PlayerPrefs.Save();
            SpawnImmediately();
        }
        else
        {
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("SpawnEnemy", spawnTime);
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        enemy.transform.position = spawnPos;
        enemy.SetActive(true);

        if (bgmManager != null)
            bgmManager.PlayChaseBGM();

        StartCoroutine(AutoDespawn());
    }

    public void SpawnImmediately()
    {
        CancelInvoke("SpawnEnemy");
        SpawnEnemy();
    }

    public void DespawnEnemy()
    {
        StopCoroutine(AutoDespawn());
        enemy.SetActive(false);
        ScheduleNextSpawn();

        if (bgmManager != null)
            bgmManager.PlayExploreBGM();
    }

    IEnumerator AutoDespawn()
    {
        yield return new WaitForSeconds(despawnTime);
        if (enemy.activeSelf)
        {
            enemy.SetActive(false);
            ScheduleNextSpawn();

            if (bgmManager != null)
                bgmManager.PlayExploreBGM();

            PlayerPrefs.SetInt("WiringReset", 1);
            PlayerPrefs.DeleteKey("WiringTimeLeft");
            PlayerPrefs.Save();
        }
    }
}