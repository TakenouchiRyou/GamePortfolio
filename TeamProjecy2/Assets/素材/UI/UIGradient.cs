using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class UIGradient : BaseMeshEffect
{
    // 左側の色（グラデーション開始色）
    public Color leftColor = Color.white;

    // 右側の色（グラデーション終了色）
    public Color rightColor = Color.black;

    public override void ModifyMesh(VertexHelper vh)
    {
        // コンポーネントが無効なら処理しない
        if (!IsActive()) return;

        // 頂点データを取得するリスト
        var vertices = new System.Collections.Generic.List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        // X 座標の最小値・最大値を求める
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        for (int i = 0; i < vertices.Count; i++)
        {
            float x = vertices[i].position.x;

            // 最小・最大の X を更新
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
        }

        // 各頂点にグラデーション色を適用
        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex v = vertices[i];

            // 頂点の X 位置を 0?1 に正規化
            float t = (v.position.x - minX) / (maxX - minX);

            // leftColor → rightColor の間で補間
            v.color = Color32.Lerp(leftColor, rightColor, t);

            // 変更した頂点を戻す
            vertices[i] = v;
        }

        // メッシュを書き換え
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }
}
