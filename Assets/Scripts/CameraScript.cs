using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float offset = 6.5f;

    // Use this for initialization
    void Start ()
    {
        Player.PlayerController.MovePlayerEvent += OnMovePlayer;
        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }

    private void OnMovePlayer(GameObject sender)
    {
        if (sender.transform.position.x > LevelData.LeftLimiter.position.x + offset
            && sender.transform.position.x < LevelData.RightLimiter.position.x - offset)
            transform.parent = sender.transform;
        else
            transform.parent = null;
    }
}
