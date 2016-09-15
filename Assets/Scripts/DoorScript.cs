using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public delegate void PlayerInDoor(GameObject door);
    public static event PlayerInDoor PlayerInDoorEvent;

    //public CommonComponents CommonComponents;

    private SoundSystemEventListener _soundSystemEventListener;
    private AudioSource _creakSound;

    private void Start()
    {
        if (gameObject.name == "EndDoor")
            transform.rotation = new Quaternion(0, 180, 0, 0);

        _creakSound = GetComponent<AudioSource>();
        _soundSystemEventListener = new SoundSystemEventListener(_creakSound, FindObjectOfType<SettingSysScript>());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.name == "EndDoor" && other.tag == "Player")
        {
            if(PlayerInDoorEvent != null)
                PlayerInDoorEvent.Invoke(gameObject);
        }
    }
    private void OnDestroy()
    {
        _soundSystemEventListener.DestroyListener();
    }
}
