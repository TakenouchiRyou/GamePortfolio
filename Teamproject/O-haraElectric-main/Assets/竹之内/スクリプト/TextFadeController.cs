using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextFadeController : MonoBehaviour
{
    [Header("TextMeshProを入れる")]
    public TextMeshProUGUI textUI;

    [Header("透明度をいじる")]
    public CanvasGroup canvasGroup;

    [Header("文章を入れる")]
    [TextArea(3, 10)]
    public string[] messages;

    [Header("フェードインさせる時間")]
    public float fadeDuration = 1f;

    [Header("テキストを表示する時間")]
    public float displayTime = 2f;

    [Header("次のシーン名")]
    public string nextSceneName = "探索";

    [Header("シーン遷移までの待機時間")]
    public float sceneTransitionDelay = 1f;

    [Header("スキップメッセージ用TextMeshPro(右下)")]
    public TextMeshProUGUI skipTextUI;

    [Header("スキップ促進メッセージ")]
    public string skipMessage = "Enterキーでスキップできます";

    [Header("スキップメッセージが消えるまでの時間")]
    public float skipMessageTimeout = 3f;

    private bool showingSkipMessage = false; // Enterでスキップできる状態か
    private bool isSkipped = false;          // スキップ確定
    private bool isDisplayingText = false;   // 文章表示中か
    private float skipMessageTimer = 0f;

    void Start()
    {
        if (skipTextUI != null)
        {
            skipTextUI.text = skipMessage;
            skipTextUI.gameObject.SetActive(false);
        }

        StartCoroutine(PlayTextSequence());
    }

    void Update()
    {
        // 文章表示中に何かキーを押したらスキップメッセージ表示＆タイマーリセット
        if (Input.anyKeyDown && isDisplayingText && !showingSkipMessage)
        {
            showingSkipMessage = true;
            skipMessageTimer = 0f;
            if (skipTextUI != null)
                skipTextUI.gameObject.SetActive(true);
            return; // このフレームはEnter判定しない
        }

        // スキップメッセージ表示中のタイマー（文章表示中のみ）
        if (showingSkipMessage && isDisplayingText)
        {
            skipMessageTimer += Time.deltaTime;

            if (skipMessageTimer >= skipMessageTimeout)
            {
                showingSkipMessage = false;
                skipMessageTimer = 0f;
                if (skipTextUI != null)
                    skipTextUI.gameObject.SetActive(false);
            }
        }

        // showingSkipMessage(フラグ)がある状態でEnter → スキップ確定
        if (showingSkipMessage && Input.GetKeyDown(KeyCode.Return))
        {
            isSkipped = true;
        }
    }

    IEnumerator PlayTextSequence()
    {
        foreach (string msg in messages)
        {
            if (isSkipped) break;

            textUI.text = msg;
            isDisplayingText = true;
            yield return StartCoroutine(Fade(0f, 1f));

            float timer = 0f;
            while (timer < displayTime)
            {
                if (isSkipped) break;
                timer += Time.deltaTime;
                yield return null;
            }

            // 文章終わり → フラグをリセット
            isDisplayingText = false;
            showingSkipMessage = false;
            if (skipTextUI != null)
                skipTextUI.gameObject.SetActive(false);

            if (isSkipped) break;
            yield return StartCoroutine(Fade(1f, 0f));
        }

        if (skipTextUI != null)
            skipTextUI.gameObject.SetActive(false);

        isSkipped = false;
        showingSkipMessage = false;
        isDisplayingText = false;

        float waitTimer = 0f;
        while (waitTimer < sceneTransitionDelay)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(nextSceneName);
                yield break;
            }
            waitTimer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}