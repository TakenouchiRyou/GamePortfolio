using UnityEngine;
using UnityEngine.SceneManagement;

public class roll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 100f;
    [SerializeField] float endY = 1200f;

    // Inspectorから設定するシーン名
    public string nextSceneName = "endless";

    private RectTransform rectTransform;

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
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }

        // Enterキーでスキップ
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}