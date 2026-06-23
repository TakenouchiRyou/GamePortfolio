using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardUI : MonoBehaviour
{
    public BattlePlayerStats player;

    [Header("UI")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI powerText;
    public Image cardImage;
    public BattleUIManager uiManager;
    public Button button;
    public TextMeshProUGUI descriptionText;

    [Header("参照")]
    public CardData cardData;
    public DeckManager deckManager;
    public EnemyStats targetEnemy;
    public TurnManager turnManager;

    // --- カード表示 ---
    public void Setup(CardData data)
    {
        cardData = data;

        cardNameText.text = data.cardName;

        int level = GameManager.instance.GetCardLevel(data);

        int displayPower = data.power + (level - 1) * data.levelUpPower;

        int totalPower = displayPower + player.healBuff;

        powerText.text = displayPower.ToString();
        if (cardData.cardType == CardType.Attack)
        {
            if (player.healBuff > 0)
            {
                if (cardData.attackCount > 1)
                {
                    powerText.text = displayPower + "(+" + player.healBuff + ")" + " × " + cardData.attackCount;
                }
                else
                {
                    powerText.text = displayPower + "(+" + player.healBuff + ")";
                }
            }   
            else
            {
                if (cardData.attackCount > 1)
                {
                    powerText.text = displayPower + " × " + cardData.attackCount;
                }
                else
                {
                    powerText.text = displayPower.ToString();
                }
            }
        }

        cardImage.sprite = data.cardImage;

        descriptionText.text = data.description;
    }

    // --- エフェクト ---
    void PlayCardEffect()
    {
        if(cardData.useEffect == null)
        {
            return;
        }
        Vector3 effectPos;
        if(cardData.effectTarget == EffectTarget.Player)
        {
            effectPos = player.transform.position;
        }
        else
        {
            if(targetEnemy == null)
            {
                return;
            }
            effectPos = targetEnemy.transform.position;
        }
        Instantiate(cardData.useEffect,effectPos,Quaternion.identity);
    }

    // --- カードSE ---
    void PlayCardSE()
    {
        if (cardData.useSE == null)
        {
            return;
        }

        AudioManager.Instance.PlaySE(cardData.useSE);
    }

    IEnumerator AttackCoroutine(int finalValue)     // 攻撃カード処理
    {
        for (int i = 0; i < cardData.attackCount; i++)
        {
            if (targetEnemy == null)        // 複数攻撃時敵が途中で死ぬとエラーが出るときの対策
            {
                break;
            }
            PlayCardEffect();
            PlayCardSE();
            int damage = finalValue + player.healBuff;
            targetEnemy.TakeDamage(damage);
            int bonusDamage = damage * GameManager.instance.bonusDamagePercent / 100;
            //    targetEnemy.TakeDamage(finalValue);
            if (bonusDamage > 0)
            {
                targetEnemy.TakeDamage(bonusDamage);
            }

            yield return new WaitForSeconds(0.15f);
        }

        // ログ関連
        int previewDamage = finalValue + player.healBuff;
        int previewBonus = previewDamage * GameManager.instance.bonusDamagePercent / 100;
        string attackText = previewDamage.ToString();

        if (previewBonus > 0)        // 超過バフ
        {
            attackText += "(+" + previewBonus + ")";
        }

        if (cardData.attackCount > 1)
        {
            attackText += " × " + cardData.attackCount;
        }

        uiManager.ShowActionLog(cardData.cardName + "\n" + attackText
        );

        deckManager.UseCard(cardData);

        Destroy(gameObject);

        turnManager.EndPlayerTurn();

        deckManager.handUIManager.RefreshHandUI();

    }

    // --- カード使用 ---
    public void UseCard()
    {
        if (! turnManager .CanPlayerAct())
        {
            return;
        }

        int level = GameManager.instance .GetCardLevel(cardData);

        int finalValue = cardData.power +
            ((level - 1) * cardData.levelUpPower);

        switch (cardData.cardType)
        {
            // --- 攻撃 ---
            case CardType.Attack:
                {
                    StartCoroutine(AttackCoroutine(finalValue));
                    return;
                }

            // --- 防御 ---
            case CardType.Defense:
                
                    player.AddDefense(finalValue);

                    uiManager.ShowActionLog("防御 +" + finalValue);

                PlayCardEffect();
                PlayCardSE();
                if (cardData.enableCounter)
                    {
                        player.counterAttack = true;
                    }

                    break;
                
            // --- 回復 ---
            case CardType.Heal:

                PlayCardEffect();
                PlayCardSE();
                if (
                    cardData.enableHealBuff)
                {
                    player.HealWithBuff(finalValue,cardData.healBuffRate);
                }

                else
                {
                    player.Heal(
                        finalValue);
                }

                break;

            // --- 毒 ---
            case CardType.poison:
                {
                    // 起爆ダメージ
                    int explodeDamage = 0;
                    int bonusDamage = 0;

                    PlayCardEffect();
                    PlayCardSE();

                    if (cardData.poisonattack > 0)
                    {
                        explodeDamage = targetEnemy.poison * cardData.poisonattack;
                        targetEnemy.TakeDamage(explodeDamage);
                        // 追加ダメージ
                        bonusDamage = explodeDamage * GameManager.instance.bonusDamagePercent / 100;
                        if (bonusDamage > 0)
                        {
                            targetEnemy.TakeDamage(bonusDamage);
                        }
                    }
                    targetEnemy.AddPoison(finalValue);
                    if (explodeDamage > 0)
                    {
                        uiManager.ShowActionLog(cardData.cardName + "\n起爆 " + explodeDamage + "(+" + bonusDamage + ")" + "\n毒 " + finalValue + "付与");
                    }
                    else
                    {
                        uiManager.ShowActionLog("毒 " + finalValue + " 付与");
                    }
                    break;
                }
            // --- バフ ---
            case CardType.Buff:

                PlayCardEffect();
                PlayCardSE();
                if (cardData.enableDamageBoost)
                {
                    GameManager.instance.bonusDamagePercent += finalValue;
                    uiManager.ShowActionLog(cardData.cardName + "\n追加ダメージ +" + finalValue + "%");
                }

                if (cardData.enableTaunt)
                {
                    int tauntTurn = cardData.tauntTurns + ((level - 1)* cardData.levelUpTauntTurns);
                    GameManager.instance.tauntTurns += finalValue;
                    uiManager.ShowActionLog(cardData.cardName + "\n挑発" + finalValue + "ターン");
                }
                break;
        }
        
        deckManager.UseCard(cardData);

        Destroy(gameObject);

        turnManager.EndPlayerTurn();

        deckManager.handUIManager.RefreshHandUI();
    }
}