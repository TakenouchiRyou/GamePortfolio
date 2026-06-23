using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 保存された位置を復元
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            transform.position = new Vector3(x, y, 0f);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // ポーズ中は動かない

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? dashSpeed : moveSpeed;
        rb.velocity = new Vector2(x, y).normalized * speed;

        // 移動アニメーション
        if (x != 0 || y != 0)
        {
            anim.SetBool("isRun", true);

            // 左右の向きを変える
            if (x < 0)
            {
                direction = -1;
                transform.localScale = new Vector3(direction, 1, 1);
            }
            else if (x > 0)
            {
                direction = 1;
                transform.localScale = new Vector3(direction, 1, 1);
            }
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }
}