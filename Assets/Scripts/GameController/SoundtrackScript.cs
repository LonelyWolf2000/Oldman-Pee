using UnityEngine;
using UnityEngine.UI;

public class SoundtrackScript : MonoBehaviour
{
    private SettingSysScript _settingSys;
    private AudioSource _audioSource;

    // Use this for initialization
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _settingSys = FindObjectOfType<SettingSysScript>();

        if (_settingSys)
        {
            _settingSys.MusicSlider.ChangeValueSliderEvent += OnChangeValueSliderEvent;
            _settingSys.MusicToggle.ChangeValueToggleEvent += OnChangeValueToggleEvent;
        }
        
        DontDestroyOnLoad(transform.gameObject);
    }

    private void OnChangeValueToggleEvent(string nameToogle, Toggle toggle)
    {
        if (_audioSource && _settingSys && nameToogle == "MusicToggle")
            _audioSource.mute = !toggle.isOn;
    }

    private void OnChangeValueSliderEvent(string nameSlider, Slider slider)
    {
        if (_audioSource && _settingSys && nameSlider == "MusicSlider")
            _audioSource.volume = slider.value;
    }
}
