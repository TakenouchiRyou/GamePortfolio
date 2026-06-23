using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicItem : Interactable
{
    public string relicID;      // アイテム番号（同じ数値のアイテムがあると、入手時消えるので注意）
    public RelicType relicType;
    public int value = 1;       // HP増加量
    public CardData addCard;
    public int appearFloor = 1;

    void Start()
    {
        if (GameManager.instance.obtainedRelics.Contains(relicID))
        {
            Destroy(gameObject);
            return;
        }
        if (GameManager.instance.currentFloor < appearFloor) 
        {
            Destroy(gameObject);
            return;
        }
    }

    void ApplyRelic()
    {
        switch (relicType) 
        {
            case RelicType.Survive:

                GameManager.instance.surviveAtOneHP= true;
                Debug.Log("耐え取得");
                break;

            case RelicType.Buff:
                GameManager.instance.bonusDamagePercent += value;
                Debug.Log("与ダメージ +" + value + "%");
                break;
            case RelicType.MaxHP:
                GameManager.instance.bonusMaxHP += value;
                Debug.Log("最大HP +" + value);
                break;
            case RelicType.Heal:
                GameManager.instance.playerCurrentHP += value;

                int maxHP = 100 + GameManager.instance.bonusMaxHP;

                GameManager.instance.playerCurrentHP =
                    Mathf.Min(
                        GameManager.instance.playerCurrentHP,
                        maxHP
                    );

                Debug.Log("HP +" + value);
                break;
        }
    }

    public override void OnInteract()
    {
        if (addCard != null)
        {
            GameManager.instance.currentDeck.Add(addCard);
        }
        ApplyRelic();

        GameManager.instance.obtainedRelics.Add(relicID);
        SaveManager.instance.Save();
        Destroy(gameObject);
    }
}
