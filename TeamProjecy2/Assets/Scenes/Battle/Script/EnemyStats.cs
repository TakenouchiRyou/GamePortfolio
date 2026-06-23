using UnityEngine;

public class EnemyStats :
MonoBehaviour
{
    public Canvas　canvas;

    public GameObject
    damageTextPrefab;

    Transform
        damageTextSpawn;

    public BattleUIManager
        uiManager;

    public UpgradeManager
        upgradeManager;

    // 敵データ
    public EnemyData
        slime;

    public EnemyData
        goblin;

    public EnemyData
        dragon;

    public EnemyData endless;

    // 見た目
    public SpriteRenderer
        enemySprite;

    // 現在ステータス
    public int maxHP;

    public int currentHP;

    public int attackPower;

    public int healPower;

    public int defense = 0;

    // 状態

    public int poison = 0;

    public bool isDead = false;
    public EnemyData currentEnemy;
    // 戦闘開始
    void Start()
    {
        int floor = GameManager.instance.floor;

        //EnemyData currentEnemy;

        // 敵切替
        if(floor <= 5)
        {
            currentEnemy = slime;  // 1～5
        }

        else if( floor <= 10)
        {
            currentEnemy = goblin; // 6～10
        }

        else if(floor <= 15)
        {
            currentEnemy = dragon; // 11～15
        }
        else
        {
            currentEnemy = endless; // 16～
        }

        // 見た目変更
        enemySprite.sprite = currentEnemy .enemySprite;

        // ステータス
        maxHP = currentEnemy .baseHP + floor * 5;

        attackPower = currentEnemy .baseAttack + floor * 2;

        healPower = currentEnemy .baseHeal;

        currentHP = maxHP;

        Debug.Log(currentEnemy .enemyName + " 出現");
    }

    void Awake()
    {
        damageTextSpawn = transform.Find("DamageSpawn");
        Debug.Log(damageTextSpawn);
    }

    // ダメージ
    public void
        TakeDamage(int damage)
    {
        if (isDead) {  return; }

        Debug.Log(damageTextPrefab);

        Debug.Log(damageTextSpawn);

        Debug.Log(FindObjectOfType<Canvas>());

        Vector3 randomOffset =
    new Vector3(
        Random.Range(-50f, 50f),Random.Range(-30f, 30f),0f);

        GameObject obj =
        Instantiate(damageTextPrefab,damageTextSpawn.position + randomOffset,Quaternion.identity,canvas.transform);

        DamageText text = obj.GetComponent<DamageText>();

        text.Setup(damage);

        if (isDead)　return;

        int finalDamage =　Mathf.Max(damage　-　defense,0);

        currentHP -= finalDamage;

        if(currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        uiManager.RefreshUI();
    }

    // 毒
    public void AddPoison(int amout)
    {
        poison += amout;
        Debug.Log("毒");
        //Debug.Log("毒 +"+ amount);
    }

    // 回復
    public void Heal(int amount)
    {
        if(isDead) return;

        currentHP += amount;

        currentHP = Mathf.Min(currentHP,maxHP);

        uiManager.RefreshUI();
    }

    // 防御
    public void AddDefense(int amount)
    {
        defense += amount;
    }

    // ターン終了
    public void EndTurn()
    {
        defense = 0;
    }

    // 攻撃
    public void Attack(
        BattlePlayerStats player)
    {
        player.TakeDamage(attackPower);
    }

    // 死亡
    void Die()
    {
        isDead = true;

        GameManager.instance.floor++;
        GameManager.instance.currentFloor++;

        upgradeManager.ShowUpgrade();

        BattleManager.Instance.PlayerWin();

        Destroy(gameObject);
    }

    // HP割合
    public float
        GetHPRatio()
    {
        return(float)currentHP /maxHP;
    }
}