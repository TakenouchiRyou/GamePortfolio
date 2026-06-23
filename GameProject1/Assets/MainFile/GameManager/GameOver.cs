using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("設定")]
    public string gameOverSceneName = "Gameover"; // ゲームオーバーシーン名
    public float delay = 3.5f;                    // 遷移までの待機時間

    private bool isChanging = false;

    // 外から呼ぶ関数
    public void TriggerGameOver()
    {
        if (isChanging) return;
        isChanging = true;
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // 少し待ってからシーン遷移
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(gameOverSceneName);
    }
}
