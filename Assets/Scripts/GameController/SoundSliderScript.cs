using UnityEngine;
using UnityEngine.UI;

public class SoundSliderScript : MonoBehaviour
{
    public delegate void ChangeValueSlider(string nameSlider, Slider slider);
    public event ChangeValueSlider ChangeValueSliderEvent;

    public string NameSlider;
    //public Toggle BindToggle;

    private Slider _slider;
    private float _fixedValue;

    // Use this for initialization
    void Start ()
    {
        _slider = GetComponent<Slider>();
        _FixValue(); 

        if(ChangeValueSliderEvent != null)
            ChangeValueSliderEvent.Invoke(NameSlider, _slider);
    }

    public void OnChangeValue()
    {
        if (ChangeValueSliderEvent != null &&_fixedValue != _slider.value)
        {
            _FixValue();
            ChangeValueSliderEvent.Invoke(NameSlider, _slider);
            //BindToggle.isOn = true;
        }
    }

    private void _FixValue()
    {
        if (_slider)
            _fixedValue = _slider.value;
    }
}
