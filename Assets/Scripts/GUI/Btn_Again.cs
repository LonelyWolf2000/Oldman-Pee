using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Again : MonoBehaviour
{
    public void OnAgainButtonClick()
    {
        GetComponentInParent<Canvas>().enabled = false;
        SceneManager.LoadScene("mainScene");
    }
}
