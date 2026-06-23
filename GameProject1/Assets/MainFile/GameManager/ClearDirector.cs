using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDirector : MonoBehaviour
{
        // スタート画面のSTARTボタンから呼ぶ
        public void GoToGame()
        {
            SceneManager.LoadScene("Prototype");
        }

        // クリア・ゲームオーバー画面のタイトルに戻るボタンから呼ぶ
        public void GoToTitle()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
