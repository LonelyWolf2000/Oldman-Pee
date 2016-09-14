using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.GameController;
using GameController.Commands;
using GameController;
using Random = UnityEngine.Random;

namespace Enemy.Cat
{
    public class Cat : MonoBehaviour
    {
        public float MoveSpeed = 3.0f;
        public int StressValue;
        public float HissTime = 0.5f;
        public float ThinkTime = 1.0f;
        public float RadiusPool = 6.0f;
        public float RadiusAgro = 1.5f;
        public float DistanceFollowing = 2.0f;
        public float JumpForce = 5.0f;
        public bool IsAgro { get; private set; }
        private bool _vulnerability;
        private string _currentMarker = "none";
        //public CommonComponents CommonComponents;

        private SoundSystemEventListener _soundSystemEventListener;
        private AudioSource _crySound;
        private Transform _target;
        private MarkersScript _markers;
        private bool _isHissingRun;

        void Start()
        {
            gameObject.name = "cat";
            InputController.Instance.CryEvent += OnCryEvent;
            InputController.Instance.BlockEvent += OnBlockEvent;
            _markers = GetComponent<MarkersScript>();
            _crySound = GetComponent<AudioSource>();
            _soundSystemEventListener = new SoundSystemEventListener(_crySound, FindObjectOfType<SettingSysScript>());
        }


        public void FollowTarget(Transform target)
        {
            gameObject.layer = 8;   // Перемещаем кошку на 8й слой (Cats), чтоб физика столкновения кошек не просчитывалась
            IsAgro = true;
            _target = target;
            StartCoroutine(_MoveToTarget());
        }

        public void Jump()
        {
            StopAllCoroutines();

            GetComponent<CircleCollider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().gravityScale = 3;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, JumpForce);
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            _RunAway();
        }

        private void _Startle()
        {
            //Jump();
            StopAllCoroutines();
            _PlaySound();
            _currentMarker = "none";
            MoveSpeed += 2;
            transform.rotation = new Quaternion(0, 180, 0, 0);
            StartCoroutine(_MoveToPoint(new Vector3(transform.position.x + 6.5f, transform.position.y, transform.position.z)));
        }

        private void _Block()
        {
            if(_currentMarker == "AtackMarker")
                _currentMarker = "none";
        }
        private void _RunAway()
        {
            MoveSpeed += 2;
            transform.rotation = new Quaternion(0, 180, 0, 0);
            DistanceFollowing = 0.5f;
            IsAgro = false;
            _target = LevelData.RightLimiter;
            StartCoroutine(_MoveToTarget());
        }

        private void _PlaySound()
        {
            if (_crySound != null)
                _crySound.Play();
        }

        private void _Destroy()
        {
            InputController.Instance.CryEvent -= OnCryEvent;
            InputController.Instance.BlockEvent -= OnBlockEvent;
            _soundSystemEventListener.DestroyListener();
            StopAllCoroutines();
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }

        private IEnumerator _MoveToTarget()
        {
            while (true)
            {
                if (_DistanceToTarget() < DistanceFollowing + 0.1f && _DistanceToTarget() > DistanceFollowing - 0.1f)
                    break;

                Vector3 direction = transform.position.x - _target.transform.position.x > DistanceFollowing ? Vector3.left : Vector3.right;
                transform.Translate(direction * Time.deltaTime * MoveSpeed, Space.World);

                yield return new WaitForEndOfFrame();
            }

            if (IsAgro)
                StartCoroutine(_SaveDistance());
        }

        private IEnumerator _MoveToPoint(Vector3 point)
        {
            while (true)
            {
                if (Math.Abs(transform.position.x - point.x) < 0.1f)
                    break;

                transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed, Space.World);

                yield return new WaitForEndOfFrame();
            }

            MoveSpeed -= 2;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            _isHissingRun = false;

            yield return new WaitForSeconds(2.0f);

            StartCoroutine(_MoveToTarget());
        }

        private IEnumerator _SaveDistance()
        {
            while (true)
            {
                if(!_isHissingRun)
                    StartCoroutine(_Hissing());

                yield return null;

                if (Mathf.Abs(_DistanceToTarget() - DistanceFollowing) > 0)
                {
                    yield return new WaitForSeconds(ThinkTime);
                    StartCoroutine(_MoveToTarget());
                    break;
                }
            }
        }

        private IEnumerator _Hissing()
        {
            _isHissingRun = true;
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

            float delay = Random.Range(0.5f, 2.0f);
            _currentMarker = _markers.EnableRandomMarker(delay);

            yield return new WaitForSeconds(delay);

            if (_currentMarker == "AtackMarker")
            {
                _target.GetComponent<Player.PlayerController>().AddStress(StressValue);
                _PlaySound();
            }

            _currentMarker = "none";
            _isHissingRun = false;
        }

        private float _DistanceToTarget()
        {
            return Mathf.Abs(transform.position.x - _target.position.x);
        }

        private void OnCryEvent(float axis)
        {
            if (axis != 0 && _target != null && _currentMarker == "CryMarker")
            {
               CommandManager.RegisterCommand(new Cry(_Startle));
            }
        }

        private void OnBlockEvent(float axis)
        {
            if (axis != 0 && _target != null && _currentMarker == "AtackMarker")
            {
                CommandManager.RegisterCommand(new Block(_Block));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_target != null && other.name == "EndDoor")
                _Destroy();

            //if (_target != null && (other.name == "spider" || other.name == "cat"))
            //    Jump();
        }
    }
}