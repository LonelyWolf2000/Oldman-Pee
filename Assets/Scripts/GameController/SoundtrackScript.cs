using UnityEngine;
using System.Collections;

public class SoundtrackScript : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
