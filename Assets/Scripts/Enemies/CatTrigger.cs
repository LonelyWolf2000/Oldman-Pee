using UnityEngine;
using System.Collections;

public class CatTrigger : MonoBehaviour
{
    private Transform _target;
    private Cat _cat;

    private void Start()
    {
        _cat = GetComponentInParent<Cat>();
        GetComponent<CircleCollider2D>().radius = _cat.RadiusPool;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _target == null)
        {
            _target = other.transform;
            GetComponent<CircleCollider2D>().radius = _cat.RadiusArgo;
            _cat.FollowTarget(other.transform);
        }
        else if (other.tag == "Player" && _target != null)
        {
            _cat.Attack(_target);
        }
    }
}
