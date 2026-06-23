using System.Collections;
using UnityEngine;

public class ScreenGlitch : MonoBehaviour
{
    RectTransform rectTransform;

    Vector3 startPos;

    [Header("揺れの強さ")]
    public float glitchAmount = 5f;

    [Header("最小待機時間")]
    public float minTime = 2f;

    [Header("最大待機時間")]
    public float maxTime = 5f;

    [Header("グリッチ時間")]
    public float glitchTime = 0.08f;

    void Start()
    {
        // RectTransform取得
        rectTransform = GetComponent<RectTransform>();

        // 初期位置保存
        startPos = rectTransform.localPosition;

        // 開始
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            // ランダム待機
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            float timer = 0f;

            while (timer < glitchTime)
            {
                timer += Time.deltaTime;

                // 横方向メイン
                float x = Random.Range(-glitchAmount, glitchAmount);

                // 縦は少し
                float y = Random.Range(-1f, 1f);

                // 位置変更
                rectTransform.localPosition =
                    startPos + new Vector3(x, y, 0);

                yield return null;
            }

            // 元に戻す
            rectTransform.localPosition = startPos;
        }
    }
}