using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string sceneName;
    public GameObject questionMark;
    public string clearKey; // 例：「Clear_1」「Clear_2」など
    public GameObject enemy; // InspectorでEnemyをセット

    private bool playerNear = false;

    void Start()
    {
        questionMark.SetActive(false);

        int clearValue = PlayerPrefs.GetInt(clearKey, 0);
        Debug.Log(clearKey + "の値: " + clearValue);

        if (clearValue == 1)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            questionMark.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            questionMark.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.F))
        {
            // Enemy出現中は配線修理に入れない
            if (enemy.activeSelf)
            {
                Debug.Log("Enemyが出現中は入れない！");
                return;
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
                PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
            }
            PlayerPrefs.SetString("CurrentClearKey", clearKey);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneName);
        }
    }
}