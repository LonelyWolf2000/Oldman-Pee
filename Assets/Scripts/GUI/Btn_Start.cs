using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Start : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("mainScene");
    }

}
