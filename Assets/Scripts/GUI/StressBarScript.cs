using UnityEngine;
using UnityEngine.UI;

public class StressBarScript : MonoBehaviour
{
    public Image Fill;
    private Player.PlayerController _player;

    // Use this for initialization
    void Start ()
    {
        _player = FindObjectOfType<Player.PlayerController>();
        Fill.fillAmount = 0;
    }
    
    // Update is called once per frame
    void Update ()
    {
        Debug.Log(_player.Stress);
        Fill.fillAmount = _player.Stress / 100;
    }
}
