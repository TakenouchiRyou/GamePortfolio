using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // Ÿ‚¿
    public void GameClear()
    {
        Debug.Log("Ÿ—˜I");
        Time.timeScale = 0f; // ƒQ[ƒ€’â~
    }

    // •‰‚¯
    public void GameOver()
    {
        Debug.Log("”s–k...");
        Time.timeScale = 0f; // ƒQ[ƒ€’â~
    }
}