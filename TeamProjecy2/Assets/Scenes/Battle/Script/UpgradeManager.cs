using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager :
MonoBehaviour
{
    public GameObject upgradePanel;

    public Transform upgradeArea;

    public GameObject upgradeCardPrefab;

    public bool isUpgradeOpen = false;

    int selectedIndex = 0;

    List<UpgradeCardUI> currentCards = new List<UpgradeCardUI>();

    void Update()
    {
        if (!isUpgradeOpen)
        {
            return;
        }

        // --- ç∂ ---
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex--;

            if (selectedIndex < 0)
            {
                selectedIndex = currentCards.Count - 1;
            }
            UpdateSelection();
        }

        // --- âE ---
        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex++;

            if (selectedIndex >= currentCards.Count)
            {
                selectedIndex = 0;
            }
            UpdateSelection();
        }

        // --- åàíË ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCards[selectedIndex]
            .Select();
        }
    }

    public void ShowUpgrade()
    {
        upgradePanel.SetActive(true);

        isUpgradeOpen =true;

        currentCards.Clear();

        foreach (Transform child in upgradeArea)
        {
            Destroy(child.gameObject);
        }

        HashSet<CardData> shownCards = new HashSet<CardData>();

        foreach ( CardData card in GameManager .instance .currentDeck)
        {
            if (shownCards.Contains(card))
            {
                continue;
            }

            shownCards.Add(card);

            GameObject obj =Instantiate(upgradeCardPrefab,upgradeArea);

            UpgradeCardUI ui =obj.GetComponent<UpgradeCardUI>();

            ui.Setup(card,this);

            currentCards.Add(ui);
        }

        selectedIndex =0;

        UpdateSelection();
    }


    void UpdateSelection()
    {
        for (int i = 0;i <currentCards.Count;i++)
        {
            currentCards[i].transform.localScale = i == selectedIndex ? Vector3.one * 1.2f : Vector3.one;
        }
    }

    // --- ã≠âª ---
    public void Upgrade(CardData card)
    {
        GameManager.instance.LevelUpCard(card);

        Debug.Log(card.cardName+" ã≠âª");

        SaveManager.instance.Save();

        CloseUpgrade();
    }

    void CloseUpgrade()
    {
        upgradePanel.SetActive(false);

        isUpgradeOpen = false;

        int floor = GameManager.instance.floor;
        if (floor <= 5)
        {
            SceneManager.LoadScene("map1");
        }
        else if (floor <= 10)
        {
            SceneManager.LoadScene("map2");
        }
        else if(floor <= 15)
        {
            SceneManager.LoadScene("map3");
        }
        else if (floor <= 16)
        {
            SceneManager.LoadScene("Endscene");
        }
        else
        {
            SceneManager.LoadScene("endless");
        }
        //SceneManager.LoadScene("MaP");
    }
}