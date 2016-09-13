using UnityEngine;
using System.Collections;
using Player;

namespace Player
{
    public class StressTrigger : MonoBehaviour
    {
        public PlayerController Player { get; private set; }

        // Use this for initialization
        void Start()
        {
            Player = GetComponentInParent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "spider")
            {
                Player.AddStress(10);
            }
        }
    }
}
