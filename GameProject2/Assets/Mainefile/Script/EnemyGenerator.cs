using System.Collections;
using UnityEngine;

public class EnemyGenerator : SingletonMonoBehaviour<EnemyGenerator>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField] private GameObject[] enemyPrefabs; // 敵のPrefab
    [SerializeField] private float interval = 3f;       // 生成間隔（秒）
    [SerializeField] private Transform spawnPoint;      // 出現位置（固定）

    void Start()
    {
        StartCoroutine(GenerateEnemy());
    }

    // 一定間隔で敵を生成し続けるコルーチン
    private IEnumerator GenerateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            SpawnEnemy();
        }
    }

    // ランダムな敵を固定位置に生成する
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogWarning("EnemyPrefabsまたはSpawnPointが設定されていません。");
            return;
        }

        // ランダムに敵の種類を選ぶ
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
        Debug.Log(enemyPrefabs[enemyIndex].name + " を生成しました。");
    }
}