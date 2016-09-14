using UnityEngine;
using System.Collections;
using Assets.Scripts.GameController;

namespace Enemy.Spider
{
    public class Spider : MonoBehaviour
    {
        public float MoveSpeed;
        public float Cooldown;
        public int StressValue;
        public AudioClip[] AudioClips;
        private AudioSource _spiderSound;
        //public CommonComponents CommonComponents;

        private SoundSystemEventListener _soundSystemEventListener;
        public bool IsCooldown { get; private set; }

        // Use this for initialization
        void Start()
        {
            gameObject.name = "spider";
            transform.position = new Vector3(transform.position.x, LevelData.HightLevel, transform.position.z);
            _spiderSound = GetComponent<AudioSource>();
            _soundSystemEventListener = new SoundSystemEventListener(_spiderSound, FindObjectOfType<SettingSysScript>());
            StartCoroutine(_RandomMove());
        }

        public void GetDown()
        {
            if(IsCooldown) return;

            IsCooldown = true;
            StartCoroutine(_VerticalMoving());
        }

        private IEnumerator _RandomMove()
        {
            while (true)
            {
                yield return null;

                if (!IsCooldown)
                {
                    yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
                    GetDown();
                }
            }
        }

        private IEnumerator _VerticalMoving()
        {
            float t = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
            _PlaySpiderSound(0);

            bool exit = false;
            while (true)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, t * MoveSpeed);

                if (transform.position == endPos)
                {
                    if (exit)
                    {
                        StartCoroutine(_CooldownTimer());
                        break;
                    }

                    t = 0;
                    endPos = startPos;
                    startPos = transform.position;
                    exit = true;
                    _PlaySpiderSound(1);
                }

                yield return null;
            }

            if(_spiderSound)
                _spiderSound.Stop();
        }

        private IEnumerator _CooldownTimer()
        {
            yield return new WaitForSeconds(Cooldown);
            IsCooldown = false;
        }

        private void _PlaySpiderSound(int index)
        {
            if(_spiderSound == null || AudioClips == null || AudioClips.Length == 0)
                return;

            _spiderSound.clip = AudioClips[index];
            _spiderSound.Play();
        }

        private void OnDestroy()
        {
            _soundSystemEventListener.DestroyListener();
        }

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (other.name == "cat" && other.GetComponent<Cat.Cat>().IsAgro)
        //    {
        //        GetComponent<SpriteRenderer>().enabled = false;
        //        Destroy(gameObject);
        //    }
        //}
    }
}