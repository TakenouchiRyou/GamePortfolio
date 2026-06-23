using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraSyn : MonoBehaviour
{
    // プレイヤーの向きを同期したい Cinemachine FreeLook カメラ
    public CinemachineFreeLook freeLookCamera;

    private void Update()
    {
        // カメラが設定されていなければ何もしない
        if (freeLookCamera == null) return;

        // FreeLook カメラの X 軸（横回転）の値を取得
        // これはカメラの左右回転（Yaw）を表す
        float CameraYaw = freeLookCamera.m_XAxis.Value;

        // 現在のオブジェクト（プレイヤーなど）の回転角を取得
        Vector3 euler = transform.eulerAngles;

        // Y軸（左右向き）だけカメラの向きに合わせる
        euler.y = CameraYaw;

        // 回転を反映
        transform.eulerAngles = euler;
    }
}
