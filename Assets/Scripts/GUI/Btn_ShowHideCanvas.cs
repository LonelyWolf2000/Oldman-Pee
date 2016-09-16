using UnityEngine;

public class Btn_ShowHideCanvas : MonoBehaviour
{
    public Canvas ShowCanvas;
    public Canvas[] HideCanvaces;
    public void OnButtonClick()
    {
        if (ShowCanvas)
            ShowCanvas.enabled = !ShowCanvas.enabled;

        if (HideCanvaces != null && HideCanvaces.Length > 0)
        {
            foreach (var canvace in HideCanvaces)
            {
                canvace.enabled = false;
            }
        }
    }
}
