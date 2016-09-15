using System.Collections;
using UnityEngine;

namespace Enemy.Cat
{
    public class MarkersScript : MonoBehaviour
    {
        public float DefaultDelayHideMarker = 0.8f;
        public SpriteRenderer[] Markers;

        private SpriteRenderer _lastMarker;

        public string EnableRandomMarker()
        {
            if (Markers == null) return null;

            return EnableRandomMarker(0, Markers.Length, DefaultDelayHideMarker);
        }
        public string EnableRandomMarker(int startIndex, int endIndex, float customDelay)
        {
            int index = Random.Range(startIndex, endIndex + 1);
            if (Markers == null || !_CheckValidAccess(index)) return null;

            EnableMarkerByIndex(index, customDelay);
            return Markers[index].name;
        }
        public void CryMarker_Enable()
        {
            CryMarker_Enable(DefaultDelayHideMarker);
        }
        public void CryMarker_Enable(float customDelay)
        {
            EnableMarkerByIndex(0, customDelay);
        }
        public void AfraidMarker_Enable(float customDelay)
        {
            EnableMarkerByIndex(2, customDelay);
        }
        public void AfraidMarker_Enable()
        {
            AfraidMarker_Enable(DefaultDelayHideMarker);
        }
        public void EnableMarkerByIndex(int index, float customDelay)
        {
            if (!_CheckValidAccess(index)) return;

            _HideLastMarker();
            float delay = customDelay > 0 ? customDelay : DefaultDelayHideMarker;
            _lastMarker = Markers[index];
            _lastMarker.enabled = true;

            StartCoroutine(_HideMarker(_lastMarker, delay));
        }
        private void _HideLastMarker()
        {
            if (_lastMarker && _lastMarker.enabled)
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
