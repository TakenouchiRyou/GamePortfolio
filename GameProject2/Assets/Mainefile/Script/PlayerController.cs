using DG.Tweening;              // DOTween（アニメーション用）
using System.Collections;
using UnityEngine;

// プレイヤー制御クラス
public class PlayerController : MonoBehaviour
{
    // ===== 移動設定 =====
    [Header("移動")]
    public float speed = 5.0f;  // 左に進むスピード

    // ===== HP設定 =====
    [Header("HP")]
    public int maxHP = 3;       // 最大HP
    public int currentHP;       // 現在のHP

    // ===== ダメージ・無敵設定 =====
    [Header("ダメージ設定")]
    public float invincibleTime = 1.5f; // 無敵時間（秒）
    public int flashCount = 5;          // 点滅回数

    // ===== コンポーネント =====
    Rigidbody2D rb;            // Rigidbody2D（物理挙動）
    SpriteRenderer sr;         // 見た目（色変更用）

    // ===== 状態管理 =====
    bool isInvincible = false; // 無敵状態かどうか
    Color originalColor;       // 元の色を保存

    [Header("効果音")]
    public AudioClip damageSE;

    AudioSource audioSource;

    // ゲーム開始時に1回だけ呼ばれる
    void Start()
    {
        // コンポーネント取得
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 元の色を保存（点滅後に戻すため）
        originalColor = sr.color;

        // 初期状態：左に移動開始
        rb.velocity = Vector2.left * speed;

        // HP初期化
        currentHP = maxHP;

        audioSource = GetComponent<AudioSource>();
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // 何かの原因で止まった場合、再び動かす
        // ※無敵中はノックバック演出を優先するので動かさない
        if (rb.velocity.x == 0 && !isInvincible)
        {
            rb.velocity = Vector2.left * speed;
        }
    }

    // トリガーに入ったとき（Enemyと接触）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemyタグで、かつ無敵状態でない場合のみダメージ
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            // ダメージ処理
            TakeDamage(1);

            // 一旦停止（ノックバック前）
            rb.velocity = Vector2.zero;

            // ノックバック（右方向にジャンプ）
            transform.DOJump(
                transform.position + Vector3.right, // 右へ移動
                2f,                                 // ジャンプの高さ
                2,                                  // ジャンプ回数
                0.5f                                // 時間
            )
            .SetEase(Ease.Linear)                   // 一定速度
            .OnComplete(() =>
            {
                // ノックバック終了後、再び左に進む
                rb.velocity = Vector2.left * speed;
            })
            .SetLink(gameObject);                   // オブジェクト削除時にTweenも破棄

            // 無敵処理開始（点滅込み）
            StartCoroutine(InvincibleRoutine());
        }
    }

    // ダメージ処理
    void TakeDamage(int damage)
    {
        // HPを減らす
        currentHP -= damage;

        // 音が入っている時だけ再生
        if (damageSE != null)
        {
            audioSource.PlayOneShot(damageSE);
        }

        // デバッグ表示
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
        // 無敵状態ON
        isInvincible = true;

        // 点滅処理（赤→元色を繰り返す）
        for (int i = 0; i < flashCount; i++)
        {
            // 赤くする
            sr.color = Color.red;

            // 半分の時間待つ
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));

            // 元の色に戻す
            sr.color = originalColor;

            // 残り半分の時間待つ
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));
        }

        // 念のため元の色に戻す
        sr.color = originalColor;

        // 無敵状態OFF
        isInvincible = false;
    }

    // 死亡処理
    void Die()
    {
        Debug.Log("Player has died.");

        // プレイヤー削除（ゲームオーバー処理などに置き換え可）
        Destroy(gameObject);
    }
}