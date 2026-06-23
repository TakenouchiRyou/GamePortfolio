using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject PlyerPerfab;
    public GameObject PlyerPerfab2;
    public float _span = 10.0f; //playerが生成される間隔(単位：秒)
    public float _delta;//直前にplayerが生成されてから経過した時間

    public int SummonCost = 3;//召喚コスト
    public int SummonCost2 = 6;//召喚コスト
    public Directo630 scoreManager; // スコア管理スクリプトへの参照

    void Start()
    {
    }


    void Update()
    {
        if (scoreManager != null && scoreManager.cost >= SummonCost)
        {
            //scoreManager.SpendScore(SummonCost);
            //GameObject go = Instantiate(PlyerPerfab);
            //go.transform.position = new Vector3(3.65f, -0.82f, 0.0f);
        }
        else if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager が設定されていません。");
        }

        if (scoreManager != null && scoreManager.cost >= SummonCost2)
        {
            //scoreManager.SpendScore(SummonCost);
            //GameObject go = Instantiate(PlyerPerfab);
            //go.transform.position = new Vector3(3.65f, -0.82f, 0.0f);
        }
        else if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager が設定されていません。");
        }
    }

    public void Summon()
    {
        if (scoreManager.cost >= SummonCost)
        {
            scoreManager.SpendScore(SummonCost);
            GameObject go = Instantiate(PlyerPerfab);
            go.transform.position = new Vector3(3.65f, -0.82f, 0.0f);
        }
    }

    public void Summon(GameObject player)
    {
        var cost = player.GetComponent<SummonCost>();

        if (cost != null)
        {
            int summonCost = cost.summonCost;

            if (scoreManager.cost >= summonCost)
            {
                scoreManager.SpendScore(summonCost);
                GameObject go = Instantiate(player);
                go.transform.position = new Vector3(3.65f, -0.82f, 0.0f);
            }
        }
    }
}
