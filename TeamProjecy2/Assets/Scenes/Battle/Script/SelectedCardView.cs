using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedCardView :
MonoBehaviour
{
    public Image
        cardImage;

    public TextMeshProUGUI
        cardNameText;

    public TextMeshProUGUI
        powerText;

    public TextMeshProUGUI
        descriptionText;

    public void ShowCard(
        CardData card)
    {
        int level =
            GameManager
            .instance
            .GetCardLevel(
                card);

        int power =
            card.power
            +
            (
                level
                -
                1
            )
            *
            card.levelUpPower;

        cardImage.sprite =
            card.cardImage;

        cardNameText.text =
            card.cardName;

        if (
            card.attackCount
            >
            1)
        {
            powerText.text =
                power
                +
                " Å~ "
                +
                card.attackCount;
        }

        else
        {
            powerText.text =
                power
                .ToString();
        }

        descriptionText.text =
            card.description;
    }
}