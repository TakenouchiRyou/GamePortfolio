using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // プレイヤーの移動
    public float moveSpeed = 5f;

    public bool canMove = true; // 動けなく

    private Rigidbody2D rb;
    private Vector2 moveInput;

    Animator animator;

    void Start()
    {
        //取得
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            animator.SetBool("IsMove", false);
            return;
        }
        //移動入力
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // キャラが移動しているかどうか
        moveInput = new Vector2(moveX, moveY).normalized;

        animator.SetBool("IsMove", moveInput != Vector2.zero);
        // プレイヤーが移動している時だけ向きを更新
        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
            Debug.Log($"MoveX:{moveInput.x} MoveY:{moveInput.y}");
        }
    }

    void FixedUpdate()
    {
        // 移動
        rb.velocity = moveInput * moveSpeed;
    }
}