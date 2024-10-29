using SexyDu.Touch;
using UnityEngine;

namespace SexyDu.UI
{
    public partial class ButtonComponent : ITouchEmployer
    {
        [SerializeField] private TouchEmployee[] employees;

        private void InitializeEmployees()
        {
            for (int i = 0; i < employees.Length; i++)
            {
                employees[i].SetEmpoloyer(this);
            }
        }

        public void ReceiveReport()
        {

        }
    }
}