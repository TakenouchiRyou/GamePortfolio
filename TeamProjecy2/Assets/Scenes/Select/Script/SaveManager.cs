using UnityEngine;

public class SaveManager :
MonoBehaviour
{
    public static
        SaveManager
        instance;

    // 初期化
    void Awake()
    {
        if (
            instance
            ==
            null)
        {
            instance =
                this;


            DontDestroyOnLoad(
                gameObject);
        }


        else
        {
            Destroy(
                gameObject);
        }
    }

    // 全強化保存
    public void Save()
    {
        foreach (
            UpgradeType type
            in
            System.Enum.GetValues(
                typeof(
                    UpgradeType)))
        {
            int value =

                GameManager
                .instance
                .GetUpgrade(
                    type);

            PlayerPrefs
            .SetInt(
                type
                .ToString(),

                value);
        }

        // 階層保存
        PlayerPrefs
        .SetInt(
            "Floor",

            GameManager
            .instance
            .floor);

        PlayerPrefs
        .Save();

        Debug.Log(
            "保存完了");
    }

    // 全強化読込
    public void Load()
    {
        foreach (
            UpgradeType type
            in
            System.Enum.GetValues(
                typeof(
                    UpgradeType)))
        {
            int value =

                PlayerPrefs
                .GetInt(
                    type
                    .ToString(),

                    0);

            GameManager
            .instance
            .upgradeLevels
            [
                type
            ]
            =
            value;
        }

        // 階層読込
        GameManager
        .instance
        .floor

        =

        PlayerPrefs
        .GetInt(
            "Floor",

            1);

        Debug.Log(
            "ロード完了");
    }

    // データ削除
    public void DeleteSave()
    {
        PlayerPrefs
            .DeleteAll();

        Debug.Log(
            "セーブ削除");
    }
}