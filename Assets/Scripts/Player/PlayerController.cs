using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Player { get; private set; }
        public int Stress { get; private set; }
        public float MoveSpeed;

        private const int _DIVIDER = 10;
        private float _speed;


        private void Awake()
        {
            if (Player == null)
                Player = this;

            _speed = MoveSpeed > 0 ? MoveSpeed / _DIVIDER : 0.1f;
        }

        private void Update()
        {
            Move(Input.GetAxis("Moving"));
        }

        public void Move(float direction)
        {
            if(direction != 0)
                transform.position = new Vector3(transform.position.x + _speed * direction, transform.position.y, transform.position.z);
        }
    }
}
