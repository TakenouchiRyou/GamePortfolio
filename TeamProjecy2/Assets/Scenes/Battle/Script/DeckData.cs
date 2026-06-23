using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
fileName = "NewDeck",
menuName = "Deck")]

public class DeckData :
ScriptableObject
{
    public string deckName;

    public Sprite deckImage;

    public List<CardData>
        startingCards;
}