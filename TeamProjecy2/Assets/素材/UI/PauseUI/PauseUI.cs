using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [Header("ポーズ画面（MainMenuを子に持つ）")]
    [SerializeField] private GameObject pausePanel;

    [Header("操作説明パネル")]
    [SerializeField] private GameObject controlPanel;

    [Header("確認パネル")]
    [SerializeField] private GameObject confirmPanel;

    [Header("タイトルシーン名")]
    public string titleSceneName = "Title";

    [Header("SE")]
    [SerializeField] private AudioClip clickSE;

    public static bool isPaused = false;

    private bool isOpen = false;

    // true = タイトルへ戻る
    // false = ゲーム終了
    private bool returnToTitle = false;

    void Start()
    {
        pausePanel.SetActive(false);
        controlPanel.SetActive(false);
        confirmPanel.SetActive(false);

        isOpen = false;
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 操作説明が開いていたら閉じる
            if (controlPanel.activeSelf)
            {
                controlPanel.SetActive(false);
                return;
            }

            // 確認画面が開いていたら閉じる
            if (confirmPanel.activeSelf)
            {
                confirmPanel.SetActive(false);
                return;
            }

            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;

        pausePanel.SetActive(isOpen);

        Time.timeScale = isOpen ? 0f : 1f;
        isPaused = isOpen;
    }

    public void OpenPause()
    {
        isOpen = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ClosePause()
    {
        isOpen = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // 操作説明
 
    public void OnControlButton()
    {
        PlaySE();
        controlPanel.SetActive(true);
    }

    public void OnControlBackButton()
    {
        PlaySE();
        controlPanel.SetActive(false);
    }

    // タイトルへ戻る
 
    public void OnTitleButton()
    {
        PlaySE();

        returnToTitle = true;
        confirmPanel.SetActive(true);
    }

    // ゲーム終了

    public void OnQuitButton()
    {
        PlaySE();

        returnToTitle = false;
        confirmPanel.SetActive(true);
    }

    // YES

    public void OnYesButton()
    {
        PlaySE();

        if (returnToTitle)
        {
            StartCoroutine(ReturnTitleAfterSE());
        }
        else
        {
            StartCoroutine(QuitAfterSE());
        }
    }

    // NO

    public void OnNoButton()
    {
        PlaySE();
        confirmPanel.SetActive(false);
    }

    // タイトルへ戻る処理

    private IEnumerator ReturnTitleAfterSE()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isOpen = false;

        if (clickSE != null)
        {
            yield return new WaitForSecondsRealtime(clickSE.length);
        }

        SceneManager.LoadScene(titleSceneName);
    }

    // ゲーム終了処理

    private IEnumerator QuitAfterSE()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isOpen = false;

        if (clickSE != null)
        {
            yield return new WaitForSecondsRealtime(clickSE.length);
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 効果音
    public void PlaySE()
    {
        if (AudioManager.Instance != null && clickSE != null)
        {
            AudioManager.Instance.PlaySE(clickSE);
        }
    }
}