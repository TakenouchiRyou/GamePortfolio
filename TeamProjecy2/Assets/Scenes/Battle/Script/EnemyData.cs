using UnityEngine;

[CreateAssetMenu(fileName = "Enemy",menuName = "Enemy")]
public class EnemyData :
ScriptableObject
{
    [Header("基本")]
    public string enemyName;

    public Sprite enemySprite;

    [Header("基礎ステータス")]
    public int baseHP;

    public int baseAttack;

    public int baseHeal;

    // --- 音 ---
    public AudioClip attackSE;
}