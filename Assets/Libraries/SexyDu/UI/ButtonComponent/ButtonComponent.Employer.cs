using SexyDu.Touch;
using UnityEngine;

namespace SexyDu.UI
{
    public partial class ButtonComponent : ITouchEmployer
    {
        [Header("Employee")]
        [SerializeField] private TouchEmployee[] employees;

        private void InitializeEmployees()
        {
            for (int i = 0; i < employees.Length; i++)
            {
                employees[i].SetEmpoloyer(this);
            }
        }

        private void SendEmployees(int fingerId)
        {
            for (int i = 0; i < employees.Length; i++)
            {
                employees[i].Detect(fingerId);
            }
        }

        private void DisappearEmployees()
        {
            for (int i = 0; i < employees.Length; i++)
            {
                employees[i].Disappear();
            }
        }

        public void ReceiveReport()
        {
            StopTouchRoutine();

            entered = false;

            ClearTouch();

            for (int i = 0; i < employees.Length; i++)
            {
                employees[i].Cancel();
            }
        }

        public bool ValidTouch(int fingerId)
        {
            Vector2 pos = GetTouchPosition(fingerId);

            // 터치 위치가 정상적으로 잡힌 경우
            if (pos.x > 0 || pos.y > 0)
            {
                Component component = Config.GetTouchedComponent2D(pos);

                return component is null ? false : component.Equals(colliderComponent);
            }
            else
                return false;
        }

#if UNITY_EDITOR
        public void SetEmployees()
        {
            this.employees = GetComponents<TouchEmployee>();
        }
#endif
    }
}