using UnityEngine;

/// <summary>
/// ƒV[ƒ“‚²‚Æ‚ÌBGM‚ğÄ¶‚·‚é
/// </summary>
public class SceneBGM : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgmClip;

    private void Start()
    {
        if (AudioManager.Instance != null && bgmClip != null)
        {
            AudioManager.Instance.PlayBGM(bgmClip);
        }
    }
}