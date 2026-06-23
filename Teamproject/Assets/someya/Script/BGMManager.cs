using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        // •Û‘¶‚³‚ê‚½‰¹—Ê‚ðMixer‚É”½‰f
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        float se = PlayerPrefs.GetFloat("SEVolume", 0.8f);

        audioMixer.SetFloat("BGMVolume", Mathf.Log10(bgm) * 20);
        audioMixer.SetFloat("SEVolume", Mathf.Log10(se) * 20);
    }
}