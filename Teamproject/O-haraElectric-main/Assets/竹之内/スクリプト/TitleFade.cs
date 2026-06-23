using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleFade : MonoBehaviour
{
    [Header("画像")]
    public Image fadeImage;          // 画面全体を覆う黒フェード用イメージ
    public GameObject titleImage;    // タイトル画像

    [Header("ボタン関係")]
    public Button startButton;       // スタートボタン
    public Button exitButton;        // 終了ボタン
    public GameObject confirmPanel;
    public Button yesButton;
    public Button noButton;
    public CanvasGroup buttonGroup;  // ボタンのフェード制御用

    [Header("BGM and SE")]
    public AudioSource audioSource;  // SE再生用
    public AudioClip clickSE;        // クリック音

    [Header("フェード速度移動先シーン")]
    public float fadeSpeed = 1f;     // フェード速度
    public string nextSceneName = "GameScene"; // 遷移先シーン名

    private bool isFadingOut = false; // 二重押し防止

    IEnumerator Start()
    {

        // タイトル画像は最初非表示
        titleImage.SetActive(false);
        //　二段終了画面のフラグ
        confirmPanel.SetActive(false);

        // ボタンは最初透明＆押せない
        buttonGroup.alpha = 0;
        startButton.interactable = false;
        exitButton.interactable = false;

        // 画面を黒でスタート
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        // 少し待ってからタイトル表示
        yield return new WaitForSeconds(0.5f);

        // タイトル画像を表示
        titleImage.SetActive(true);

        // 黒フェードアウト（徐々に透明に）
        while (fadeImage.color.a > 0)
        {
            color = fadeImage.color;
            color.a -= fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        // ボタンフェードイン開始
        StartCoroutine(FadeInButton());
    }

    IEnumerator FadeInButton()
    {
        // ボタンを徐々に表示
        while (buttonGroup.alpha < 1)
        {
            buttonGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        // 完全に表示されたら押せるようにする
        startButton.interactable = true;
        exitButton.interactable = true;
    }

    public void OnStartButton()
    {
        // フェードアウト中は押せない
        if (!isFadingOut)
        {
            startButton.interactable = false;
            exitButton.interactable = false; 

            // クリック音再生
            audioSource.PlayOneShot(clickSE);

            // シーン遷移フェード開始
            StartCoroutine(FadeOut());
        }
    }
    public void OnExitButton()
    {
        if (!isFadingOut)
        {
            if (audioSource && clickSE)
                audioSource.PlayOneShot(clickSE);

            // 確認画面を出す
            confirmPanel.SetActive(true);

            // 元のボタンを押せなくする
            startButton.interactable = false;
            exitButton.interactable = false;
        }
    }

    public void OnYesButton()
    {
        if (!isFadingOut)
        {
            startButton.interactable = false;
            exitButton.interactable = false;

            audioSource.PlayOneShot(clickSE);

            StartCoroutine(FadeOutExit());
        }
    }

    public void OnNoButton()
    {
        if (audioSource && clickSE)
            audioSource.PlayOneShot(clickSE);

        confirmPanel.SetActive(false);

        // 元に戻す
        startButton.interactable = true;
        exitButton.interactable = true;
    }

    IEnumerator FadeOutExit()
    {
        isFadingOut = true;

        while (fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    IEnumerator FadeOut()
    {
        isFadingOut = true;

        // 黒フェードイン（徐々に暗く）
        while (fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        // シーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}
