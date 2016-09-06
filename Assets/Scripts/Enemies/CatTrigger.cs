using UnityEngine;
using System.Collections;

public class CatTrigger : MonoBehaviour
{
    private Transform _target;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _target == null)
        {
            _target = other.transform;
            GetComponentInParent<Cat>().FollowTarget(other.transform);
        }
    }
}
