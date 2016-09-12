using UnityEngine;
using System.Collections;
using Player;

namespace Player
{
    public class StressTrigger : MonoBehaviour
    {
        private PlayerController _player;

        // Use this for initialization
        void Start()
        {
            _player = GetComponentInParent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemies")
            {
                _player.AddStress(other.tag);
            }
        }
    }
}
