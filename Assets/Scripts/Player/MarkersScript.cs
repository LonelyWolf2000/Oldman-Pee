using System.Collections;
using UnityEngine;

namespace Player
{
    public class MarkersScript : MonoBehaviour
    {
        public float DelayHideMarker = 0.8f;
        public SpriteRenderer[] Markers;

        private SpriteRenderer _lastMarker;

        public void GoAwayMarker_Show()
        {
            if (!_CheckValidAccess(0)) return;

            _HideLastMarker();
            Markers[0].enabled = true;
            _lastMarker = Markers[0];
            StartCoroutine(_HideMarker(_lastMarker, DelayHideMarker));
        }

        public void BlockMarker_Show()
        {
            if (!_CheckValidAccess(2)) return;

            _HideLastMarker();
            Markers[2].enabled = true;
            _lastMarker = Markers[2];
            StartCoroutine(_HideMarker(_lastMarker, DelayHideMarker));
        }

        public void WarningMarker_Show()
        {
            if (!_CheckValidAccess(1)) return;

            _HideLastMarker();
            Markers[1].enabled = true;
            _lastMarker = Markers[1];
            StartCoroutine(_HideMarker(_lastMarker, DelayHideMarker));
        }

        private void _HideLastMarker()
        {
            if(_lastMarker &&_lastMarker.enabled)
                _lastMarker.enabled = false;
        }

        private IEnumerator _HideMarker(SpriteRenderer marker, float delay)
        {
            yield return new WaitForSeconds(delay);
            marker.enabled = false;
        }

        private bool _CheckValidAccess(int index)
        {
            return Markers != null && index > -1 && index < Markers.Length;
        }
    }
}