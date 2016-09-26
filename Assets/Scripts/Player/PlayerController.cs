using System.Collections;
using System.Collections.Generic;
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
        public AudioSource[] AudioSources;
        //public CommonComponents CommonComponents;

        private MarkersScript _markers;
        private AudioSource _walkSound;
        private List<SoundSystemEventListener> _soundSystemEventListeners;
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

            if (AudioSources != null)
            {
                _soundSystemEventListeners = new List<SoundSystemEventListener>();

                _walkSound = AudioSources[0];
                _soundSystemEventListeners.Add(new SoundSystemEventListener(_walkSound, FindObjectOfType<SettingSysScript>()));
                _soundSystemEventListeners.Add(new SoundSystemEventListener(AudioSources[1], FindObjectOfType<SettingSysScript>()));
                StartCoroutine(_EnableWalkSound());
            }

            StartCoroutine("_StressTick");
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
                Debug.Log(_currentDirection);
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

            if (stress > 5)
            {
                _markers.WarningMarker_Show();
                AudioSources[1].Play();
            }

            if (Stress >= MaxStress && FullStressEvent != null)
            {
                InputController.Instance.MoveEvent -= OnMoveEvent;
                StopAllCoroutines();
                _walkSound.Pause();
                FullStressEvent.Invoke();
            }
        }

        private IEnumerator _StressTick()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                AddStress(LevelData.StressOfLevel);
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

            foreach (var _soundSystemEventListener in _soundSystemEventListeners)
                _soundSystemEventListener.DestroyListener();
        }
    }
}
