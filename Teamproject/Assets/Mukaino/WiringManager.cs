using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class WiringManager : MonoBehaviour
{
    [Header("配線設定")]
    public int wireCount = 4;
    public float timeLimit = 30f;

    [Header("ワイヤー描画")]
    public Material wireMaterial;

    private Plug selectedPlug;
    private LineRenderer dragLine;
    private int connectedCount = 0;
    private float timeLeft = 0f;
    private bool isPlaying = false;

    private Camera mainCam;

    public float TimeLeft => timeLeft;
    public int ConnectedCount => connectedCount;

    [Header("ワイヤーマテリアル")]
    public Material wireMat_Red;
    public Material wireMat_Blue;
    public Material wireMat_Yellow;
    public Material wireMat_Purple;

    [Header("効果音")]
    public AudioClip clearSE;
    public AudioClip missSE;

    public AudioSource audioSource;

    [Header("プラグ")]
    public Plug[] leftPlugs;
    public Plug[] rightPlugs;

    [Header("Y座標設定")]
    public float yMin = -2.5f;
    public float yMax = 2.5f;

    [Header("接続判定")]
    public float connectRadius = 0.4f;

    [Header("ルーレットゲージ")]
    public RouletteGauge rouletteGauge;

    [Header("Enemy設定")]
    public float minEnemySpawnTime = 10f;
    public float maxEnemySpawnTime = 20f;
    private float enemySpawnTimer = 0f;
    private bool enemySpawned = false;

    [Header("デバッグ設定")]
    public bool debugMode = false;
    public float debugEnemySpawnTime = 3f;

    private Plug pendingRightPlug;

    void Start()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        StartGame();
    }

    void Update()
    {
        if (!isPlaying) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            GameOver();
        }

        if (!enemySpawned)
        {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0f)
            {
                enemySpawned = true;
                SpawnEnemyAndReturn();
            }
        }

        if (rouletteGauge.isSpinning)
        {
            if (Input.GetMouseButtonDown(0))
                rouletteGauge.StopGauge();
            return;
        }

        if (dragLine != null)
        {
            Vector3 mouseWorld = GetMouseWorldPos();
            SetBezierLine(dragLine, selectedPlug.transform.position, mouseWorld);
        }

        if (Input.GetMouseButtonDown(1))
            CancelSelection();
    }

    public void StartGame()
    {
        connectedCount = 0;
        isPlaying = true;
        enemySpawned = false;

        enemySpawnTimer = debugMode
            ? debugEnemySpawnTime
            : Random.Range(minEnemySpawnTime, maxEnemySpawnTime);

        // WiringResetフラグが立っていたら削除して最初から
        PlayerPrefs.DeleteKey("WiringReset");
        PlayerPrefs.DeleteKey("WiringTimeLeft");
        PlayerPrefs.Save();

        timeLeft = timeLimit;

        ShufflePlugPositions();
    }

    void SpawnEnemyAndReturn()
    {
        isPlaying = false;
        PlayerPrefs.SetInt("EnemySpawnOnReturn", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("染谷/探索画面");
    }

    public void SetPlaying(bool value)
    {
        isPlaying = value;
    }

    void ShufflePlugPositions()
    {
        float[] leftYList = GetEvenYPositions(leftPlugs.Length);
        for (int i = 0; i < leftPlugs.Length; i++)
        {
            Vector3 pos = leftPlugs[i].transform.position;
            pos.y = leftYList[i];
            leftPlugs[i].transform.position = pos;
        }

        float[] rightYList = GetEvenYPositions(rightPlugs.Length);
        Shuffle(rightYList);
        for (int i = 0; i < rightPlugs.Length; i++)
        {
            Vector3 pos = rightPlugs[i].transform.position;
            pos.y = rightYList[i];
            rightPlugs[i].transform.position = pos;

            float randomAngle = Random.Range(0f, 360f);
            rightPlugs[i].transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        }
    }

    float[] GetEvenYPositions(int count)
    {
        float[] result = new float[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = Mathf.Lerp(yMin, yMax, (float)i / (count - 1));
        }
        return result;
    }

    void Shuffle(float[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            float tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = -mainCam.transform.position.z;
        return mainCam.ScreenToWorldPoint(mp);
    }

    public void OnPlugClicked(Plug plug)
    {
        if (!isPlaying) return;
        if (rouletteGauge.isSpinning) return;

        if (plug.isLeft)
        {
            CancelSelection();
            selectedPlug = plug;
            plug.SetSelected(true);
            dragLine = CreateLineRenderer(GetColorFromId(plug.colorId), plug.colorId);
        }
        else
        {
            if (selectedPlug == null) return;

            if (plug.colorId == selectedPlug.colorId)
            {
                pendingRightPlug = plug;
                rouletteGauge.StartGauge(
                    () => Connect(selectedPlug, pendingRightPlug),
                    () =>
                    {
                        Debug.Log("タイミングミス！");
                        audioSource.PlayOneShot(missSE);
                        CancelSelection();
                    }
                );
            }
            else
            {
                Debug.Log("色が違う！");
                audioSource.PlayOneShot(missSE);
                CancelSelection();
            }
        }
    }

    void Connect(Plug left, Plug right)
    {
        left.isConnected = true;
        right.isConnected = true;
        left.SetSelected(false);

        dragLine = null;
        selectedPlug = null;

        connectedCount++;
        if (connectedCount >= wireCount)
            StageClear();
    }

    void CancelSelection()
    {
        if (selectedPlug != null)
        {
            selectedPlug.SetSelected(false);
            selectedPlug = null;
        }
        if (dragLine != null)
        {
            Destroy(dragLine.gameObject);
            dragLine = null;
        }
    }

    LineRenderer CreateLineRenderer(Color color, string colorId)
    {
        var obj = new GameObject("Wire_Drag");
        obj.transform.SetParent(transform);

        var lr = obj.AddComponent<LineRenderer>();
        lr.material = GetMatFromId(colorId);
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 0.3f;
        lr.endWidth = 0.3f;
        lr.positionCount = 20;
        lr.textureMode = LineTextureMode.Tile;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 0;

        return lr;
    }

    void SetBezierLine(LineRenderer lr, Vector3 from, Vector3 to)
    {
        int segments = lr.positionCount;
        Vector3 mid1 = new Vector3(0f, from.y, 0f);
        Vector3 mid2 = new Vector3(0f, to.y, 0f);

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 p = Mathf.Pow(1 - t, 3) * from
                      + 3 * Mathf.Pow(1 - t, 2) * t * mid1
                      + 3 * (1 - t) * Mathf.Pow(t, 2) * mid2
                      + Mathf.Pow(t, 3) * to;
            lr.SetPosition(i, p);
        }
    }

    public Color GetColorFromId(string id) => id switch
    {
        "red" => new Color(0.94f, 0.29f, 0.29f),
        "blue" => new Color(0.22f, 0.54f, 0.86f),
        "yellow" => new Color(0.98f, 0.93f, 0.20f),
        "purple" => new Color(0.55f, 0.27f, 0.86f),
        _ => Color.white,
    };

    Material GetMatFromId(string id) => id switch
    {
        "red" => wireMat_Red,
        "blue" => wireMat_Blue,
        "yellow" => wireMat_Yellow,
        "purple" => wireMat_Purple,
        _ => wireMat_Red,
    };

    void StageClear()
    {
        isPlaying = false;
        int score = Mathf.RoundToInt(timeLeft);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        audioSource.Play();
        StartCoroutine(LoadClearScene());
    }

    IEnumerator LoadClearScene()
    {
        yield return new WaitForSeconds(1.5f);

        string clearKey = PlayerPrefs.GetString("CurrentClearKey", "");
        if (clearKey != "")
        {
            PlayerPrefs.SetInt(clearKey, 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("someya/ExperimentScene");
    }

    void GameOver()
    {
        isPlaying = false;
        PlayerPrefs.SetInt("TimeOverGameOver", 1);
        PlayerPrefs.Save();
        StartCoroutine(LoadExploreScene());
    }

    IEnumerator LoadExploreScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Scenes/ClearScreen");
    }
}