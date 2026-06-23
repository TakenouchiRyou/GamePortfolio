using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameClear : MonoBehaviour
{
    [Header("設定")]
    public string nextScneName = "ClearScene";//次のシーン
    public float delay = 3.5f; //シーンが切り替わるまでの時間
    
    private bool isChanging = false;

    public void ChangeScene()
    {
        if(isChanging)return;

        isChanging = true;
        StartCoroutine(ChangeSceneCoroutine());
    }

    IEnumerator ChangeSceneCoroutine()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScneName);
    }

}
