using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour
{
    public float MoveSpeed;
    public float HissTime;
    private Transform _target;

    void Start()
    {
        gameObject.name = "cat";
    }

    public void RunAway()
    {
        MoveSpeed += 2;
        transform.rotation = new Quaternion(0, 180, 0, 0);
        StartCoroutine(_Follow(LevelData.RightLimiter, Vector3.right));
    }

    public void FollowTarget(Transform target)
    {
        StartCoroutine(_Follow(target, Vector3.left));
    }

    private void _Destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }

    private IEnumerator _Follow(Transform point, Vector3 direction)
    {
        while (Mathf.Abs(transform.position.x - point.position.x) > 0.8f)
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
        yield return new WaitForSeconds(HissTime);
        Debug.Log("Hissssss");
        RunAway();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Player" && _target == null)
    //    {
    //        _target = other.transform;
    //        StartCoroutine(_Follow(_target, Vector3.left));
    //    }
    //}
}
