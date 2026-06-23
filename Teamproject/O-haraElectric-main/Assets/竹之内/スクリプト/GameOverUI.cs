using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("戻るシーン名")]
    public string titleSceneName = "Title";

    [Header("決定SE")]
    public AudioSource selectSE;

    [Header("進行度リセット")]
    public string[] clearKeys;

    private bool isMoving = false;

    void Start()
    {
        foreach (string key in clearKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (!isMoving && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            ReturnTitle();
        }
    }

    // タイトルへ戻る
    public void ReturnTitle()
    {
        StartCoroutine(ReturnTitleCoroutine());
    }

    IEnumerator ReturnTitleCoroutine()
    {
        isMoving = true;
        // SE再生
        selectSE.Play();
        // 少し待つ
        yield return new WaitForSeconds(1f);
        // シーン移動
        SceneManager.LoadScene(titleSceneName);
    }
}