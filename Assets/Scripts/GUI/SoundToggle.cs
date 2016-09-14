using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public delegate void ChangeValueToogle(string nameToogle, Toggle toggle);
    public event ChangeValueToogle ChangeValueToggleEvent;

    public string Name;
    //public Slider BindSlider;
    public Image CheckmarkOff;
    //private float _savedValue;

    public void OnCheckToggle()
    {
        CheckmarkOff.enabled = !CheckmarkOff.enabled;
        
        if(ChangeValueToggleEvent != null)
            ChangeValueToggleEvent.Invoke(Name, GetComponent<Toggle>());

        //if (BindSlider)
        //{
        //    BindSlider.value = GetComponent<Toggle>().isOn ? _savedValue : 0;
        //    _savedValue = BindSlider.value;
        //}
    }
}
