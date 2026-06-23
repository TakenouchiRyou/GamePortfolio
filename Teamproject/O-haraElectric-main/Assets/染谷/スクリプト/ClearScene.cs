using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScene : MonoBehaviour
{
    public string nextScene = "’Tõ‰æ–Ê"; // ƒV[ƒ“–¼‚ğ‡‚í‚¹‚Ä‚­‚¾‚³‚¢

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}