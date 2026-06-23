using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// ゲーム全体のBGM・SEを管理するクラス
/// 各シーンに配置してもPlayerPrefsから音量を復元する
/// </summary>
public class AudioManager : MonoBehaviour
{
    // シングルトン
    public static AudioManager Instance { get; private set; }

    [Header("AudioSources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SE_VOLUME_KEY = "SEVolume";

    private void Awake()
    {
        // 以前のAudioManagerが存在したら削除
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        // シーンをまたいでも保持
        DontDestroyOnLoad(gameObject);

        // 保存済み音量を読み込む
        float masterVol = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        float bgmVol = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        float seVol = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 1f);

        ApplyMasterVolume(masterVol);
        ApplyBGMVolume(bgmVol);
        ApplySEVolume(seVol);
    }

    //=========================
    // 音量設定
    //=========================

    private void ApplyMasterVolume(float value)
    {
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
    }

    private void ApplyBGMVolume(float value)
    {
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("BGMVolume", dB);
    }

    private void ApplySEVolume(float value)
    {
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("SEVolume", dB);
    }

    // 外部から変更できるようにする
    public void SetMasterVolume(float value)
    {
        ApplyMasterVolume(value);
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
    }

    public void SetBGMVolume(float value)
    {
        ApplyBGMVolume(value);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, value);
    }

    public void SetSEVolume(float value)
    {
        ApplySEVolume(value);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, value);
    }

    //=========================
    // BGM
    //=========================

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        // 同じ曲なら何もしない
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.Stop();

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //=========================
    // SE
    //=========================

    public void PlaySE(AudioClip clip)
    {
        if (clip == null || seSource == null)
            return;

        seSource.PlayOneShot(clip);
    }
}