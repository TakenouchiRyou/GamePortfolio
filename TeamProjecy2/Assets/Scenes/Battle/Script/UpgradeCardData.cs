using UnityEngine;

[CreateAssetMenu(
menuName =
"Upgrade Card")]
public class UpgradeCardData :
ScriptableObject
{
    public UpgradeType
        upgradeType;


    public string
        cardName;


    public Sprite
        cardImage;


    public int
        upgradeAmount;
}