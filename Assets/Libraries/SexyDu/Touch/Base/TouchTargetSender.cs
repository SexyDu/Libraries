using UnityEngine;
using UnityEngine.EventSystems;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    public class TouchTargetSender : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TouchTarget target;

        public void OnPointerDown(PointerEventData eventData)
        {
            target.AddTouch(eventData.pointerId);
        }
    }
}