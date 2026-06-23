using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [Header("AudioMixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider heartbeatSlider;

    [Header("Toggle")]
    public Toggle screenShakeToggle;

    [Header("Texts")]
    public TMP_Text bgmText;
    public TMP_Text seText;
    public TMP_Text heartbeatText;

    [Header("Panels")]
    public GameObject settingPanel;
    public GameObject keyBindPanel;
    public GameObject confirmPanel;

    [Header("確認パネル")]
    public Text confirmText;

    [Header("フェード")]
    public Image fadeImage;
    public float fadeSpeed = 1f;

    public string BackScene;

    private bool isFadingOut = false;
    private bool isQuitting = false;

    void Start()
    {
        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 10;
        bgmSlider.wholeNumbers = true;

        seSlider.minValue = 0;
        seSlider.maxValue = 10;
        seSlider.wholeNumbers = true;

        heartbeatSlider.minValue = 0;
        heartbeatSlider.maxValue = 10;
        heartbeatSlider.wholeNumbers = true;

        confirmPanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);

        LoadSettings();
    }

    void ApplyVolume(string paramName, float value)
    {
        if (value == 0)
            audioMixer.SetFloat(paramName, -80f);
        else
            audioMixer.SetFloat(paramName, Mathf.Log10(value / 10f) * 20f);
    }

    public void OnBGMChanged(float value)
    {
        PlayerPrefs.SetFloat("BGMVolume", value);
        ApplyVolume("BGMVolume", value);
        bgmText.text = "BGM " + ((int)value).ToString();
    }

    public void OnSEChanged(float value)
    {
        PlayerPrefs.SetFloat("SEVolume", value);
        ApplyVolume("SEVolume", value);
        seText.text = "SE " + ((int)value).ToString();
    }

    public void OnHeartbeatChanged(float value)
    {
        PlayerPrefs.SetFloat("HeartbeatVolume", value);
        ApplyVolume("HeartbeatVolume", value);
        heartbeatText.text = "HeartBeat " + ((int)value).ToString();
    }

    void LoadSettings()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 8f);
        float se = PlayerPrefs.GetFloat("SEVolume", 8f);
        float hb = PlayerPrefs.GetFloat("HeartbeatVolume", 8f);
        int shake = PlayerPrefs.GetInt("ScreenShake", 1);

        bgmSlider.onValueChanged.RemoveAllListeners();
        seSlider.onValueChanged.RemoveAllListeners();
        heartbeatSlider.onValueChanged.RemoveAllListeners();

        bgmSlider.value = bgm;
        seSlider.value = se;
        heartbeatSlider.value = hb;
        screenShakeToggle.isOn = shake == 1;

        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        seSlider.onValueChanged.AddListener(OnSEChanged);
        heartbeatSlider.onValueChanged.AddListener(OnHeartbeatChanged);

        bgmText.text = "BGM " + ((int)bgm).ToString();
        seText.text = "SE " + ((int)se).ToString();
        heartbeatText.text = "HeartBeat " + ((int)hb).ToString();

        ApplyVolume("BGMVolume", bgm);
        ApplyVolume("SEVolume", se);
        ApplyVolume("HeartbeatVolume", hb);
    }

    // KeyBindボタン
    public void OnKeyBindButton()
    {
        settingPanel.SetActive(false);
        keyBindPanel.SetActive(true);
    }

    // KeyBind戻るボタン
    public void OnKeyBindBackButton()
    {
        keyBindPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    // 戻るボタン
    public void OnBackButton()
    {
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        Scene scene = SceneManager.GetSceneByName("設定画面");
        SceneManager.UnloadSceneAsync(scene.buildIndex);
    }

    // タイトルに戻るボタン
    public void OnTitleButton()
    {
        if (!isFadingOut)
        {
            isQuitting = false;
            confirmText.text = "セーブしていないデータは消えますが\nよろしいですか？";
            confirmPanel.SetActive(true);
        }
    }

    // ゲームを終了ボタン
    public void OnQuitButton()
    {
        if (!isFadingOut)
        {
            isQuitting = true;
            confirmText.text = "セーブしていないデータは消えますが\nよろしいですか？";
            confirmPanel.SetActive(true);
        }
    }

    // はいボタン
    public void OnConfirmYes()
    {
        confirmPanel.SetActive(false);
        if (isQuitting)
            StartCoroutine(FadeOutExit());
        else
            StartCoroutine(FadeOutTitle());
    }

    // いいえボタン
    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
    }

    IEnumerator FadeOutExit()
    {
        isFadingOut = true;
        fadeImage.gameObject.SetActive(true);

        while (fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.unscaledDeltaTime;
            fadeImage.color = color;
            yield return null;
        }

        PlayerPrefs.Save();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    IEnumerator FadeOutTitle()
    {
        isFadingOut = true;
        fadeImage.gameObject.SetActive(true);

        while (fadeImage.color.a < 1)
        {
            Color color = fadeImage.color;
            color.a += fadeSpeed * Time.unscaledDeltaTime;
            fadeImage.color = color;
            yield return null;
        }

        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("設定画面");
        SceneManager.LoadScene("Scenes/タイトル画面");
    }

    // SettingsManagerに追加

    // SettingsManagerに追加

    [Header("進行度リセット")]
    public string[] clearKeys; // Inspectorで「Clear_1」「Clear_2」など入力

    public void OnResetButton()
    {
        foreach (string key in clearKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("設定画面");
        SceneManager.LoadScene("染谷/探索画面");
    }
}