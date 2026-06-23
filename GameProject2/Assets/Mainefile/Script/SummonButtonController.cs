using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonButtonController : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;   // Œø‰Ê‰¹‚ğ–Â‚ç‚·AudioSource
    public AudioClip clickSound;      // Ä¶‚·‚éŒø‰Ê‰¹
    Text text;
    int cost;
    Directo630 costManager;
    Button button;

    void Start()
    {
        var p = player.GetComponent<SummonCost>();
        cost = p.summonCost;

        Text txt = GetComponentInChildren<Text>();
        txt.text = cost.ToString();

        costManager = GameObject.FindObjectOfType<Directo630>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSummonButtonClick);
    }

    void Update()
    {
        //if (costManager.cost >= cost) {
        button.interactable = (costManager.cost >= cost);
    }
    void OnSummonButtonClick()
    {
        // Œø‰Ê‰¹‚ğ–Â‚ç‚·
        audioSource.PlayOneShot(clickSound);

        // ‚±‚±‚É¢Š«ˆ—‚È‚Ç‚ğ’Ç‰Á‰Â”\
        Debug.Log("Summon Button Clicked!");
    }

}
