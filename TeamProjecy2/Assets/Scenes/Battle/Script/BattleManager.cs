using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerLose()
    {
        Debug.Log("ゲームオーバー");

        // 負け演出
        FindObjectOfType<
        GameOverManager>()
        .ShowGameOver();
    }

    public void PlayerWin()
    {
        Debug.Log("勝利");
    }
}