using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class playerMOVE : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;   // 最大HP
    [SerializeField] float playerHP;     // 現在のHP

    [Header("Death Effect")]
    public GameObject deathEffectPrefab;  // 死亡時に出すエフェクト

    [Header("playersetting")]
    [SerializeField] private float speed = 5f;   // プレイヤーの移動速度
    private Rigidbody rb;                        // Rigidbody 参照用
    Animator _anim;                              // Animator 参照用
    private bool isDead = false;                 //playerの生死判定

    [Header("AudioSource")]
    public AudioSource oneShotSource;          //最初の1回だけ鳴らす
    public AudioSource loopSource;             //2回目鳴らす（ループする）

    public float fadeTime = 0.25f;             //フェードイン時間

    private bool wasMoving = false;             //  前のフレームの移動状態
    private Coroutine fadeInCoroutine;         //  1回目の音が流れたか（流れたかを判別）

    [Header("Effect")]
    public RightEffectController rightEffect;
    public RightEffectController leftEffect;

    [Header("Camerasetting")]
    public Camera  Ca;
    [Header("LOOKON")]
    public float LookonDisutans = 10f;
    public float LookonAngle = 45f;
    [Header("Enemy")]
    public Transform Enemy  = null;

    [Header("HP UI")]
    public Text hpText;

    [Header("Quick Boost UI")]
    public Slider boostGaugeSlider;  // Unity UIのSlider


    [Header("Quick Boost Gauge")]
    public float maxBoostGauge = 100f;      // 最大ゲージ
    public float boostCost = 30f;           // 1回のQB消費量
    public float boostRegenRate = 20f;      // 1秒あたりの回復量

    [SerializeField] private float currentBoostGauge; // 現在のゲージ

    [Header("Quick Boost")]
    public float boostDistance = 6f;      // QB距離
    public float boostTime = 0.15f;       // QB時間
    public float boostCooldown = 1.0f;    // クールダウン

    private bool isBoosting = false;
    private float lastBoostTime = -10f;

    void Start()
    {

        playerHP = maxHP;                        // ゲーム開始時にHPを最大値にする
        currentBoostGauge = maxBoostGauge;      //ブーストゲージの初期化

        rb = GetComponent<Rigidbody>();          // Rigidbody を取得
        rb.freezeRotation = true;                // 物理演算で勝手に回転しないように固定
        _anim = GetComponent<Animator>();        // Animator を取得

       loopSource.loop = true;                   //オンにしておく
        UpdateHPText();                         // ←追加
    }

    private void Update()
    {

        if (isDead) return;
        if (Time.timeScale == 0f) return;

        float x = Input.GetAxis("Horizontal");　 // 入力取得（WASD / 十字キー）
        float z = Input.GetAxis("Vertical");     // 入力取得（WASD / 十字キー）

      
        if (Input.GetKeyDown(KeyCode.LeftShift))   // クイックブースト入力
        {
            TryQuickBoost(x, z);
        }

        if (!isBoosting && currentBoostGauge < maxBoostGauge)　// QBゲージ回復
        {
            currentBoostGauge += boostRegenRate * Time.deltaTime;
            currentBoostGauge = Mathf.Min(currentBoostGauge, maxBoostGauge);
        }

        // ゲージ更新
        if (boostGaugeSlider != null)
        {
            boostGaugeSlider.value = currentBoostGauge / maxBoostGauge;
        }

        Vector3 CF = Ca.transform.forward;
        CF.y = 0f;
       
        CF = CF.normalized;

        float t = Mathf.Clamp01(5f * Time.deltaTime);

        bool isLockedOn = false;

        // ロックオン処理
        if (Enemy != null)
        {
            Vector3 targetdirection = (Enemy.position - transform.position).normalized;

            // 敵が距離・角度の範囲内にいる場合ロックオン
            if (Vector3.Distance(transform.position, Enemy.position) < LookonDisutans
                && Vector3.Angle(targetdirection, Ca.transform.forward) < LookonAngle)
            {
                Vector3 lookDir = Enemy.position - transform.position;
                lookDir.y = 0f;
                transform.forward = lookDir.normalized; // 敵の方向を向く
                isLockedOn = true;                      // ロックオン中フラグを立てる
            }
        }

        // ロックオン中でない時だけカメラ方向に同期する
        if (!isLockedOn)
        {
            transform.forward = Vector3.Lerp(transform.forward, CF, t);
        }


        Vector3 inputDirection = new Vector3(x, 0, z);　// 入力方向ベクトルを作成

       
        inputDirection = Camera.main.transform.TransformDirection(inputDirection); // カメラの向きに合わせて移動方向を変換
        inputDirection.y = 0; // 上下方向の傾きを無視して水平移動に限定

        bool isMoving  = inputDirection.sqrMagnitude > 0.01f;//動いているなら true 止まっているなら false

        //ブースター音の処理
        if (isMoving)
        {
            
            if (!wasMoving)　//もし押した瞬間（静止状態だった時）
            {
                //1回目の音を再生する
                oneShotSource.Stop();
                oneShotSource.Play();

                //ループ音を小音で再生しフェードイン開始（自然になるように）
                loopSource.volume = 0f;
                loopSource.Play();

                if(fadeInCoroutine != null)
                    StopCoroutine(fadeInCoroutine);

                fadeInCoroutine = StartCoroutine(FadeInLoopSound());

                //動き始めた時にエフェクト再生（右）
                if (rightEffect != null) rightEffect.PlayEffect();
                if (leftEffect != null) leftEffect.PlayEffect();
            }

        }
       else 
        {
            //離した瞬間にすべて停止
            if (wasMoving)
            {
                oneShotSource.Stop();
                loopSource.Stop();

                //止まった時にエフェクト停止
                if (rightEffect != null) rightEffect.StopEffect();
                if (leftEffect != null) leftEffect.StopEffect();
            }
            if (fadeInCoroutine != null)
                StopCoroutine(fadeInCoroutine);
        }
        wasMoving = isMoving;

        if (inputDirection.sqrMagnitude > 0)
        {
            // 入力があるとき：移動する
            rb.velocity = inputDirection.normalized * speed;

            // プレイヤーの向きを移動方向に合わせる
            this.transform.forward = inputDirection;
        }
        else
        {
            // 入力がないとき：完全に停止させる
            rb.velocity = Vector3.zero;
        }

        // アニメーション用パラメータに速度を渡す
        _anim.SetFloat("Speed", rb.velocity.magnitude);

        // 左クリックを押している間だけ fireIdle を true
        _anim.SetBool("fireIdle", Input.GetMouseButton(0));
    }
    //ループ音のフェードイン用
    IEnumerator FadeInLoopSound()
    {
        float t = 0f;
        float start = 0f;
        float end = 0.1f;  // 最終音量

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            loopSource.volume = Mathf.Lerp(start, end, t / fadeTime);
            yield return null;
        }

        loopSource.volume = end;
    }

    // ダメージ処理
    public void TakeDamage(float amount)
    {
        // HPを減らす
        playerHP -= amount;

        UpdateHPText();   // ←追加

        // HPが0以下になったら死亡
        if (playerHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = "AP " + playerHP.ToString();
        }
    }

    void Die()
    {
        isDead = true;

        if (_anim != null)
            //死亡アニメ再生
            _anim.SetTrigger("Die");

        // 死亡エフェクト
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

        }

        // ゲームオーバーシーンへ遷移
        FindObjectOfType<GameOver>().TriggerGameOver();

        //3秒後に敵削除（アニメ再生のため)        
        Destroy(gameObject, 3f);
    }

    void TryQuickBoost(float x, float z)
    {
        if (isBoosting) return;
        if (currentBoostGauge < boostCost) return;  // ゲージ不足で使用不可
        if (Time.time - lastBoostTime < boostCooldown) return;

        Vector3 dir = new Vector3(x, 0, z);

        //入力がない時は前にQB
        if(dir.sqrMagnitude < 0.01f)
            dir = transform.forward;
        else
            dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;
        dir.Normalize();

        currentBoostGauge -= boostCost;  // ゲージ消費

        StartCoroutine(QuickBoost(dir));
    }
    IEnumerator QuickBoost(Vector3 dir)
    {
        isBoosting = true;
        lastBoostTime = Time.time;

        float timer = 0f;

        while (timer < boostTime)
        {
            rb.velocity = dir * (boostDistance / boostTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isBoosting=false;
    }
}