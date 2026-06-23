using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public string[] clearKeys;

    void Start()
    {
        // 進行度リセット
        foreach (string key in clearKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("染谷/探索画面");
        }
    }
}