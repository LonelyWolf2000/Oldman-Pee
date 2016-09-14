using UnityEngine;
using System.Collections;

public class SettingSysScript : MonoBehaviour
{
    public SoundSliderScript MusicSlider { get; private set; }
    public SoundSliderScript SoundSlider { get; private set; }
    public SoundToggle MusicToggle { get; private set; }
    public SoundToggle SoundToggle { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        foreach (var item in FindObjectsOfType<SoundSliderScript>())
        {
            if (item.transform.parent.name == "Music")
                MusicSlider = item;
            if (item.transform.parent.name == "Sound")
                SoundSlider = item;
        }

        foreach (var item in FindObjectsOfType<SoundToggle>())
        {
            if (item.transform.parent.name == "Music")
                MusicToggle = item;
            if (item.transform.parent.name == "Sound")
                SoundToggle = item;
        }

    }
}
