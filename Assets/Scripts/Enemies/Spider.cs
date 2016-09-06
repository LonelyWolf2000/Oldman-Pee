using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        transform.position = new Vector3(transform.position.x, LevelData.HightLevel, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "cat")
            other.GetComponent<Cat>().RunAway();
    }
}
