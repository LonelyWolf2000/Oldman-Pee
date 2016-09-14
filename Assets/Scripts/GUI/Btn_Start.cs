using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Start : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        GetComponentInParent<Canvas>().enabled = false;
        SceneManager.LoadScene("mainScene");
    }

}
