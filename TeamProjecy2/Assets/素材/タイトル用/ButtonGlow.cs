using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// ボタンにカーソルが乗った時に文字を光らせるスクリプト
// IPointerEnterHandler / IPointerExitHandler を実装して
// マウスホバー時のイベントを受け取る
public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 対象となる TextMeshPro のテキスト
    public TextMeshProUGUI text;

    // 通常時のフォントマテリアル
    public Material normalMaterial;

    // ホバー時に光らせるフォントマテリアル
    public Material glowMaterial;

    // マウスカーソルがボタンに乗った瞬間に呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        // テキストのマテリアルを光るマテリアルに変更
        text.fontSharedMaterial = glowMaterial;
    }

    // マウスカーソルがボタンから離れた瞬間に呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        // テキストのマテリアルを通常のものに戻す
        text.fontSharedMaterial = normalMaterial;
    }
}
