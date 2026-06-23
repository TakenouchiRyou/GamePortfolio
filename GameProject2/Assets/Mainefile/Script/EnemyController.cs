using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHP = 3;      // 最大HP
    public int currentHP;      // 現在のHP

    public float attackInterval = 1.0f; // 城への攻撃間隔

    private bool isKnockback = false;   // ノックバック中フラグ
    private bool canAttack = true;      // 攻撃できるか


    [Header("効果音")]
    public AudioClip damageSE;

    AudioSource audioSource;

    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        // コンポーネント取得
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        // 右方向へ移動開始
        rb.velocity = Vector2.right * speed;

        // HP初期化
        currentHP = maxHP;
    }

    void Update()
    {
        // 特に処理なし（必要ならここに書く）
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ===== プレイヤーに当たった =====
        if (collision.CompareTag("Player") && !isKnockback)
        {
            // 自分がダメージを受ける
            TakeDamage(1);

            // 点滅演出
            StartCoroutine(DamageFlash());

            // HPが半分以下ならノックバック
            if (currentHP <= maxHP / 2)
            {
                isKnockback = true;

                // 一旦停止
                rb.velocity = Vector2.zero;

                // 左へジャンプ（ノックバック）
                transform.DOJump(transform.position + Vector3.left, 2f, 2, 1.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // 終了後、再び右へ進む
                        rb.velocity = Vector2.right * speed;
                        isKnockback = false;
                    })
                    .SetLink(gameObject);
            }
        }

        // ===== 城に当たった =====
        if (collision.CompareTag("Castle") && canAttack)
        {
            canAttack = false; // 連続攻撃防止

            // 城にダメージ
            Castle castle = collision.GetComponent<Castle>();
            if (castle != null)
            {
                castle.TakeDamage(1);
            }

            // ちょっとだけ左にずらす
            transform.position += Vector3.left * 0.05f;

            // 攻撃クールタイム開始
            StartCoroutine(AttackCooldown());
        }
    }

    // ===== 攻撃クールタイム =====
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }

    // ===== ダメージ時の点滅演出 =====
    IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        sr.color = Color.white;
    }

    // ===== ダメージ処理 =====
    void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("EnemyHP: " + currentHP);

        // ダメージ音
        if (damageSE != null)
        {
            audioSource.PlayOneShot(damageSE);
        }

        // HPが0以下で死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // ===== 死亡処理 =====
    void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }

    // ===== 未使用（必要なら使える） =====
    IEnumerator ResumeMovement()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.right * speed;
    }
}