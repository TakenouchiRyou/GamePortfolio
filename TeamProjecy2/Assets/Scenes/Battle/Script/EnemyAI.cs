using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public BattleUIManager uiManager;

    [Header("参照")]
    public EnemyStats enemyStats;

    public BattlePlayerStats playerStats;

    [Header("エフェクト")]
    public ParticleSystem poisonTickEffect;
    public ParticleSystem attackEffect;
    public ParticleSystem defendEffect;
    public ParticleSystem healEffect;

    [Header("エフェクト位置")]
    public Transform attackEffectPoint;
    public Transform defendEffectPoint;
    public Transform healEffectPoint;
    
    IEnumerator PoisonDamageCoroutine()     // 毒エフェクト
    {
        if (poisonTickEffect != null)
        {
            Instantiate(
                poisonTickEffect,
                enemyStats.transform.position,
                Quaternion.identity
            );
        }

        yield return new WaitForSeconds(0.8f);      

        int poisonDamage = enemyStats.poison;

        enemyStats.TakeDamage(poisonDamage);

        int bonusDamage =
            poisonDamage *
            GameManager.instance.bonusDamagePercent / 100;

        if (bonusDamage > 0)
        {
            enemyStats.TakeDamage(bonusDamage);
        }

        uiManager.ShowActionLog(
            "毒 " + poisonDamage + "(+" + bonusDamage + ")"
        );

        enemyStats.poison--;

        if (!enemyStats.isDead)
        {
            ChooseAction();
        }
    }

    // --- エフェクト再生 ---
    void PlayEffect(ParticleSystem effect,Transform effectPoint)
    {
        if (effect == null || effectPoint == null)
        {
            return;
        }

        Instantiate(
            effect,
            effectPoint.position,
            Quaternion.identity
        );
    }

    // 敵ターン
    public void EnemyTurn()
    {
        // 死んでたら行動しない
        if (enemyStats.isDead) return;

        Debug.Log("敵ターン開始");

        // 毒
        if(enemyStats.poison > 0)
        {
            StartCoroutine(PoisonDamageCoroutine());
            return;
            //if (poisonTickEffect != null)
            //{
            //    Instantiate(
            //        poisonTickEffect,
            //        enemyStats.transform.position,
            //        Quaternion.identity
            //    );
            //}
            //int poisonDamage = enemyStats.poison;
            //enemyStats.TakeDamage(poisonDamage);
            //int bonusDamage = poisonDamage * GameManager.instance.bonusDamagePercent / 100;
            //if(bonusDamage > 0)
            //{
            //    enemyStats.TakeDamage(bonusDamage);
            //}

            //uiManager.ShowActionLog("毒 " +  poisonDamage + "(+" + bonusDamage + ")");

            //enemyStats.poison--;
        }

        // 毒で死んだとき
        if (enemyStats.isDead) return;

        // 行動決定
        ChooseAction();
    }

    // 行動選択
    void ChooseAction()
    {
        //// HP低い時は回復優先     デッキによってゲームの体験が損なわれるためなし
        //if (enemyStats.currentHP <= enemyStats.maxHP / 3)
        //{
        //    int lowHPAction = Random.Range(0, 100);

        //    // 40%で回復
        //    if (lowHPAction < 40)
        //    {
        //        Heal();
        //        return;
        //    }
        //}

        // 通常行動
        int action = Random.Range(0, 100);

        if (GameManager.instance.tauntTurns > 0)
        {
            Attack();
            GameManager.instance.tauntTurns--;
            return;
        }

        if (action < 70)
        {
            Attack();
        }
        else if (action < 80)
        {
            Defend();
        }
        else
        {
            Heal();
        }
    }

    // 攻撃
    void Attack()
    {
        PlayEffect(attackEffect,attackEffectPoint);
        AudioManager.Instance.PlaySE(enemyStats.currentEnemy.attackSE);
        Debug.Log("敵が攻撃");
        
        uiManager.ShowActionLog(
        "敵は攻撃した！");

        enemyStats.Attack(playerStats);
    }

    // 防御
    void Defend()
    {
        PlayEffect(defendEffect, defendEffectPoint);

        AudioManager.Instance.PlaySE(enemyStats.currentEnemy.attackSE);
        int defenseAmount = 3;

        enemyStats.AddDefense(defenseAmount);

        Debug.Log("敵が防御");

        uiManager.ShowActionLog(
        "敵は防御した！");
    }

    // 回復
    void Heal()
    {
        PlayEffect(healEffect, healEffectPoint);
        AudioManager.Instance.PlaySE(enemyStats.currentEnemy.attackSE);

        int healAmount = 3;

        enemyStats.Heal(healAmount);

        Debug.Log("敵が回復");

        uiManager.ShowActionLog(
        "敵は回復した！");
    }
}