using UnityEngine;

public class Btn_Settings : MonoBehaviour
{
    public Canvas SettingsWin;
    public void OnSettingsButtonClick()
    {
        if (SettingsWin)
            SettingsWin.enabled = !SettingsWin.enabled;
    }
}
