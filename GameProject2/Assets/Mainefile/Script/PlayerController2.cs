using DG.Tweening;
using System.Collections;
using UnityEngine;

// プレイヤー制御（別バージョン）
public class PlayerController2 : MonoBehaviour
{
    // ===== 移動設定 =====
    public float speed = 5.0f;   // 左に進むスピード

    // ===== HP設定 =====
    public int maxHP = 3;        // 最大HP
    public int currentHP;        // 現在のHP

    // ===== 無敵・演出設定 =====
    public float invincibleTime = 1.5f; // 無敵時間
    public int flashCount = 5;          // 点滅回数

    // ===== コンポーネント =====
    SpriteRenderer sr;           // 色変更用
    Rigidbody2D rb;              // 物理挙動

    // ===== 状態 =====
    Color originalColor;         // 元の色
    bool isInvincible = false;   // 無敵フラグ


    [Header("効果音")]
    public AudioClip damageSE;

    AudioSource audioSource;

    // 初期化
    void Start()
    {
        // コンポーネント取得
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // 元の色を保存
        originalColor = sr.color;

        // 初期移動（左へ）
        rb.velocity = Vector2.left * speed;

        // HP初期化
        currentHP = maxHP;
        sr = GetComponent<SpriteRenderer>();
    }

    // 毎フレーム処理
    void Update()
    {
        // 停止していたら再び動かす（無敵中は除外）
        if (rb.velocity.x == 0 && !isInvincible)
        {
            rb.velocity = Vector2.left * speed;
        }
    }

    // 敵と接触したとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemyタグかつ無敵でない場合のみ
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            // ダメージ処理
            TakeDamage(1);

            // 一旦停止
            rb.velocity = Vector2.zero;

            // ノックバック（右へジャンプ）
            transform.DOJump(transform.position + Vector3.right, 2f, 2, 1.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // 終了後、左へ再移動
                    rb.velocity = Vector2.left * speed;
                })
                .SetLink(gameObject);

            // 無敵＋点滅開始
            StartCoroutine(InvincibleRoutine());
        }
    }

    // ダメージ処理
    void TakeDamage(int damage)
    {
        currentHP -= damage;

        // ダメージ音
        if (damageSE != null)
        {
            audioSource.PlayOneShot(damageSE);
        }

        Debug.Log("Player HP: " + currentHP);

        // HPが0以下なら死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 無敵＋点滅処理
    IEnumerator InvincibleRoutine()
    {
        // 無敵開始
        isInvincible = true;

        // 点滅（赤⇔元色）
        for (int i = 0; i < flashCount; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));

            sr.color = originalColor;
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));
        }

        // 念のため元に戻す
        sr.color = originalColor;

        // 無敵終了
        isInvincible = false;
    }

    // 死亡処理
    void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }

    // 使ってないけど残してる（必要なら使える）
    IEnumerator ResumeMovement()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.left * speed;
    }
}
