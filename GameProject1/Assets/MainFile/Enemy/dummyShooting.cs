using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyShooting : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform muzzle;            // 弾が出る位置
    public GameObject bulletPrefab;     // 弾のプレハブ
    public float bulletSpeed = 50f;     // 弾の速度
    public float shootDistance = 1000f; // Raycast距離
    public float fireRate = 1.0f;       // 発射間隔

    [Header("Target")]
    public Transform player;            // 攻撃対象（プレイヤー）

    [Header("Ammo Settings")]
    public int maxBullets = 30;         //最大弾数
    public float cooldownTime = 20f;    //リロード時間

    [Header("Homing Settings")]
    public float dummyHomingPower = 1.0f; //敵の弾の強さ


    private EnemyHealth enemy;          //EnemyHealth を取得
    private int currentBullets;         // 現在の弾数
    private bool isCooldown = false;    // リロード中か
    private float nextFireTime = 0f;    // 次に撃てる時間

    private Animator anim;             //Animatorの追加

    void Start()
    {
        // 弾数を最大値にリセットする
        currentBullets = maxBullets;

        //Animatorの取得
        anim = GetComponent<Animator>();

        //EnemyHealthを取得
        enemy = GetComponentInParent<EnemyHealth>();

        // "Player" タグが付いたオブジェクトをシーンから探す
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Target");

        // 見つかった場合は、その Transform を player にセットする
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }


    void Update()
    {
        Debug.Log(enemy.canShoot);

        if (player == null) return;

        if (enemy == null) return;

        // 索敵外なら必ずIdle
        if (!enemy.canShoot)
        {
            if (anim != null)
                anim.SetBool("isShooting", false);

            return;
        }

        // リロード中
        if (isCooldown)
        {
            if (anim != null)
                anim.SetBool("isShooting", false);
            return;
        }

        // 射撃アニメ
        if (anim != null)
            anim.SetBool("isShooting", true);

        // 発射タイミング
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }
    void Shoot()
    {
        //弾がなければリロードする
        if (currentBullets <= 0)
        {
            StartCoroutine(Cooldown());
            return;
        }
        currentBullets--;

        //playerの方向を計算
        Vector3 dirToPlayer = (player.position - this.transform.position).normalized;

        Ray ray = new Ray(transform.position, dirToPlayer);
        RaycastHit hit;
        Vector3 targetPoint;

        // Rayが何かに当たった場合
        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = transform.position + dirToPlayer * shootDistance;
        }

        //銃口からターゲット方向へ
        Vector3 dir = (targetPoint - muzzle.position).normalized;

        //弾の生成
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            Quaternion.LookRotation(dir)
        );

        //弾の速度設定
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = dir * bulletSpeed;
        }

        //追尾設定
        Bulletcontroller bc = bullet.GetComponent<Bulletcontroller>();

        if (bc != null)
        {
            bc.SetTarget(player);        //追尾するターゲット
            bc.SetOwner("Enemy");     // ←ここに入れる 
            bc.SetSpeed(bulletSpeed);   //弾の速さ
            bc.rotateSpeed = dummyHomingPower;
        }
        // 弾を撃ち切ったらリロード
        if (currentBullets <= 0)
        {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;

        //リロード中は射撃アニメーション停止
        if (anim != null)
            anim.SetBool("isShooting", false);

        yield return new WaitForSeconds(cooldownTime);

        currentBullets = maxBullets;
        isCooldown = false;
    }
}
