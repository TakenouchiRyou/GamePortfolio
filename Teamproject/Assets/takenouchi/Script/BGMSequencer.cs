using System.Collections;
using UnityEngine;

public class BGMSequencer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] bgmList;

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(PlayBGMSequence());
    }

    IEnumerator PlayBGMSequence()
    {
        while (currentIndex < bgmList.Length)
        {
            audioSource.clip = bgmList[currentIndex];
            audioSource.Play();

            // ‹È‚ªI‚í‚é‚Ü‚Å‘Ò‚Â
            yield return new WaitForSeconds(audioSource.clip.length);

            currentIndex++;
        }
    }
}