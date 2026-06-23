using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BattlePlayerStats : MonoBehaviour
{
    public BattleUIManager uiManager;

    public EnemyStats enemyStats;

    public bool counterAttack = false;

    public int healBuff = 0;        //超過回復バフ

    bool surviveUsed = false;       //一度限りバフ

    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("戦闘ステータス")]
    public int defense = 0;

    [Header("状態")]
    public bool isDead = false;

    void Start()
    {
        // 耐えリセット
        surviveUsed = false;
        // 戦闘開始時HP
        maxHP += GameManager.instance.bonusMaxHP;
        currentHP = GameManager.instance.playerCurrentHP;
    }

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        if (isDead)return;

        // 攻撃を受ける前の防御値
        int beforeDefense = defense;

        // 防御で吸収
        int blockedDamage = Mathf.Min(defense,damage);

        defense -= blockedDamage;

        // 実際のダメージ
        int finalDamage =damage - blockedDamage;

        currentHP -=finalDamage;

        // --- 反撃 ---
        if (counterAttack && beforeDefense > damage)
        {
            int counterDamage = beforeDefense - damage;

            enemyStats.TakeDamage(counterDamage);

            // 追加ダメージ
            int bonusDamage = counterDamage * GameManager.instance.bonusDamagePercent / 100;
            if (bonusDamage > 0) 
            { 
                enemyStats.TakeDamage(bonusDamage);
            }

            Debug.Log("反撃 " + counterDamage + "(+" + bonusDamage + ")");

            counterAttack = false;
        }

        Debug.Log("防御で " + blockedDamage + " 防いだ / ダメージ " + finalDamage);

        // 耐え
        if (currentHP <= 0 && GameManager.instance.surviveAtOneHP && !surviveUsed) 
        {
            currentHP = 1;
            surviveUsed |= true;
            Debug.Log("耐え");
        }
        else if (currentHP <= 0) 
        {
            currentHP = 0;
            Die();
        }

        GameManager.instance.playerCurrentHP = currentHP;
        uiManager.RefreshUI();
    }

    // 回復
    public void Heal(int amount)
    {
        if (isDead) return;

        //int missingHP = maxHP - currentHP;

        //if (amount > missingHP)
        //{
        //    int extre = amount - missingHP;
        //    healBuff += extre;
        //    Debug.Log("超過回復 " + extre);
        //}

        currentHP += amount;

        currentHP = Mathf.Min(currentHP, maxHP);

        GameManager.instance.playerCurrentHP = currentHP;

        //// 最大HPを超えない
        //currentHP = Mathf.Min(currentHP, maxHP);
        Debug.Log("プレイヤーが " + amount + " 回復");

        uiManager.RefreshUI();
    }

    // 防御
    public void AddDefense(int amount)
    {
        if (isDead) return;

        defense += amount;

        Debug.Log(
        "防御 +" +
        amount +
        " 現在防御=" +
        defense);
    }

    public void HealWithBuff(int amount,int buffRate)
    {
        if (isDead)return;

        int missingHP = maxHP - currentHP;

        int extraHeal = Mathf.Max(amount - missingHP,0);
        healBuff += extraHeal * buffRate;
        Debug.Log("超過回復 " + extraHeal);

        currentHP += amount;

        currentHP = Mathf.Min(currentHP,maxHP);

        if (buffRate > 0)
        {
            healBuff += extraHeal * buffRate;

            Debug.Log("攻撃バフ +" + extraHeal * buffRate);
        }
        GameManager.instance.playerCurrentHP = currentHP;
        uiManager.RefreshUI();
    }

    // ターン終了処理
    public void EndTurn()
    {
        // 防御を毎ターン消す
        //defense = 0;

        Debug.Log("ターン終了");
    }

    // 死亡
    void Die()
    {
        isDead = true;

        Debug.Log("プレイヤー死亡");

        BattleManager.Instance.PlayerLose();
    }

    // HP割合取得（UI用）
    public float GetHPRatio()
    {
        return (float)currentHP / maxHP;
    }
}