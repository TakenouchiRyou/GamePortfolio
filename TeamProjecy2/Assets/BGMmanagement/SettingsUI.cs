//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SettingsUI : MonoBehaviour
//{
//    [Header("設定パネル")]
//    [SerializeField] private GameObject settingsPanel;

//    [Header("スライダー")]
//    [SerializeField] private Slider bgmSlider;
//    [SerializeField] private Slider seSlider;

//    [Header("プレビュー用SEクリップ")]
//    [SerializeField] private AudioClip sePreviewClip;

//    private void Start()
//    {
//        //スライダーの初期値をAudioManegerから取得
//        bgmSlider.value = AudioManager.Instance.BGMVolume;
//        seSlider.value = AudioManager.Instance.SEVolume;

//        //スライダーの変更時のイベント登録
//        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
//        seSlider.onValueChanged.AddListener(OnSESliderChanged);

//        //最初は非表示
//        settingsPanel.SetActive(false);
//    }
//    // 設定ボタンから呼ぶ
//    public void OpenSettings() => settingsPanel.SetActive(true);
//    public void CloseSettings() => settingsPanel.SetActive(false);

//    private void OnBGMSliderChanged(float value)
//    {
//        AudioManager.Instance.BGMVolume = value;
//    }

//    private void OnSESliderChanged(float value)
//    {
//        AudioManager.Instance.SEVolume = value;

//        // スライダーを動かしたときにSEをプレビュー再生（クリップが設定されていれば）
//        if (sePreviewClip != null)
//            AudioManager.Instance.PlaySE(sePreviewClip);
//    }
//}
