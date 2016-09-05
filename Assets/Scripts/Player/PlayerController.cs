using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void MovePlayer(GameObject sender);
        public static event MovePlayer MovePlayerEvent;

        public static PlayerController Player { get; private set; }
        public int Stress { get; private set; }
        public float MoveSpeed;

        private const int _DIVIDER = 10;
        private float _speed;
        private float _leftOffset;
        private float _rightOffset;



        private void Start()
        {
            if (Player == null)
                Player = this;

            _speed = MoveSpeed > 0 ? MoveSpeed / _DIVIDER : 0.1f;
            _leftOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 + 0.1f;
            _rightOffset = 1 - GetComponent<SpriteRenderer>().sprite.pivot.x / 100;
        }

        private void Update()
        {
            Move(Input.GetAxis("Moving"));
        }

        public void Move(float direction)
        {
            Vector3 newPosition = new Vector3(transform.position.x + _speed * direction, transform.position.y, transform.position.z);

            if (newPosition.x > LevelData.LeftLimiter.position.x + _leftOffset
                && newPosition.x < LevelData.RightLimiter.position.x - _rightOffset)
            {
                transform.position = newPosition;
                if (MovePlayerEvent != null) MovePlayerEvent.Invoke(gameObject);
            }
            
        }
    }
}
