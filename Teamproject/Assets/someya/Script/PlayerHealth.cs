using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = Vector2.zero;
            SceneManager.LoadScene("someya/GameOverScreen");
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = Vector2.zero;
            SceneManager.LoadScene("someya/GameOverScreen");
        }
    }
}