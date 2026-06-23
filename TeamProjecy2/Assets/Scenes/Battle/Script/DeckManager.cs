using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("現在デッキ")]
    public List<CardData> deck = new List<CardData>();

    [Header("山札")]
    public List<CardData> drawPile = new List<CardData>();

    [Header("手札")]
    public List<CardData> hand = new List<CardData>();

    [Header("捨て札")]
    public List<CardData> discardPile = new List<CardData>();

    [Header("設定")]
    public int drawPerTurn = 4;

    [Header("UI")]
    public HandUIManager handUIManager;

    // 戦闘開始
    public void StartBattle()
    {
        GameManager.instance.bonusDamagePercent = 0;
        drawPile.Clear();
        hand.Clear();
        discardPile.Clear();

        // --- 現在デッキ取得 ---
        deck =
        new List<CardData>(
            GameManager
            .instance
            .currentDeck
        );

        foreach (
            CardData card
            in deck)
        {
            drawPile.Add(
                card);
        }

        Shuffle(
            drawPile);

        StartPlayerTurn();

        Debug.Log(
            "戦闘開始");
    }


    // プレイヤーターン開始
    public void StartPlayerTurn()
    {
        // 残り手札を捨てる
        DiscardHand();

        // 新しく引く
        DrawCards(drawPerTurn);

        // UI更新
        handUIManager.RefreshHandUI();

        Debug.Log("プレイヤー " + drawPerTurn + "枚ドロー");
    }

    // 手札全部捨てる
    void DiscardHand()
    {
        foreach (CardData card in hand)
        {
            discardPile.Add(card);
        }

        hand.Clear();
    }


    // 複数枚ドロー
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
        }
    }


    // 1枚ドロー
    void DrawCard()
    {
        if (drawPile.Count <= 0)
        {
            ReshuffleDiscardPile();
        }

        if (drawPile.Count <= 0)
        {
            return;
        }

        CardData card = drawPile[0];

        drawPile.RemoveAt(0);

        hand.Add(card);
    }


    // カード使用
    public void UseCard(CardData card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);

            // Buffカードは捨て札に送らない
            if (card.cardType != CardType.Buff)
            {
                discardPile.Add(card);
            }
        }
    }


    // 捨て札→山札
    void ReshuffleDiscardPile()
    {
        foreach (CardData card in discardPile)
        {
            drawPile.Add(card);
        }

        discardPile.Clear();

        Shuffle(drawPile);

        Debug.Log("山札再生成");
    }

    // シャッフル
    void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random =
                Random.Range(i, list.Count);

            CardData temp =
                list[i];

            list[i] =
                list[random];

            list[random] =
                temp;
        }
    }
}