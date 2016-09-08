using UnityEngine;
using System.Collections;
using GameController.Commands;
using GameController;

namespace Enemy.Cat
{
    public class Cat : MonoBehaviour
    {
        public float MoveSpeed = 3.0f;
        public float HissTime = 0.5f;
        public float ThinkTime = 1.0f;
        public float RadiusPool = 6.0f;
        public float RadiusAgro = 1.5f;
        public float DistanceFollowing = 2.0f;
        public float JumpForce = 5.0f;

        private Transform _target;

        void Start()
        {
            gameObject.name = "cat";
            InputController.Instance.CryEvent += OnCryEvent;
        }


        public void FollowTarget(Transform target)
        {
            gameObject.layer = 8;   // Перемещаем кошку на 8й слой (Cats), чтоб физика столкновения кошек не просчитывалась
            _target = target;
            StartCoroutine(_Follow(_target, Vector3.left));
        }

        public void Jump()
        {
            StopAllCoroutines();

            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().gravityScale = 3;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, JumpForce);

            _RunAway();
        }
        private void _RunAway()
        {
            MoveSpeed += 2;
            transform.rotation = new Quaternion(0, 180, 0, 0);
            DistanceFollowing = 0.5f;
            StartCoroutine(_Follow(LevelData.RightLimiter, Vector3.right));
        }

        private void _CryReaction()
        {
            Jump();
        }

        private void _Destroy()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            InputController.Instance.CryEvent -= OnCryEvent;
            Destroy(gameObject);
        }

        private IEnumerator _Follow(Transform point, Vector3 direction)
        {
            while (Mathf.Abs(transform.position.x - point.position.x) > DistanceFollowing)
            {
                transform.Translate(direction * Time.deltaTime * MoveSpeed, Space.World);
                yield return new WaitForEndOfFrame();
            }

            if (direction == Vector3.left)
                StartCoroutine(_Hissing());
            else
                _Destroy();
        }

        private IEnumerator _Hissing()
        {
            while (true)
            {
                yield return new WaitForSeconds(HissTime);
                Debug.Log("Hissssss");
                if (Mathf.Abs(transform.position.x - _target.position.x) > DistanceFollowing)
                {
                    yield return new WaitForSeconds(ThinkTime);
                    FollowTarget(_target);
                    break;
                }
            }
        }

        private ICommand OnCryEvent(float axis)
        {
            if (axis != 0 && _target != null && Mathf.Abs(transform.position.x - _target.position.x) < DistanceFollowing)
                return new Cry(_CryReaction);

            return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_target != null && other.name == "spider")
            {
                Jump();
            }
        }
    }
}