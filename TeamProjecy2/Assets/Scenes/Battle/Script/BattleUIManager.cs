using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleUIManager :
MonoBehaviour
{

    public bool
    isHistoryOpen =
    false;

    [Header("参照")]
    public BattlePlayerStats
        player;

    public EnemyStats
        enemy;

    public TurnManager
        turnManager;


    [Header("戦闘UI")]
    public TextMeshProUGUI
        playerHPText;

    public TextMeshProUGUI
        enemyHPText;

    public TextMeshProUGUI
        actionLogText;

    public TextMeshProUGUI
        turnText;


    [Header("ログ履歴")]
    public GameObject
        historyPanel;

    public TextMeshProUGUI
        historyText;

    public ScrollRect
    historyScroll;

    // --- 履歴保存 ---
    List<string>
        logHistory =
        new List<string>();


    // --- UI更新 ---
    public void RefreshUI()
    {
        playerHPText.text =
            "HP "
            +
            player.currentHP
            +
            "/"
            +
            player.maxHP;


        enemyHPText.text =
            "HP "
            +
            enemy.currentHP
            +
            "/"
            +
            enemy.maxHP;
    }


    // --- ターン更新 ---
    public void RefreshTurn()
    {
        turnText.text =
            "TURN "
            +
            turnManager.turnCount;
    }


    // --- ログ表示 ---
    public void ShowActionLog(
        string message)
    {
        string log =

            "TURN "

            +

            turnManager
            .turnCount

            +

            "\n"

            +

            message;


        actionLogText.text =
            log;


        logHistory
        .Add(
            log);


        // --- 上限 ---
        if (
            logHistory
            .Count
            >
            100)
        {
            logHistory
            .RemoveAt(
                0);
        }
    }


    // --- 履歴開く ---
    public void OpenHistory()
    {
        historyPanel
    .SetActive(
        true);

        isHistoryOpen =
            true;

        historyText.text =
            "";

        foreach (
            string log
            in logHistory)
        {
            historyText.text
            +=
            log
            +
            "\n\n";
        }


        // 一番下へ
        Canvas
        .ForceUpdateCanvases();

        historyScroll
        .verticalNormalizedPosition =
            0f;
    }


    // --- 履歴閉じる ---
    public void CloseHistory()
    {
        historyPanel
        .SetActive(
            false);

        isHistoryOpen =
        false;
    }


    // --- 開始 ---
    void Start()
    {
        RefreshUI();

        RefreshTurn();


        historyPanel
        .SetActive(
            false);
    }

    void Update()
    {
        // --- ESCで開閉 ---
        if (
            Input.GetKeyDown(
                KeyCode.Q))
        {
            if (
                historyPanel
                .activeSelf)
            {
                CloseHistory();
                // 閉じる
            }

            else
            {
                OpenHistory();
                // 開く
            }
        }


        // --- 閉じてたら終了 ---
        if (
            !
            historyPanel
            .activeSelf)
        {
            return;
        }


        // --- 上スクロール ---
        if (
            Input.GetKey(
                KeyCode.W))
        {
            historyScroll
            .verticalNormalizedPosition
            +=
            Time.deltaTime;
        }


        // --- 下スクロール ---
        if (
            Input.GetKey(
                KeyCode.S))
        {
            historyScroll
            .verticalNormalizedPosition
            -=
            Time.deltaTime;
        }
    }
}