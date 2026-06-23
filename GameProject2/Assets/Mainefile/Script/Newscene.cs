using UnityEngine;
using UnityEngine.SceneManagement;

public class Newscene : MonoBehaviour
{
    [Header("遷移先シーン名")]
    [SerializeField] private string sceneName;

    [Header("クリックで遷移するか")]
    [SerializeField] private bool clickToLoad = true;

    void Update()
    {
        if (clickToLoad && Input.GetMouseButtonDown(0))
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}