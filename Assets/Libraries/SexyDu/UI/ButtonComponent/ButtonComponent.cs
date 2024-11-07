using System.Collections;
using UnityEngine;

namespace SexyDu.UI
{
    public partial class ButtonComponent : ButtonHandler
    {
        [Header("ButtonComponent")]
        [SerializeField] private Component colliderComponent; // entered 확인용 콜리더 컴포넌트

        private void Awake()
        {
            if (colliderComponent == null)
            {
                Collider2D collider2D = GetComponent<Collider2D>();

                if (collider2D != null)
                {
                    Debug.LogWarningFormat("'{0}'의 ButtonComponent에 colliderComponent가 연결되어 있지 않아 Awake(Run time)에서 연결해줌.\n터치 컴포넌트와의 비교를 위해 직접 달아주세요. Inspector에서 'SetColliderComponent' 버튼 눌러도 됨",
                    name);
                    colliderComponent = collider2D;
                }
                else
                {
                    Debug.LogErrorFormat("'{0}'의 ButtonComponent에 colliderComponent가 없어 연결을 시도하였으나 해당 오브젝트에 Collider2D가 없음.");
                }
            }

            InitializeEmployees();
        }

        /// <summary>
        /// 터치 입력 함수
        /// : ITouchTarget
        /// </summary>
        public override void ReceiveTouch(int fingerId)
        {
            if (!Touched)
            {
                this.fingerId = fingerId;
                TouchStart();

                SendEmployees(this.fingerId);
            }
        }

        // 터치 포인트가 해당 오브젝트에 들어와있는지 여부
        private bool entered = false;

        /// <summary>
        /// 터치 시작
        /// </summary>
        protected void TouchStart()
        {
            if (Touched)
            {
                InteractPress();

                entered = true;

                RunTouchRoutine();
            }
        }

        /// <summary>
        /// 터치 종료
        /// </summary>
        protected void TouchEnd()
        {
            if (Touched)
            {
                StopTouchRoutine();

                DisappearEmployees();

                if (entered)
                    OnClick();

                ClearTouch();
            }
        }

        // 터치 코루틴 변수
        private IEnumerator ieTouch = null;
        /// <summary>
        /// 터치 코루틴 함수
        /// </summary>
        private IEnumerator CoTouch()
        {
            while (Touched)
            {
                yield return null;

                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    CheckTouch();
                }
                else
                {
                    TouchEnd();
                    break;
                }
            }
        }

        /// <summary>
        /// 터치 체크
        /// </summary>
        private void CheckTouch()
        {
            Vector2 pos = GetTouchPosition(fingerId);

            // 터치 위치가 정상적으로 잡힌 경우
            if (pos.x > 0 || pos.y > 0)
            {
                Component component = Config.GetTouchedComponent2D(pos);

                entered = component is null ? false : component.Equals(colliderComponent);
            }
            // 아닌 경우
            else
            {
                TouchEnd();
            }
        }

        /// <summary>
        /// 터치 코루틴 실행
        /// </summary>
        private void RunTouchRoutine()
        {
            StopTouchRoutine();

            ieTouch = CoTouch();
            StartCoroutine(ieTouch);
        }

        /// <summary>
        /// 터치 코루틴 종료
        /// </summary>
        private void StopTouchRoutine()
        {
            if (ieTouch is not null)
            {
                StopCoroutine(ieTouch);
                ieTouch = null;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// [에디터 전용] colliderComponent 설정 함수
        /// </summary>
        public void SetColliderComponent()
        {
            Collider2D collider2D = GetComponent<Collider2D>();

            if (collider2D != null)
                colliderComponent = collider2D;
            else
                Debug.LogErrorFormat("'{0}'에 Collider2D가 설정되어 있지 않음.");
        }
#endif
    }
}