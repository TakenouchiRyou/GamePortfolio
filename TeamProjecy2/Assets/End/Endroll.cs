using UnityEngine;
using UnityEngine.SceneManagement;

public class Endroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 100f;
    [SerializeField] float endY = 1200f;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // エンドロールを上へ移動
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // 最後まで流れたらシーン移動
        if (rectTransform.anchoredPosition.y >= endY)
        {
            SceneManager.LoadScene("endless", LoadSceneMode.Single);
        }

        // Enterキーでスキップ
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("endless", LoadSceneMode.Single);
        }
    }
}