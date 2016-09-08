using UnityEngine;
using GameController;
using GameController.Commands;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void MovePlayer(GameObject sender);
        public static event MovePlayer MovePlayerEvent;
        public delegate void FullStress();
        public static event FullStress FullStressEvent;

        public static PlayerController Player { get; private set; }
        public int Stress { get; private set; }
        public float MoveSpeed;

        private const int _DIVIDER = 100;
        private float _speed;
        private float _leftOffset;
        private float _rightOffset;
        private float _currentDirection;

        private void Start()
        {
            if (Player == null)
                Player = this;

            InputController.Instance.MoveEvent += OnMoveEvent;

            _leftOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 - 0.5f;
            _rightOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 + 0.4f;

        }

        private ICommand OnMoveEvent(float direction)
        {
            if (direction == 0) return null;
            _currentDirection = direction;

            return new Move(_Move);
        }

        private void _Move()
        {
            Vector3 newPosition = new Vector3(transform.position.x + _speed * _currentDirection, transform.position.y, transform.position.z);
            _speed = MoveSpeed > 0 ? MoveSpeed / _DIVIDER : 0.1f;
            _currentDirection = 0;

            if (newPosition.x > LevelData.LeftLimiter.position.x + _leftOffset
                && newPosition.x < LevelData.RightLimiter.position.x - _rightOffset)
            {
                transform.position = newPosition;
                if (MovePlayerEvent != null) MovePlayerEvent.Invoke(gameObject);
            }
        }
        private void _AddStress(string sourceOfStress)
        {
            Stress += sourceOfStress == "Enemies" ? 10 : 2;

            if (Stress > 10 && FullStressEvent != null)
            {
                InputController.Instance.MoveEvent -= OnMoveEvent;
                FullStressEvent.Invoke();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemies")
            {
                _AddStress(other.tag);
            }
        }
    }
}
