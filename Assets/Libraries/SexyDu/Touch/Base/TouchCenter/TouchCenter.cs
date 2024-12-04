#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using UnityEngine;
using UnityEngine.EventSystems;

namespace SexyDu.Touch
{
    public /*abstract*/ partial class TouchCenter : MonoBehaviour, ITouchCenter
    {

        private Camera mainCam = null;
        public Camera MainCam => mainCam;
        public float orthographicSize => mainCam.orthographicSize;

        protected EventSystem eventSystem => EventSystem.current;

        protected virtual void Awake()
        {
            mainCam = GetComponent<Camera>();

            if (mainCam == null)
            {
                Debug.LogErrorFormat("TouchCenter가 활성화 되었지만 해당 오브젝트에 카메라가 없습니다.\n- 오브젝트 이름 : {0}", name);

                Destroy(this);
            }
        }

        private void OnEnable()
        {
            ITouchCenter.Config.AddTouchCenter(this);
        }

        private void OnDisable()
        {
            ITouchCenter.Config.RemoveTouchCenter(this);
        }

        /// <summary>
        /// 터치 충돌 타겟 반환
        /// </summary>
        protected /*abstract*/ ITouchTarget GetTouchedTarget(Camera camera, UnityEngine.Touch touch)
        {
            return GetTouchedTarget(camera, touch.position);
        }

        /// <summary>
        /// 터치 충돌 타겟 반환
        /// </summary>
        protected /*abstract*/ ITouchTarget GetTouchedTarget(Camera camera, Vector2 position)
        {
            Component touchedComponent = ITouchCenter.Config.GetTouchedComponent2D(camera, position);

            if (touchedComponent == null)
                touchedComponent = ITouchCenter.Config.GetTouchedComponent3D(camera, position);

            if (touchedComponent != null)
                return touchedComponent.GetComponent<ITouchTarget>();
            else
                return null;
        }

        /// <summary>
        /// 터치 입력 수신 이벤트 함수
        /// </summary>
        public void OnTouchBegin(UnityEngine.Touch touch)
        {
            GetTouchedTarget(mainCam, touch)?.ReceiveTouch(touch.fingerId);
        }
        /// <summary>
        /// 마우스 입력 수신 이벤트 함수
        /// </summary>
        public void OnMouseBegin(int mouseId, Vector2 position)
        {
            // 터치된 타겟이 있는 경우만 AddTouch
            GetTouchedTarget(mainCam, position)?.ReceiveTouch(mouseId);
        }
    }
}