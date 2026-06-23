using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter :
MonoBehaviour
{
    private void
    OnTriggerEnter2D(
        Collider2D other)
    {
        if (
            other.CompareTag(
                "Player"))
        {
            Debug.Log(
                "í“¬ŠJn");


            SceneManager
            .LoadScene(
                "BattleScene");
        }
    }
}