using UnityEngine;

namespace Enemy.Cat
{
    internal class CatTrigger : MonoBehaviour
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
                GetComponent<CircleCollider2D>().radius = _cat.RadiusAgro;
                _cat.FollowTarget(other.transform);
            }
            else if (other.tag == "Player" && _target != null)
            {
                _cat.Jump();
            }
        }
    }
}