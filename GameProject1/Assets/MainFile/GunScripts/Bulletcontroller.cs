using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletcontroller : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float rotateSpeed = 5f;  //弾の追尾の強さ（小さくすると自然に見える）
    public float lifetime = 5f;     // 自動消滅までの時間
    public float damage = 10f;      // 弾が与えるダメージ

    private Transform target;      //追尾する対象
    private Rigidbody rb;          //Rigidbody
    private float moveSpeed;       //弾の移動スピード

    private string ownerTag;
    public void SetOwner(string tagName)
    {
        ownerTag = tagName;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //弾のすり抜けの防止
        if(rb != null )
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetSpeed(float s)
    {
        moveSpeed = s;
    }

    private void Update()
    {
        //ターゲットがいれば向きを補正
        if (target != null)
        {
            //ターゲットの方向
            Vector3 dir = (target.position - transform.position).normalized;
            //ターゲットの方向への回転
            Quaternion targetRot = Quaternion.LookRotation(dir);
            //少しずつターゲットの方向への回転
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // 前方へ一定速度で進む
        if (rb != null && moveSpeed > 0f)
        {
            rb.velocity = transform.forward * moveSpeed;    
        }
    }
    //void OnCollisionEnter(Collision collision)
    //{
    //    // 当たった相手から EnemyHealth を探す
    //    EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();

    //    if (health != null)
    //    {
    //        health.TakeDamage(damage);
    //    }

    //    // 弾を消す
    //    Destroy(gameObject);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(ownerTag) && other.CompareTag(ownerTag)) return;
        // 当たった相手から EnemyHealth を探す
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponentInParent<EnemyHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        //当たった相手からplayerMOVEを探す
        if (other.CompareTag("Player"))
        {
            playerMOVE player = other.GetComponentInParent<playerMOVE>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        // 弾を消す
        Destroy(gameObject);
    }
}
