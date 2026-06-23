using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEffectController : MonoBehaviour
{
    // 右側に出すエフェクト（ParticleSystem）
    public ParticleSystem rightEffect;

    // 左側に出すエフェクト（ParticleSystem）
    public ParticleSystem leftEffect;

    //地面から出るエフェクト
    public ParticleSystem desserteffect;

    // プレイヤーが「動き始めた瞬間」に呼ばれる想定の関数
    // 右・左それぞれのエフェクトを再生する
    public void PlayEffect()
    {
        // 右エフェクトが設定されていて、まだ再生されていない場合だけ再生
        if (rightEffect != null && !rightEffect.isPlaying)
            rightEffect.Play();

        // 左エフェクトも同じく再生
        if (leftEffect != null && !leftEffect.isPlaying)
            leftEffect.Play();

        //
        if(desserteffect != null && !desserteffect.isPlaying)
            desserteffect.Play();


    }

    // プレイヤーが「止まった瞬間」に呼ばれる想定の関数
    // 右・左のエフェクトを停止する
    public void StopEffect()
    {
        // 右エフェクトが設定されていて、再生中なら停止
        if (rightEffect != null && rightEffect.isPlaying)
            rightEffect.Stop();

        // 左エフェクトも同じ
        if (leftEffect != null && leftEffect.isPlaying)
            leftEffect.Stop();

        //
        if (desserteffect != null && desserteffect.isPlaying)
            desserteffect.Stop();
    }
}