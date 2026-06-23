using System.Collections;
using UnityEngine;

public class SimpleSEPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] sounds;

    void Start()
    {
        StartCoroutine(PlaySE());
    }

    IEnumerator PlaySE()
    {
        foreach (AudioClip clip in sounds)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}