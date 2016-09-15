using System.Collections;
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
        public int MaxStress = 100;
        public float MoveSpeed;
        public float BrakingMoment;
        public GameObject[] Indicators;
        //public CommonComponents CommonComponents;

        private MarkersScript _markers;
        private AudioSource _walkSound;
        private SoundSystemEventListener _soundSystemEventListener;
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
            InputController.Instance.BlockEvent += OnBlockEvent;

            _leftOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 - 0.5f;
            _rightOffset = GetComponent<SpriteRenderer>().sprite.pivot.x / 100 + 0.4f;

            _walkSound = GetComponent<AudioSource>();
            _soundSystemEventListener = new SoundSystemEventListener(_walkSound, FindObjectOfType<SettingSysScript>());
            StartCoroutine(_EnableWalkSound());
        }

        private void OnCryEvent(float axis)
        {
            CommandManager.RegisterCommand(new Cry(_GoAwayCry));
        }
        private void OnBlockEvent(float axis)
        {
            CommandManager.RegisterCommand(new Cry(_Block));
        }

        private void OnMoveEvent(float direction)
        {
            //if (direction == 0) return;

            _currentDirection = direction;
            if(_currentDirection != 0)
                CommandManager.RegisterCommand(new Move(_Move));
        }
        private void _Move()
        {
            Vector3 newPosition = new Vector3(transform.position.x + _speed * _currentDirection, transform.position.y, transform.position.z);
            _speed = MoveSpeed > 0 ? MoveSpeed / _DIVIDER : 0.1f;

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
        private void _Block()
        {
            _markers.BlockMarker_Show();
        }
        public void AddStress(int stress)
        {
            Stress += stress;
            _markers.WarningMarker_Show();

            if (Stress >= MaxStress && FullStressEvent != null)
            {
                InputController.Instance.MoveEvent -= OnMoveEvent;
                FullStressEvent.Invoke();
            }
        }

        private IEnumerator _EnableWalkSound()
        {
            if(_walkSound == null) yield break;

            
            _walkSound.Play();
            _walkSound.Pause();

            while (true)
            {
                if (_currentDirection != 0) _walkSound.UnPause();
                else _walkSound.Pause();

                yield return new WaitForSeconds(0.1f);
            }
        }

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if(other.name == "EndDoor")
        //        InputController.Instance.MoveEvent -= OnMoveEvent;
        //}
        private void OnDestroy()
        {
            InputController.Instance.CryEvent -= OnCryEvent;
            InputController.Instance.BlockEvent -= OnBlockEvent;
            InputController.Instance.MoveEvent -= OnMoveEvent;
            _soundSystemEventListener.DestroyListener();
        }
    }
}
