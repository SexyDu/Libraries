#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SexyDu.Touch
{
    public /*abstract*/ partial class TouchCenter : MonoBehaviour
    {
        public static TouchConfig Config => TouchConfigSingleton.Ins;

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
            Config.AddTouchCenter(this);
        }

        private void OnDisable()
        {
            Config.RemoveTouchCenter(this);
        }

        /// <summary>
        /// 터치 입력 확인 및 터치 전달
        /// </summary>
        protected void SendTouchToTarget(UnityEngine.Touch touch)
        {
            // 캔버스 터치가 아닌 경우만 수행
            if (!IsCanvasTouch(touch))
                // 터치된 타겟이 있는 경우만 AddTouch
                GetTouchedTarget(mainCam, touch)?.ReceiveTouch(touch.fingerId);
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
            Component touchedComponent = Config.GetTouchedComponent2D(camera, position);

            if (touchedComponent == null)
                touchedComponent = Config.GetTouchedComponent3D(camera, position);

            if (touchedComponent != null)
                return touchedComponent.GetComponent<ITouchTarget>();
            else
                return null;
        }

        #region Coroutine
        private IEnumerator ieStandBy = null;

        public void Run()
        {
            try
            {
                if (gameObject.activeInHierarchy)
                {
                    Stop();

                    ieStandBy = CoStandBy();
                    StartCoroutine(ieStandBy);
                }
            }
            //catch(MissingReferenceException)
            //{
            //    Debug.LogFormat("오브젝트가 사라진 후에 코루틴 실행에 들어왔음");
            //}
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            if (ieStandBy != null)
            {
                StopCoroutine(ieStandBy);
                ieStandBy = null;
            }
        }

        private IEnumerator CoStandBy()
        {
            while (true)
            {
                yield return null;

                Process();
            }
        }

        /// <summary>
        /// 터치 입력 확인 함수
        /// </summary>
        private void Process()
        {
#if CONSIDER_MOUSE
            bool hasBeginTouch = false;
#endif
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase.Equals(TouchPhase.Began))
                {
                    SendTouchToTarget(Input.touches[i]);
#if CONSIDER_MOUSE
                    hasBeginTouch = true;
#endif
                }
            }

#if CONSIDER_MOUSE
            if (!hasBeginTouch)
                ProcessMouse();
#endif
        }

        /// <summary>
        /// 해당 터치가 캔버스 영역(UGUI)에 있는지 반환
        /// </summary>
        private bool IsCanvasTouch(UnityEngine.Touch touch)
        {
            return eventSystem is null ? false : eventSystem.IsPointerOverGameObject(touch.fingerId);
        }
        #endregion

#if CONSIDER_MOUSE
        private void ProcessMouse()
        {
            if (!IsCanvasMouse())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SendMouseToTarget(true);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    SendMouseToTarget(false);
                }
            }
        }

        private void SendMouseToTarget(bool left)
        {
            if (!IsCanvasMouse())
            {
                if (left)
                {
                    // 터치된 타겟이 있는 경우만 AddTouch
                    GetTouchedTarget(mainCam, Input.mousePosition)?.ReceiveTouch(TouchConfig.MouseIdLeft);
                }
                else
                {
                    // TODO: 추후 마우스 우클릭 인터페이스를 만들어서 고려시키자. (interface IRightClickTarget 같이)
                }
            }
        }

        private bool IsCanvasMouse()
        {
            return eventSystem is null ? false : eventSystem.IsPointerOverGameObject();
        }
#endif
    }
}