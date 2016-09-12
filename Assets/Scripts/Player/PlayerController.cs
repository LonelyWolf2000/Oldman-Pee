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
        public float BrakingMoment;
        public GameObject[] Indicators;

        private MarkersScript _markers;
        private const int _DIVIDER = 100;
        private float _speed;
        private float _leftOffset;
        private float _rightOffset;
        private float _currentDirection;

        private void Start()
        {
            gameObject.name = "Oldman";
            _markers = GetComponent<MarkersScript>();

            if (Player == null)
                Player = this;

            InputController.Instance.MoveEvent += OnMoveEvent;
            InputController.Instance.CryEvent += OnCryEvent;

            _leftOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 - 0.5f;
            _rightOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 + 0.4f;
        }

        private void OnCryEvent(float axis)
        {
            CommandManager.RegisterCommand(new Cry(_GoAwayCry));
        }


        private void OnMoveEvent(float direction)
        {
            if (direction == 0) return;

            _currentDirection = direction;
            CommandManager.RegisterCommand(new Move(_Move));
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
        private void _GoAwayCry()
        {
            _markers.GoAwayMarker_Show();
        }
        public void AddStress(string sourceOfStress)
        {
            Stress += sourceOfStress == "Enemies" ? 10 : 2;
            _markers.WarningMarker_Show();

            if (Stress > 20 && FullStressEvent != null)
            {
                InputController.Instance.MoveEvent -= OnMoveEvent;
                FullStressEvent.Invoke();
            }
        }
        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (other.tag == "Enemies" && gameObject.name == "Oldman")
        //    {
        //        _AddStress(other.tag);
        //    }
        //}

        private void OnDestroy()
        {
            InputController.Instance.CryEvent -= OnCryEvent;
        }
    }
}
