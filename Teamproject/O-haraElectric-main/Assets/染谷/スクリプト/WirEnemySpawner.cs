using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WirEnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    public Transform[] spawnPoints;
    public float despawnTime = 10f; // EnemyoŒ»Œã‰½•b‚ÅÁ‚¦‚é‚©

    private WiringManager wiringManager;

    void Start()
    {
        wiringManager = FindObjectOfType<WiringManager>();
        enemy.SetActive(false);

        if (PlayerPrefs.GetInt("TimeOverGameOver", 0) == 1)
        {
            PlayerPrefs.DeleteKey("TimeOverGameOver");
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

        // ”züC—‚ğˆê’â~
        if (wiringManager != null)
            wiringManager.SetPlaying(false);

        // ’Tõ‰æ–Ê‚É–ß‚é
        SceneManager.LoadScene("õ’J/’Tõ‰æ–Ê");
    }

    public void DespawnEnemy()
    {
        enemy.SetActive(false);
        // ”züC—‚ğÄŠJ
        if (wiringManager != null)
            wiringManager.SetPlaying(true);

        ScheduleNextSpawn();
    }

    public void SpawnImmediately()
    {
        CancelInvoke("SpawnEnemy");
        SpawnEnemy();
    }

    // ˆê’èŠÔŒã‚É©“®‚ÅEnemy‚ğÁ‚·
    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(despawnTime);
        if (enemy.activeSelf)
            DespawnEnemy();
    }
}