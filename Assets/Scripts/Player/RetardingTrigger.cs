using UnityEngine;
using Enemy.Cat;

namespace Player
{
    public class RetardingTrigger : MonoBehaviour
    {
        private PlayerController _player;
        private int _catsInTrigger;
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
                if(_catsInTrigger == 0)
                    _player.MoveSpeed = _player.MoveSpeed -_player.BrakingMoment;

                _catsInTrigger++;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.name == "cat")
            {
                _catsInTrigger--;

                if(_catsInTrigger == 0)
                    _player.MoveSpeed = _player.MoveSpeed +_player.BrakingMoment;
            }
        }
    }
}
