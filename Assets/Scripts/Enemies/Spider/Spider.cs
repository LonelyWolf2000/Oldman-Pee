using UnityEngine;
using System.Collections;

namespace Enemy.Spider
{
    public class Spider : MonoBehaviour
    {
        public float MoveSpeed;
        public float Cooldown;

        public bool IsCooldown { get; private set; }

        // Use this for initialization
        void Start()
        {
            gameObject.name = "spider";
            transform.position = new Vector3(transform.position.x, LevelData.HightLevel, transform.position.z);
        }

        public void GetDown()
        {
            if(IsCooldown) return;

            IsCooldown = true;
            StartCoroutine(_VerticalMoving());
        }

        private IEnumerator _VerticalMoving()
        {
            float t = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(transform.position.x, 0.0f, transform.position.z);

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
                }

                yield return null;
            }
        }

        private IEnumerator _CooldownTimer()
        {
            yield return new WaitForSeconds(Cooldown);
            IsCooldown = false;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "cat" && other.GetComponent<Cat.Cat>().IsAgro)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject);
            }
        }
    }
}