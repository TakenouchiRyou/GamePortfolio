using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform muzzle;              // 銃口の Transform
    public GameObject bulletPrefab;       // 発射する弾のプレハブ
    public float bulletSpeed = 50f;       // 弾の速度
    public float shootDistance = 1000f;   // Raycast の最大距離
    public float fireRate = 0.1f;         // 連射間隔（秒）

    [Header("Ammo Settings")]
    public int maxBullets = 30;           // 装弾数（30発）
    public float cooldownTime = 10f;      // 撃ち切った後のクールタイム（秒）

    [Header("Audio")]
    public AudioSource audioSource;       //音を鳴らす　audioSource
    public AudioClip fireSound;           //音1：弾が出てる時
    public AudioClip emptySound;          //音2：弾切れ

    [Header("UI")]
    public Text ammoText;                 // 弾数表示用 UI（Legacy Text）
    public LayerMask enemyLayer;          //敵レイヤ

    private int currentBullets;           // 現在の弾数
    private bool isCooldown = false;      // クールタイム中かどうか
    private float nextFireTime = 0f;      // 次に撃てる時間

    void Start()
    {
        currentBullets = maxBullets;      // 弾数を満タンに
        UpdateAmmoText();                 // UI 初期表示
    }

    void Update()
    {
        // クールタイム中は撃てない
        if (isCooldown) return;

        //もし弾切れ状態でクリックされたときに音2を鳴らして終了
        if(currentBullets <= 0 && Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(emptySound);
            return;
        }

        // 左クリック押しっぱなし & 発射レートの時間を超えたら発射
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;  // 次に撃てる時間を更新
            Shoot();
        }
    }

    void Shoot()
    {
        // 弾が無い → クールタイムへ
        if (currentBullets <= 0)
        {
            StartCoroutine(Cooldown());
            return;
        }
        
        // 弾を1発消費
        currentBullets--;
        UpdateAmmoText();                 // UI 更新

        // 発射音（音1）
        audioSource.PlayOneShot(fireSound);
        // プレイヤーの正面方向に Ray を飛ばす
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        Vector3 targetPoint;

        // Ray が何かに当たった場合 → その地点を狙う
        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            // 何にも当たらなければ最大距離先をターゲットに
            targetPoint = transform.position + transform.forward * shootDistance;
        }

        // 銃口からターゲットへの方向ベクトル
        Vector3 dir = (targetPoint - muzzle.position).normalized;

        // 弾を生成（向きはターゲット方向）
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            Quaternion.LookRotation(dir)
        );



        // 弾に速度を与える
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = dir * bulletSpeed;
        }


        //一番近い敵を追尾
        Transform enemy = GetNearestEnemy();

        // 弾の追尾スクリプトへターゲットを渡す
        Bulletcontroller bc = bullet.GetComponent<Bulletcontroller>();
        if (bc != null)
        {
            bc.SetTarget(enemy);   // enemy が null でも渡すだけなのでOK
            bc.SetOwner("Player");
            bc.SetSpeed(bulletSpeed);
        }
        //打ち切った後はクールタイムへ
        if (currentBullets <= 0)
        {
            StartCoroutine(Cooldown());
        }
    }

    //一番近い敵を探す
    Transform GetNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 100f, enemyLayer);
        if (enemies.Length ==0)return null;

        Transform nearest = enemies[0].transform;
        float minDist = Vector3.Distance(transform.position, nearest.position);

        foreach(var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }
        return nearest;
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;                // クールタイム開始
        Debug.Log("クールタイム開始");
        ammoText.text = "Reloading...";   // UI にリロード中と表示

        yield return new WaitForSeconds(cooldownTime);

        Debug.Log("クールタイム終了");
        currentBullets = maxBullets;      // 弾を満タンに戻す
        isCooldown = false;               // クールタイム終了
        UpdateAmmoText();                 // UI 更新
    }

    // 弾数 UI を更新する
    void UpdateAmmoText()
    {
        ammoText.text = $"{currentBullets} / {maxBullets}";
    }
}