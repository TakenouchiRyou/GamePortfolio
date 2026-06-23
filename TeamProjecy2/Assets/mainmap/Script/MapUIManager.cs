using TMPro;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI hpText;

    void Update()
    {
        floorText.text =
            "Floor " + GameManager.instance.floor;

        hpText.text =
    GameManager.instance.playerCurrentHP
    + " / "
    + (100 + GameManager.instance.bonusMaxHP);
    }
}