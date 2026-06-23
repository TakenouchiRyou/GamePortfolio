using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject fullMap;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isOpen = fullMap.activeSelf;
            fullMap.SetActive(!isOpen);
        }
    }
}