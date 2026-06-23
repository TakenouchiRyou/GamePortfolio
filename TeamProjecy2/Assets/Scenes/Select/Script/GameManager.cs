using System.Collections.Generic;
using UnityEngine;
//using System.Collections.Generic;

public class GameManager :
MonoBehaviour
{
    // --- 強化関連 ---
    public bool surviveAtOneHP = false;
    public HashSet<string> obtainedRelics = new HashSet<string>();
    public int bonusMaxHP = 0;
    public int tauntTurns = 0;

    // --- Singleton ---
    public static GameManager instance;

    // --- デッキ ---
    public DeckData selectedDeck;

    public List<CardData>currentDeck = new List<CardData>();

    // --- 進行状況 ---
    public int floor = 1;
    public int currentFloor = 1;

    // --- 強化値 ---
    public Dictionary<UpgradeType,int>upgradeLevels = new Dictionary<UpgradeType,int>();

    // --- カードLv ---
    public Dictionary<CardData,int>cardLevels =new Dictionary<CardData,int>();

    // --- Buffcard ---
    public int bonusDamagePercent = 0;

    public int playerCurrentHP = 100;       //tst

    // --- 初期化 ---
    void Awake()
    {
        if (instance == null)
        {
            instance =this;

            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);

            return;
        }

        // --- 強化値初期化 ---
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            upgradeLevels
            [
                type
            ]=0;
        }
    }

    // --- 起動時 ---
    void Start()
    {
        SaveManager.instance.DeleteSave();
    }

    // --- ラン開始 ---
    public void StartRun()
    {
        Debug.Log("startrun");currentDeck =new List<CardData>(selectedDeck.startingCards);
    }


    // --- 強化取得 ---
    public int GetUpgrade(UpgradeType type)
    {
        return
            upgradeLevels
            [
                type
            ];
    }


    // --- 強化追加 ---
    public void AddUpgrade(UpgradeType type,int amount)
    {
        upgradeLevels
        [
            type
        ] += amount;
    }


    // --- カードLv取得 ---
    public int GetCardLevel(CardData card)
    {
        if (!cardLevels.ContainsKey(card))
        {
            cardLevels
            [
                card
            ] = 1;
        }

        return cardLevels
            [
                card
            ];
    }


    // --- カードLvUP ---
    public void LevelUpCard(CardData card)
    {
        if (!cardLevels.ContainsKey(card))
        {
            cardLevels
            [
                card
            ] = 1;
        }

        cardLevels
        [
            card
        ]++;

        int level =cardLevels
            [
                card
            ];

        Debug.Log(card.cardName +" Lv "+level);

        // --- 進化判定 ---
        if (level >=card.evolveLevel && card.evolveTo !=null)
        {
            EvolveCard(card);
        }
    }


    // --- カード進化 ---
    void EvolveCard(CardData oldCard)
    {
        CardData newCard = oldCard.evolveTo;

        cardLevels.Remove(oldCard);

        cardLevels
        [
            newCard
        ] = 1;

        for (int i = 0;i <currentDeck.Count;i++)
        {
            if (currentDeck[i]==oldCard)
            {
                currentDeck[i] = newCard;
            }
        }

        Debug.Log(oldCard.cardName +" → "+newCard.cardName+" 進化");
    }

    // --- ランリセット ---
    public void ResetRun()
    {
        Debug.Log("ResetRun 呼ばれた");
        floor =1;

        // --- 強化初期化 ---
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            upgradeLevels
            [
                type
            ] = 0;
        }

        // --- カードLv初期化 ---
        cardLevels.Clear();

        // --- デッキ初期化 ---
        currentDeck = new List<CardData>(selectedDeck.startingCards);

        Debug.Log("Run Reset");
    }
}