using UnityEngine;

namespace Enemy.Spider
{
    public class SpiderTriggerActivator : MonoBehaviour
    {
        private Spider _parent;
        // Use this for initialization
        void Start()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(boxCollider.size.x, transform.position.y);
            boxCollider.offset = new Vector2(0, transform.position.y / 2);
            boxCollider.isTrigger = true;

            _parent = GetComponentInParent<Spider>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("ppp");
            if (!_parent.IsCooldown && (other.tag == "Player" || other.tag == "Enemies"))
            {
                _parent.GetDown();
            }
        }
    }
}