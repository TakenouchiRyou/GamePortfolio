using TMPro;
using UnityEngine;

public class DamageText :
MonoBehaviour
{
    public TextMeshProUGUI
        textMesh;

    float timer = 1f;

    Vector3 move =
        new Vector3(
            0,
            80f,
            0);

    public void Setup(
        int damage)
    {
        textMesh.text =
            damage
            .ToString();
    }

    void Update()
    {
        transform.position
            +=
            move
            *
            Time.deltaTime;

        timer
            -=
            Time.deltaTime;

        if (
            timer
            <=
            0)
        {
            Destroy(
                gameObject);
        }
    }
}