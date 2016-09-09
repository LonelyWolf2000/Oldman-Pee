using System.Collections;
using UnityEngine;

namespace Player
{
    public class MarkersScript : MonoBehaviour
    {
        public float DelayHideMarker = 0.8f;
        public Transform[] Markers;

        public void GoAwayMarker_Show()
        {
            if (!_CheckValidAccess(0)) return;

            Markers[0].GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(_HideMarker(Markers[0], DelayHideMarker));
        }

        public void WarningMarker_Show()
        {
            if (!_CheckValidAccess(1)) return;

            Markers[1].GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(_HideMarker(Markers[1], DelayHideMarker));
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