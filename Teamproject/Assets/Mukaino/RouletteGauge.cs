using UnityEngine;
using System;

public class RouletteGauge : MonoBehaviour
{
    [Header("設定")]
    public float rotateSpeed = 180f;   // 1秒あたりの回転角度
    public float successAngleMin = -30f; // 成功ゾーン開始角度
    public float successAngleMax = 30f;  // 成功ゾーン終了角度

    [Header("参照")]
    public GameObject gaugeUI;     // ゲージ全体のUI
    public Transform arrow;        // 回転する矢印
    public Transform successZone;

    public bool isSpinning = false;
    private float currentAngle = 0f;
    private float zoneOffset = 0f;
    private Action onSuccess;
    private Action onFail;

    void Update()
    {
        if (!isSpinning) return;

        currentAngle += rotateSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;
        arrow.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
    }

    // 外から呼んでゲージを起動する
    public void StartGauge(Action successCallback, Action failCallback)
    {
        onSuccess = successCallback;
        onFail = failCallback;
        currentAngle = 0f;
        isSpinning = true;
        gaugeUI.SetActive(true);

        // 成功ゾーンをランダムな角度に回転させる
        zoneOffset = UnityEngine.Random.Range(0f, 360f);
        if (successZone != null)
            successZone.localRotation = Quaternion.Euler(0f, 0f, -zoneOffset);
    }

    // クリックで止める
    public void StopGauge()
    {
        if (!isSpinning) return;
        isSpinning = false;
        gaugeUI.SetActive(false);

        // 矢印の角度と成功ゾーンの角度の差を計算
        float diff = (currentAngle - zoneOffset) % 360f;
        if (diff < 0) diff += 360f;
        if (diff > 180f) diff -= 360f;

        if (diff >= successAngleMin && diff <= successAngleMax)
            onSuccess?.Invoke();
        else
            onFail?.Invoke();
    }
}