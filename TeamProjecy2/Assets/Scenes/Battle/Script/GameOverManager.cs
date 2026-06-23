using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager :
MonoBehaviour
{
    public GameObject
        gameOverPanel;

    // ゲームオーバー表示
    public void ShowGameOver()
    {
        gameOverPanel
            .SetActive(
                true);



        Time.timeScale =
            0;
    }

    // リトライ
    public void Retry()
    {
        // 時間再開
        Time.timeScale =
            1;

        // ラン情報リセット
        GameManager
            .instance
            .ResetRun();

        // セーブ削除
        SaveManager
            .instance
            .DeleteSave();

        // タイトルへ
        SceneManager
            .LoadScene(
                "Select");
    }

    // 終了
    public void QuitGame()
    {
        Application
            .Quit();



        Debug.Log(
            "ゲーム終了");
    }
}