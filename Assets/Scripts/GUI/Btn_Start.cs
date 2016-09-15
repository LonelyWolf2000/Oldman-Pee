using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Btn_Start : MonoBehaviour
{
    public Transform _RestartButton;

    void Awake()
    {
        if (_RestartButton)
            SceneManager.activeSceneChanged += OnChangeActiveScene;
    }

    private void OnChangeActiveScene(Scene arg0, Scene arg1)
    {
        if (arg1.name != "StartMenu")
        {
            _RestartButton.GetComponent<Button>().interactable = true;
            _RestartButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _RestartButton.GetComponent<Button>().interactable = false;
            _RestartButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void OnStartButtonClick()
    {
        //GetComponentInParent<Canvas>().enabled = false;
        SceneManager.LoadScene("MainScene");
    }
}
