using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleFade : MonoBehaviour
{
    [Header("画像")]
    public Image fadeImage;          // 画面全体を覆う黒いフェード画像
    public GameObject titleImage;    // タイトルロゴ画像

    [Header("ボタン関係")]
    public Button startButton;       // スタートボタン
    public Button exitButton;        // 終了ボタン
    public Button optionButton;      // オプションボタン

    public GameObject confirmPanel;  // 終了確認パネル
    public GameObject settingsPanel; // オプション設定パネル

    public Button yesButton;         // 「はい」ボタン
    public Button noButton;          // 「いいえ」ボタン

    public CanvasGroup buttonGroup;  // ボタン全体の透明度制御用

    [Header("SE")]
    public AudioClip clickSE;        // ボタンクリック音

    [Header("フェード速度移動先シーン")]
    public float fadeSpeed = 1f;     // フェード速度
    public string nextSceneName = "GameScene"; // 遷移先シーン名

    private bool isFadingOut = false; // 二重押し防止フラグ

    // タイトル画面開始時
    IEnumerator Start()
    {
        // 時間を通常に戻す
        Time.timeScale = 1f;

        // フラグ初期化
        isFadingOut = false;

        // タイトル画像を非表示
        if (titleImage != null)
            titleImage.SetActive(false);

        // 確認パネルを非表示
        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        // オプションパネルを非表示
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // ボタンを透明にする
        if (buttonGroup != null)
            buttonGroup.alpha = 0;

        // ボタン操作を無効化
        if (startButton != null)
            startButton.interactable = false;

        if (exitButton != null)
            exitButton.interactable = false;

        // フェード画像を真っ黒にする
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 1;
            fadeImage.color = color;
        }

        // 少し待機
        yield return new WaitForSecondsRealtime(0.5f);

        // タイトル表示
        if (titleImage != null)
            titleImage.SetActive(true);

        // フェードイン処理
        while (fadeImage != null && fadeImage.color.a > 0)
        {
            Color color = fadeImage.color;
            color.a -= fadeSpeed * Time.unscaledDeltaTime;
            fadeImage.color = color;

            yield return null;
        }

        // ボタンを表示
        StartCoroutine(FadeInButton());
    }

    // ボタンを徐々に表示する
    IEnumerator FadeInButton()
    {
        if (buttonGroup == null)
            yield break;

        while (buttonGroup.alpha < 1)
        {
            buttonGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        // ボタン操作を有効化
        if (startButton != null)
            startButton.interactable = true;

        if (exitButton != null)
            exitButton.interactable = true;
    }

    // スタートボタン押下
    public void OnStartButton()
    {
        if (!isFadingOut)
        {
            // ボタン連打防止
            if (startButton != null)
                startButton.interactable = false;

            if (exitButton != null)
                exitButton.interactable = false;

            // クリック音再生
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySE(clickSE);

            // フェードアウト開始
            StartCoroutine(FadeOut());
        }
    }

    // 終了ボタン押下
    public void OnExitButton()
    {
        if (!isFadingOut)
        {
            // クリック音再生
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySE(clickSE);

            // 終了確認パネル表示
            if (confirmPanel != null)
                confirmPanel.SetActive(true);

            // ボタン無効化
            if (startButton != null)
                startButton.interactable = false;

            if (exitButton != null)
                exitButton.interactable = false;
        }
    }

    // オプションボタン押下
    public void OnOptionButton()
    {
        // クリック音再生
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySE(clickSE);

        // オプションパネル表示
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // 「はい」ボタン押下
    public void OnYesButton()
    {
        if (!isFadingOut)
        {
            // ボタン無効化
            if (startButton != null)
                startButton.interactable = false;

            if (exitButton != null)
                exitButton.interactable = false;

            // SE再生
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySE(clickSE);
            }

            // 終了用フェードアウト
            StartCoroutine(FadeOutExit());
        }
    }

    // 「いいえ」ボタン押下
    public void OnNoButton()
    {
        // SE再生
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySE(clickSE);

        // 確認パネルを閉じる
        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        // ボタン再有効化
        if (startButton != null)
            startButton.interactable = true;

        if (exitButton != null)
            exitButton.interactable = true;
    }

    // アプリ終了用フェードアウト
    IEnumerator FadeOutExit()
    {
        isFadingOut = true;

        while (fadeImage != null && fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.unscaledDeltaTime;
            fadeImage.color = color;

            yield return null;
        }

        // ビルド版でゲーム終了
        Application.Quit();

#if UNITY_EDITOR
        // Unityエディタ上では再生停止
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // シーン遷移用フェードアウト
    IEnumerator FadeOut()
    {
        isFadingOut = true;

        while (fadeImage != null && fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.unscaledDeltaTime;
            fadeImage.color = color;

            yield return null;
        }

        // 次のシーンへ移動
        SceneManager.LoadScene(nextSceneName);
    }
}