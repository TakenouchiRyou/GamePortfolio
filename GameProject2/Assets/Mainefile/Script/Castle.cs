using UnityEngine;
using UnityEngine.SceneManagement;

// 城の種類
public enum CastleType
{
    Player,
    Enemy
}

// 城のHP管理と勝敗判定
public class Castle : MonoBehaviour
{
    // ===== HP設定 =====
    [Header("HP設定")]
    public int maxHP = 10;

    int currentHP;

    // ===== 城の種類 =====
    [Header("城の種類")]
    public CastleType castleType;

    // ===== 押し戻し設定 =====
    [Header("押し戻し設定")]
    public float pushPower = 3f;

    // ゲーム開始時
    void Start()
    {
        currentHP = maxHP;
    }

    // 城に触れた瞬間
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player または Enemy ならダメージ
        if (collision.CompareTag("Player") ||
            collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    // Trigger内にいる間ずっと押し戻す
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Player / Enemy 以外は無視
        if (!collision.CompareTag("Player") &&
            !collision.CompareTag("Enemy"))
        {
            return;
        }

        // Rigidbody2D が無ければ無視
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        if (rb == null) return;

        // 横方向だけ押し戻す
        float dir = Mathf.Sign(
            collision.transform.position.x - transform.position.x
        );

        // 少しずつ押し戻す
        collision.transform.position +=
            Vector3.right * dir * pushPower * Time.deltaTime;
    }

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP : " + currentHP);

        // HP0以下で破壊
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 城破壊
    void Die()
    {
        // プレイヤー城が壊れた → ゲームオーバー
        if (castleType == CastleType.Player)
        {
            SceneManager.LoadScene("GameOver");
        }
        // 敵城が壊れた → ゲームクリア
        else if (castleType == CastleType.Enemy)
        {
            SceneManager.LoadScene("GameClear");
        }

        Destroy(gameObject);
    }

    // 現在HP取得
    public int GetHP()
    {
        return currentHP;
    }

    // 最大HP取得
    public int GetMaxHP()
    {
        return maxHP;
    }
}