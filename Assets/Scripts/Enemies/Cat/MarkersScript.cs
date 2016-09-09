using System.Collections;
using UnityEngine;

namespace Enemy.Cat
{
    public class MarkersScript : MonoBehaviour
    {
        public float DefaultDelayHideMarker = 0.8f;
        public Transform[] Markers;

        public void CryMarker_Enable()
        {
            CryMarker_Enable(DefaultDelayHideMarker);
        }
        public void CryMarker_Enable(float customDelay)
        {
            if (!_CheckValidAccess(0)) return;

            float delay = customDelay > 0 ? customDelay : DefaultDelayHideMarker;
            Markers[0].GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(_HideMarker(Markers[0], delay));
        }

        private IEnumerator _HideMarker(Transform spriteRenderer, float delay)
        {
            yield return new WaitForSeconds(delay);
            spriteRenderer.GetComponent<SpriteRenderer>().enabled = false;
        }

        private bool _CheckValidAccess(int index)
        {
            return Markers != null && index > -1 && index < Markers.Length;
        }
    }
}
