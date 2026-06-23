using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        scoreText.text = score.ToString();
    }
}