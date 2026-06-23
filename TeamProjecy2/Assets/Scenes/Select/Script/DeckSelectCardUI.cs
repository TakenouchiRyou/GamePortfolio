using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckSelectCardUI :
MonoBehaviour
{
    public Image
        deckImage;

    public TextMeshProUGUI
        deckName;

    DeckData
        data;

    DeckSelectManager
        manager;

    public void Setup(
        DeckData deck,
        DeckSelectManager m)
    {
        data =
            deck;

        manager =
            m;

        deckImage.sprite =
            deck.deckImage;

        deckName.text =
            deck.deckName;
    }

    public void Select()
    {
        manager
        .ChooseDeck(
            data);
    }
}