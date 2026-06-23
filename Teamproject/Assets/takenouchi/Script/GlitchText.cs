using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlitchText : MonoBehaviour
{
    // UIの位置を変更するため
    RectTransform rectTransform;

    // UIの色や透明度を変更するため
    Image image;

    // 元の位置を保存
    Vector3 startPos;

    // 元の色を保存
    Color startColor;

    [Header("揺れの強さ")]
    public float glitchAmount = 5f;

    [Header("最小待機時間")]
    public float minTime = 1f;

    [Header("最大待機時間")]
    public float maxTime = 3f;

    [Header("グリッチ時間")]
    public float glitchTime = 0.1f;

    void Start()
    {
        // RectTransform取得
        rectTransform = GetComponent<RectTransform>();

        // Image取得
        image = GetComponent<Image>();

        // 初期位置を保存
        startPos = rectTransform.localPosition;

        // 初期カラーを保存
        startColor = image.color;

        // グリッチ処理開始
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        // 無限ループ
        while (true)
        {
            // ランダム時間待機
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // 時間計測用
            float timer = 0f;

            // 指定時間グリッチ
            while (timer < glitchTime)
            {
                // 時間加算
                timer += Time.deltaTime;

                // ランダムなX座標
                float x = Random.Range(-glitchAmount, glitchAmount);

                // ランダムなY座標
                float y = Random.Range(-glitchAmount, glitchAmount);

                // 元の位置から少しズラす
                rectTransform.localPosition =
                    startPos + new Vector3(x, y, 0);

                // 元の色を取得
                Color color = startColor;

                // 透明度を少しランダム化
                color.a = Random.Range(0.8f, 1f);

                // 色を適用
                image.color = color;

                // 1フレーム待機
                yield return null;
            }

            // 位置を元に戻す
            rectTransform.localPosition = startPos;

            // 色を元に戻す
            image.color = startColor;
        }
    }
}