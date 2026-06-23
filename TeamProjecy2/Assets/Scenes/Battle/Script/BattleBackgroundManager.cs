using UnityEngine;

public class BattleBackgroundManager : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    public Sprite forestBackground;
    public Sprite caveBackground;
    public Sprite volcanoBackground;
    public Sprite endlessBackground;

    public AudioClip forestBGM;
    public AudioClip caveBGM;
    public AudioClip volcanoBGM;
    public AudioClip endlessBGM;

    void Start()
    {
        int floor = GameManager.instance.floor;

        if (floor <= 5)
        {
            backgroundRenderer.sprite = forestBackground;
            AudioManager.Instance.PlayBGM(forestBGM);
        }
        else if (floor <= 10)
        {
            backgroundRenderer.sprite = caveBackground;
            AudioManager.Instance.PlayBGM(caveBGM);
        }
        else if (floor <= 15)
        {
            backgroundRenderer.sprite = volcanoBackground;
            AudioManager.Instance.PlayBGM(volcanoBGM);
        }
        else
        {
            backgroundRenderer.sprite = endlessBackground;
            AudioManager.Instance.PlayBGM(endlessBGM);
        }
    }
}