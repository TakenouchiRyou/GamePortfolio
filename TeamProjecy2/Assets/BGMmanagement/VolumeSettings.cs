using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("スライダー")]
    [SerializeField] private Slider masterSlider;  // マスター音量スライダー
    [SerializeField] private Slider bgmSlider;     // BGM音量スライダー
    [SerializeField] private Slider seSlider;      // SE音量スライダー

    [Header("テキスト")]
    [SerializeField] private TextMeshProUGUI masterValue; // マスター音量テキスト
    [SerializeField] private TextMeshProUGUI bgmValue;    // BGM音量テキスト
    [SerializeField] private TextMeshProUGUI seValue;     // SE音量テキスト

    [Header("SE")]
    [SerializeField] private AudioClip clickSE; // クリック音

    // 設定画面が表示されるたびに実行
    private IEnumerator OnEnable()
    {
        // 1フレーム待ってから実行
        yield return null;

        // PlayerPrefsから保存済みの音量を読み込んでスライダーに反映
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float seVol = PlayerPrefs.GetFloat("SEVolume", 1f);

        // スライダーに値をセット（OnValueChangedも発火して音量も反映される）
        masterSlider.value = masterVol;
        bgmSlider.value = bgmVol;
        seSlider.value = seVol;
    }

    // マスター音量を変更する
    public void SetMasterVolume(float value)
    {
        // 0?1をdBに変換（0のときは-80dBにして無音にする）
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
        // テキストに％表示
        masterValue.text = "メイン音量　" + Mathf.RoundToInt(value * 100) + "%";
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    // BGM音量を変更する
    public void SetBGMVolume(float value)
    {
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("BGMVolume", dB);
        bgmValue.text = "BGM　" + Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    // SE音量を変更する
    public void SetSEVolume(float value)
    {
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat("SEVolume", dB);
        seValue.text = "SE　" + Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("SEVolume", value);
    }

    // クリック音を再生する
    public void PlayClickSE()
    {
        if (AudioManager.Instance != null && clickSE != null)
        {
            AudioManager.Instance.PlaySE(clickSE);
        }
    }
}