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
        _StressBar();
    }

    private void _StressBar()
    {
        Fill.fillAmount = (float)_player.Stress / 100;
    }
}
