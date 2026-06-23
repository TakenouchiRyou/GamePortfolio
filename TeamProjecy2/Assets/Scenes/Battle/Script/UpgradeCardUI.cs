using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI :
MonoBehaviour
{
    public Image cardImage;

    public TextMeshProUGUI
        cardName;

    CardData card;

    UpgradeManager manager;

    // --- ‰Šú‰» ---
    public void Setup(
        CardData data,
        UpgradeManager m)
    {
        card =
            data;

        manager =
            m;

        cardImage.sprite =
            data.cardImage;

        int level =
            GameManager
            .instance
            .GetCardLevel(
                data);

        cardName.text =
            data.cardName
            +
            " Lv"
            +
            level;
    }

    // --- ‘I‘ğ ---
    public void Select()
    {
        manager
        .Upgrade(
            card);
    }
}