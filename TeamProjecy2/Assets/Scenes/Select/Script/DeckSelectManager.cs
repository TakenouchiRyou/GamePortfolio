using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSelectManager :
MonoBehaviour
{
    [SerializeField]
    private AudioClip moveSE;
    [SerializeField]
    private AudioClip selectSE;

    // デッキ一覧
    public DeckData[] decks;

    // UI
    public Transform deckArea;

    public GameObject deckPrefab;

    List<DeckSelectCardUI> cards = new List<DeckSelectCardUI>();

    int selectedIndex = 0;

    // 開始
    void Start()
    {
        foreach (DeckData deck in decks)
        {
            GameObject obj =Instantiate(deckPrefab,deckArea);

            DeckSelectCardUI ui = obj.GetComponent<DeckSelectCardUI>();

            ui.Setup(deck,this);

            cards.Add(ui);
        }
        UpdateSelection();
    }

    // 入力
    void Update()
    {
        if (PauseUI.isPaused) return;//追加

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlaySE(moveSE);
            selectedIndex--;

            if (selectedIndex < 0)
            {
                selectedIndex = cards.Count - 1;
            }
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.Instance.PlaySE(moveSE);
            selectedIndex++;

            if (selectedIndex >= cards.Count)
            {
                selectedIndex = 0;
            }
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySE(selectSE);
            cards
            [
                selectedIndex
            ]
            .Select();
        }
    }

    // 見た目更新
    void UpdateSelection()
    {
        for (int i = 0;i <cards.Count;i++)
        {
            if (i == selectedIndex)
            {
                cards[i].transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                cards[i].transform.localScale = Vector3.one;
            }
        }
    }

    // デッキ決定
    public void ChooseDeck(DeckData deck)
    {
        GameManager.instance.selectedDeck = deck;
        GameManager.instance.StartRun();
        SceneManager.LoadScene("Map1");
    }
}