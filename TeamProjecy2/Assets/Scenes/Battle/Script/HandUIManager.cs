using UnityEngine;

public class HandUIManager :
MonoBehaviour
{
    public UpgradeManager upgradeManager;
    
    [Header("SE")]
    public AudioClip cardSelectSE;

    [Header("参照")]
    public DeckManager deckManager;

    [Header("手札UIを生成する場所")]
    public Transform handArea;

    [Header("カードPrefab")]
    public GameObject cardPrefab;

    [Header("戦闘参照")]
    public EnemyStats targetEnemy;

    public BattlePlayerStats player;

    public TurnManager turnManager;

    public BattleUIManager uiManager;

    public SelectedCardView selectedCardView;

    // 選択中カード
    private int selectedIndex =0;

    // キーボード操作
    void Update()
    {
        // ログを開いている間操作不能
        if (uiManager.isHistoryOpen)
        {
            return;
        }

        if (upgradeManager.isUpgradeOpen) return;

        if (handArea.childCount <= 0) return;

        // 左(A)
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex--;

            if (selectedIndex < 0)
            {
                selectedIndex = handArea .childCount -1;
            }

            AudioManager.Instance.PlaySE(cardSelectSE);
            UpdateSelection();
        }

        // 右(D)
        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex++;

            if (selectedIndex >= handArea.childCount)
            {
                selectedIndex = 0;
            }

            AudioManager.Instance.PlaySE(cardSelectSE);
            UpdateSelection();
        }

        // 使用(Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardUI card = handArea.GetChild(selectedIndex).GetComponent<CardUI>();

            card.UseCard();
        }
    }

    // 手札更新
    public void RefreshHandUI()
    {
        ClearHandUI();

        foreach (CardData card in deckManager.hand)
        {
            CreateCard(card);
        }

        // 最初のカード選択
        selectedIndex = 0;

        UpdateSelection();
    }

    // カード生成
    void CreateCard(CardData cardData)
    {
        GameObject cardObject = Instantiate(cardPrefab,handArea);

        CardUI cardUI =cardObject.GetComponent<CardUI>();

        cardUI.player = player;

        cardUI.Setup(cardData);

        cardUI.deckManager = deckManager;

        cardUI.targetEnemy =targetEnemy;

        cardUI.player = player;

        cardUI.turnManager = turnManager;

        cardUI.uiManager = uiManager;
    }

    // 選択表示
    void UpdateSelection()
    {
        for (int i = 0;i <handArea.childCount;i++)
        {
            Transform card = handArea.GetChild(i);

            if (i == selectedIndex)
            {
                card.localScale = Vector3.one * 1.2f;
            }

            else
            {
                card.localScale = Vector3.one;
            }
        }

        // --- 拡大カード表示 ---
        CardUI selectedCard =handArea.GetChild(selectedIndex).GetComponent<CardUI>();

        selectedCardView.ShowCard(selectedCard.cardData);
    }

    // UI削除
    void ClearHandUI()
    {
        foreach (Transform child in handArea)
        {
            Destroy(child.gameObject);
        }
    }
}