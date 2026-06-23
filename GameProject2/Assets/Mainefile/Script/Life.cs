using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Life : MonoBehaviour
{
    float _life = 100;
    float _MAXlife = 100;
    public Image _gaige;
    public void ChangeLife(float amount)
    {
        float originalLife = _life / _MAXlife;
        _life = Mathf.Clamp(_life + amount, 0, _MAXlife);
        float targetLife = _life / _MAXlife;
        //_gaige.fillAmount = _life / _MAXlife;

        DOTween.To(() => originalLife,            //初期値
            (l) => _gaige.fillAmount = l,          //値の変化に応じてやってほしい処理を書く
                                targetLife,       //
                               1                 //最終値
                                                //かける時間（秒）
            );
    }
}
