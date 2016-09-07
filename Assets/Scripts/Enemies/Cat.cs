using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Cat : MonoBehaviour
{
    public float MoveSpeed = 3.0f;
    public float HissTime = 0.5f;
    public float ThinkTime = 1.0f;
    public float RadiusPool = 6.0f;
    public float RadiusArgo = 1.5f;
    public float DistanceFollowing = 2.0f;

    private Transform _target;

    void Start()
    {
        gameObject.name = "cat";
    }

    public void RunAway()
    {
        MoveSpeed += 2;
        transform.rotation = new Quaternion(0, 180, 0, 0);
        DistanceFollowing = 0;
        StartCoroutine(_Follow(LevelData.RightLimiter, Vector3.right));
    }

    public void FollowTarget(Transform target)
    {
        _target = target;
        StartCoroutine(_Follow(_target, Vector3.left));
    }

    public void Attack(Transform target)
    {
        StopAllCoroutines();
        Debug.Log("Attack!!!!!!!!!");
        RunAway();
    }

    private void _Destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_target != null && other.name == "spider")
        {
            _target = other.transform;
            Attack(_target);
        }
    }
}
