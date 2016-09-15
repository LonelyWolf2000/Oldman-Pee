using UnityEngine;
using UnityEngine.UI;


class SoundSystemEventListener
{
    private SettingSysScript _settingSystem;
    private AudioSource _audioSource;

    public SoundSystemEventListener(AudioSource audioSource, SettingSysScript settingSysScript)
    {
        _audioSource = audioSource;
        _settingSystem = settingSysScript;

        _audioSource.volume = _settingSystem.SoundSlider.GetComponent<Slider>().value;
        _audioSource.mute = !_settingSystem.SoundToggle.GetComponent<Toggle>().isOn;

        _settingSystem.SoundSlider.ChangeValueSliderEvent += OnChangeValueSliderEvent;
        _settingSystem.SoundToggle.ChangeValueToggleEvent += OnChangeValueToggleEvent;
    }

    public void DestroyListener()
    {
        _settingSystem.SoundSlider.ChangeValueSliderEvent -= OnChangeValueSliderEvent;
        _settingSystem.SoundToggle.ChangeValueToggleEvent -= OnChangeValueToggleEvent;
    }

    private void OnChangeValueSliderEvent(string nameSlider, Slider slider)
    {
        if (_audioSource && _settingSystem && nameSlider == "SoundSlider")
            _audioSource.volume = slider.value;
    }

    private void OnChangeValueToggleEvent(string nameToggle, Toggle toggle)
    {
        if (_audioSource && _settingSystem && nameToggle == "SoundToggle")
            _audioSource.mute = !toggle.isOn;
    }
}
