using System.Collections;
using UnityEngine;

namespace Enemy.Cat
{
    public class MarkersScript : MonoBehaviour
    {
        public float DefaultDelayHideMarker = 0.8f;
        public Transform[] Markers;


        public string EnableRandomMarker()
        {
            return EnableRandomMarker(DefaultDelayHideMarker);
        }
        public string EnableRandomMarker(float customDelay)
        {
            if (Markers == null) return null;

            int index = Random.Range(0, Markers.Length);
            _EnableMarkerByIndex(index, customDelay);

            return Markers[index].name;
        }
        public void CryMarker_Enable()
        {
            CryMarker_Enable(DefaultDelayHideMarker);
        }
        public void CryMarker_Enable(float customDelay)
        {
            _EnableMarkerByIndex(0, customDelay);
        }

        private void _EnableMarkerByIndex(int index, float customDelay)
        {
            if (!_CheckValidAccess(index)) return;

            float delay = customDelay > 0 ? customDelay : DefaultDelayHideMarker;
            Markers[index].GetComponent<SpriteRenderer>().enabled = true;

            StartCoroutine(_HideMarker(Markers[index], delay));
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
