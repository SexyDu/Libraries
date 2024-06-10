using UnityEngine;
using UnityEngine.EventSystems;

namespace SexyDu.UI.Unity
{
    public class TouchTargetSender : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TouchTargetBasic target;

        public void OnPointerDown(PointerEventData eventData)
        {
            target.AddTouch(eventData.pointerId);
        }
    }
}