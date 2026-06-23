using UnityEngine;

[CreateAssetMenu(fileName = "NewCard",menuName = "Card")]

public class CardData :
ScriptableObject
{
    public string cardName;

    public Sprite cardImage;

    public CardType cardType;

    public UpgradeType upgradeType;

    public int power;

    public int levelUpPower = 1;

    public int evolveLevel = 5;

    public CardData evolveTo;

    [Header("エフェクト")]
    public ParticleSystem useEffect;
    public EffectTarget effectTarget;

    [Header("SE")]
    public AudioClip useSE;

    [TextArea(3,5)]
    public string description;

    public bool enableDamageBoost = false;      // 与ダメバフ
    public int damageBoostPercent = 0;

    public bool enableTaunt = false;        // 挑発
    public int tauntTurns = 1;
    public int levelUpTauntTurns = 1;

    // --- 進化後の調整 ---

    public int attackCount = 1;     // 攻撃カードの進化

    public int poisonattack = 0;    // 毒カードの進化

    public bool enableCounter = false;      // 防御カードの進化

    public bool enableHealBuff = false;     // 回復カードの進化
    public int healBuffRate = 0;
}

public enum CardType
{
    Attack,
    Defense,
    Heal,
    poison,
    Buff
}

public enum EffectTarget
{
    Player,
    Enemy
}