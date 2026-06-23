using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button settingButton;
    public Button backButton;
    private bool settingsOpen = false;

    void Start()
    {
        settingButton.onClick.AddListener(OnSettingsButton);
        backButton.onClick.AddListener(OnBackButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsOpen)
            {
                Scene settingsScene = SceneManager.GetSceneByName("設定画面");
                if (settingsScene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(settingsScene);
                }
                settingsOpen = false;
            }
            else
            {
                bool isOpen = pausePanel.activeSelf;
                pausePanel.SetActive(!isOpen);
                Time.timeScale = isOpen ? 1f : 0f;
            }
        }
    }

    public void OnSettingsButton()
    {
        if (!settingsOpen)
        {
            pausePanel.SetActive(false);
            SceneManager.LoadSceneAsync("染谷/設定画面", LoadSceneMode.Additive);
            settingsOpen = true;
        }
    }

    public void OnBackButton()
    {
        Debug.Log("BackButton押された");
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "設定画面")
        {
            settingsOpen = false;
            pausePanel.SetActive(true); // ポーズ画面を表示
        }
    }
}