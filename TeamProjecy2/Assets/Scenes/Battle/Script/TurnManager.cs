using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    [Header("ターン")]
    public int turnCount = 0;
    public BattleUIManager uiManager;

    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn,
        Busy
    }

    public TurnState currentTurn;

    [Header("参照")]
    public DeckManager deckManager;

    public BattlePlayerStats playerStats;

    public EnemyStats enemyStats;

    public EnemyAI enemyAI;

    // 戦闘開始
    void Start()
    {
        StartBattle();
    }

    // 戦闘開始
    void StartBattle()
    {
        deckManager.StartBattle();

        StartPlayerTurn();
    }

    // プレイヤーターン開始
    public void StartPlayerTurn()
    {
        playerStats.defense = 0;

        currentTurn =
            TurnState.PlayerTurn;

        turnCount++;

        deckManager.StartPlayerTurn();

        uiManager.RefreshTurn();

        Debug.Log(
            "ターン "
            + turnCount
        );
    }

    // プレイヤーターン終了
    public void EndPlayerTurn()
    {
        if (currentTurn
            != TurnState.PlayerTurn)
            return;

        playerStats.EndTurn();

        currentTurn =
            TurnState.Busy;

        StartCoroutine(
            EnemyTurnCoroutine());
    }

    // 敵ターン
    IEnumerator EnemyTurnCoroutine()
    {
        currentTurn =
            TurnState.EnemyTurn;

        Debug.Log("敵ターン");

        yield return
            new WaitForSeconds(1f);


        enemyAI.EnemyTurn();


        yield return
            new WaitForSeconds(1f);


        enemyStats.EndTurn();

        StartPlayerTurn();
    }

    // プレイヤー操作可能？
    public bool CanPlayerAct()
    {
        return currentTurn
            == TurnState.PlayerTurn;
    }
}