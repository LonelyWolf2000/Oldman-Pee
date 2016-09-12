using UnityEngine;
using Enemy.Cat;

namespace Player
{
    public class RetardingTrigger : MonoBehaviour
    {
        private PlayerController _player;
        // Use this for initialization
        void Start()
        {
            Cat cat = FindObjectOfType<Cat>();
            if (cat != null)
                GetComponent<CircleCollider2D>().radius = cat.DistanceFollowing;
            _player = GetComponentInParent<PlayerController>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "cat")
            {
                _player.MoveSpeed = _player.MoveSpeed -_player.BrakingMoment;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.name == "cat")
            {
                _player.MoveSpeed = _player.MoveSpeed +_player.BrakingMoment;
            }
        }
    }
}
