
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Gamemanager : MonoBehaviour
{
    [Header("GameClear UI")]
    public Text gameClearText;

    [Header("Clear Voice")]
    public AudioSource clearVoice;

    void Start()
    {
        if (gameClearText != null)
            gameClearText.enabled = false;
    }

    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        //敵の死亡アニメーションを待つ
        yield return new WaitForSeconds(1.5f);

        //GAME CLEAR表示
        if(gameClearText != null)
            gameClearText.enabled = true;

        //ボイス再生
        if (clearVoice != null)
            clearVoice.Play();

        //少し余韻を残す
        yield return new WaitForSeconds(1f);

        // クリア画面へ遷移
        FindObjectOfType<GameClear>().ChangeScene();

   

      
    }
}
