using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TMP_Text timerText;

    private System.Func<float> getTime; // ラムダ式で保存

    void Start()
    {
        WiringManager manager = FindObjectOfType<WiringManager>();

        // ラムダ式でタイマーの取得方法を保存
        getTime = () => manager.TimeLeft;
    }

    void Update()
    {
        timerText.text = Mathf.CeilToInt(getTime()).ToString();
    }
}