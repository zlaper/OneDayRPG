using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using zg.gramrpg.assets;
using zg.gramrpg.data;

namespace zg.utils
{
    public class TapDetector : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerClickHandler, IPointerUpHandler
    {
        public Action<TapType> tapped;

        public float allowableMovement = 0.1f;

        private Vector2 _beginLocation;
        private bool _validLongTap;
        private Tween _longPressTimer;

        public void OnPointerDown(PointerEventData eventData)
        {
            // Stop long press in progress
            _longPressTimer?.Kill();

            // Start detection
            _beginLocation = eventData.position;
            _validLongTap = true;

            _longPressTimer = DOVirtual.DelayedCall(Values.LONG_PRESS_TIME, CheckForLongTap);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Check if we move too far
            if (Vector2.Distance(eventData.position, _beginLocation) > allowableMovement)
                CancelLongPressDetection();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // On tap cancel long press and send event
            CancelLongPressDetection();
            tapped?.Invoke(TapType.Tap);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CancelLongPressDetection();
        }

        private void CancelLongPressDetection()
        {
            // Invalidate
            _validLongTap = false;
            // Cancel long press timer
            _longPressTimer?.Kill();
        }

        private void CheckForLongTap()
        {
            // If we reached this point and the long tap is still valid, send the long tap event
            if (_validLongTap)
            {
                tapped?.Invoke(TapType.LongTap);
                _validLongTap = false;
            }
        }
    }
}
