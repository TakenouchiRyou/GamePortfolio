using Unity.Mathematics;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;   // 最大HP
    [SerializeField]
    private float currentHP;     // 現在のHP

    [Header("Death Effect")]
    public GameObject deathEffectPrefab;  // 死亡時に出すエフェクト



    [Header("Look At Player")]
    public Transform player;      // プレイヤーのTransform(位置）
    public float lookRange = 10f;  // プレイヤーを認識する距離
    public float StopDistance = 2f;//プレイヤーと敵の距離

    [Header("Movement")]
    public float MoveSpeed = 3f;   //移動速度
    public float rotateSpeed = 5f; // 回転スピード

    [Header("Evasive Motion")]
    public float lateralStrength = 2f; //(lateralStrength ラタラル・ストレングス)//横揺れの強さ
    public float noiseFrequency = 1.5f;   //揺れた時のスピード
    public float swayMultiplier = 3f;  // 揺れの大きさの倍率(ここをいじれば揺れが大きくなる)

    [Header("Lose Player")]
    public float loseTime = 3f;

    [Header("Boost")]
    public float boostDistance = 5.0f; //ブーストしたときの距離
    public float boostCooldown = 4f;　//ブーストのクールダウン
    private bool isBoosting = false; //ブーストフラグ

    private float loseTimer = 0f;
    private float boostTimer = 0f;

    private bool isDead = false;
    public bool canShoot = false;

    private NavMeshAgent agent;
    private Gamemanager gameManager;
    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        // ゲーム開始時にHPを最大値にする
        currentHP = maxHP;

        //Animatorを取得
        anim = GetComponent<Animator>();

        //Rigidbodyを取得
        rb = GetComponent<Rigidbody>();

        gameManager = FindObjectOfType<Gamemanager>();

        //NavMeshAgentを取得
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = MoveSpeed;
            agent.updateRotation = false;
            agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
        }

        // playerが未設定なら、タグ "Player" を持つオブジェクトを自動取得
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }
    
    }

    void Update()
    {

        // 死亡していたら何もしない
        if (isDead) return;

        // プレイヤーが見つかっていない場合は処理しない
        if (player == null) return;

        // 敵とプレイヤーの距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        // 射撃判定
        canShoot = (distance <= lookRange) && CanSeePlayer();

        if (anim != null)
        {
            anim.SetBool("fireIdle", canShoot);
        }

        if (CanSeePlayer())
        {
            loseTimer = 0f;
        }
        else
        {
            loseTimer += Time.deltaTime;
        }

        if (loseTimer > loseTime)
        {
            if (agent != null)
                agent.isStopped = true;

            if (anim != null)
            {
                anim.SetBool("Idle_Loop", false);
                
            }

            canShoot = false;
            return;
        }

        // プレイヤーが探知距離に入ったら行動開始
        if (distance <= lookRange)
        {
            if (agent != null)
                agent.isStopped = false;


            LookAtPlayer();

            //StopDistanceよりも遠い　→　翻弄しながら（左右に動きつつ）追跡
            if (distance > StopDistance)
            {
                ChasePlayer(false);

                //移動モーション
                if (anim != null)
                    anim.SetBool("Idle_Loop", true);
            }
            else
            {

                //近距離は横揺れのみ
                ChasePlayer(true);

                //待機モーション
                if (anim != null)
                    anim.SetBool("Idle_Loop", false);
            }

            BoostCheck();
        }
        else
        {
            if (agent != null)
                agent.isStopped = true;

            if (anim != null)
            {
                anim.SetBool("Idle_Loop", false);
                anim.SetBool("Bool", false);
            }

            canShoot = false;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position + Vector3.up, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lookRange))
        {
            if(hit.transform.CompareTag("Player"))
                return true;
        }
        return false;

    }

    // プレイヤーの方向へゆっくり回転する処理 (AI参照)
    void LookAtPlayer()
    {

        // プレイヤーの方向を求める
        Vector3 direction = player.position - transform.position;

        // 上下の角度は無視して水平だけにする
        direction.y = 0f;

        // ほぼ同じ位置なら回転しない
        if (direction.sqrMagnitude < 0.001f) return;

        // 向くべき回転を作る
        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

        // 今の向きから目標の向きへゆっくり回転
        transform.rotation = Quaternion.Slerp(
            transform.rotation,             // 現在の回転
            lookRotation,                   // 目標の回転
            Time.deltaTime * rotateSpeed    // 補間速度（フレーム依存）

            );
    }

    //左右の移動をしながらの追跡のメイン処理（AI参照）
    void ChasePlayer(bool stopForward)
    {
        if (isBoosting) return;

        // プレイヤー方向（水平）
        Vector3 baseDir = (player.position - transform.position).normalized;
        baseDir.y = 0f;

        // 横方向
        Vector3 right = Vector3.Cross(Vector3.up, baseDir);

        // ノイズ（左右揺れ）
        float noise = Mathf.PerlinNoise(Time.time * noiseFrequency, 0f);
        noise = (noise - 0.5f) * 2f;

        // 前進成分（StopDistance 内では 0）
        Vector3 forward = stopForward ? Vector3.zero : baseDir;

        // 横揺れ（こちらは常に有効）
        Vector3 sway = right * noise * lateralStrength * swayMultiplier;

        // forward をきちんと使う
        Vector3 finalDir = (forward + sway).normalized;

        // 移動
        Vector3 targetPos = transform.position + finalDir * 6;

        if(agent != null)
        {
            agent.SetDestination(targetPos);
        }
    }

    void BoostCheck()
    {
        boostTimer += Time.deltaTime;

        if(boostTimer > boostCooldown)
        {
            BoostEvade();
            boostTimer = 0f;
        }
    }

    void BoostEvade()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, dir);

        float side = UnityEngine.Random.value > 0.5f ? 1f : -1f;

        Vector3 boostPos = transform.position + right * side * boostDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(boostPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.speed = MoveSpeed * 3f;   // ブースト速度
            agent.SetDestination(hit.position);
            Invoke(nameof(ResetSpeed), 0.5f);
        }
    }

    void ResetSpeed()
    {
        agent.speed = MoveSpeed;
    }

    // ダメージ処理
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // HPを減らす
        currentHP -= amount;

        // HPが0以下になったら死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
        }

        if (anim != null)
        {
            //アニメーションの動きを止める
            anim.SetBool("isMoving", false);

            //死亡アニメ再生
            anim.SetTrigger("Die");
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }


        // 死亡エフェクト
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

        }
        //ゲームクリア
        if (gameManager != null)
        {
            gameManager.GameClear();
        }
        //3秒後に敵削除（アニメ再生のため)        
        Destroy(gameObject, 3f);


    }
   
}
