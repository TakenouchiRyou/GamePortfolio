using UnityEngine;

public class MapBGMManager : MonoBehaviour
{
    [Header("BGM")]
    public AudioClip exploreBGM;  // 探索中のBGM
    public AudioClip chaseBGM;    // チェイス中のBGM

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayExploreBGM();
    }

    public void PlayExploreBGM()
    {
        if (audioSource.clip == exploreBGM) return;
        audioSource.clip = exploreBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayChaseBGM()
    {
        if (audioSource.clip == chaseBGM) return;
        audioSource.clip = chaseBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}