using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Image CheckmarkOff;

    public void OnCheckToggle()
    {
        CheckmarkOff.enabled = !CheckmarkOff.enabled;
    }
}
