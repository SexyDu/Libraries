using UnityEngine;
using UnityEngine.EventSystems;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    public class TouchTargetSender : MonoBehaviour, ITouchTargetSender, IPointerDownHandler
    {
        [SerializeField] private TouchTarget target;

        public void SetTouchReceiver(ITouchTarget receiver)
        {
            if (receiver is TouchTarget)
                target = receiver as TouchTarget;
            else
                Debug.LogError("TouchTargetSender에서는 TouchTarget만을 Receiver로 받을 수 있습니다.");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            target.AddTouch(eventData.pointerId);
        }
    }
}