using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSurvive : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.surviveAtOneHP = true;
            Destroy(gameObject);
            Debug.Log("バフを拾った（耐え）");
        }
    }
}
