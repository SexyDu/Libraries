using System;
using UnityEngine;
using TMPro;

namespace SexyDu.UI.UGUI
{
    /// <summary>
    /// 팝업 InputField
    /// </summary>
    public class PopupInputField : ResourcePopup
    {
        // Input field
        [SerializeField] private TMP_InputField inputField;
        // 설정된 워드
        private string text { get => inputField.text; }

        #region setter
        /// <summary>
        /// 초기 설정
        /// </summary>
        /// <returns></returns>
        public PopupInputField Initialize()
        {
            inputField.onSubmit.AddListener(text => {
                Submit(text);
            });

            return this;
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
        public PopupInputField Initialize(string text)
        {
            inputField.text = text;

            return Initialize();
        }

        /// <summary>
        /// InputField 선택
        /// </summary>
        public PopupInputField SelectInputField()
        {
            inputField.Select();

            return this;
        }

        /// <summary>
        /// 워드 결정 콜백 이벤트
        /// </summary>
        private Action<string> onDecided = null;
        public PopupInputField CallbackOnDecided(Action<string> onDecided)
        {
            this.onDecided = onDecided;

            return this;
        }

        /// <summary>
        /// 팝업 종료 이벤트
        /// </summary>
        private Action onClosed = null;
        public PopupInputField CallbackOnClosed(Action onClosed)
        {
            this.onClosed = onClosed;

            return this;
        }
        #endregion

        #region feature
        /// <summary>
        /// 워드 결정
        /// </summary>
        private void Decide(string text)
        {
            onDecided?.Invoke(text);
        }

        /// <summary>
        /// 입력 제출
        /// </summary>
        private void Submit(string text)
        {
            Decide(text);

            Close();
        }

        /// <summary>
        /// 빈 워드 결정
        /// </summary>
        private void DecideClearable()
        {
            Decide(string.Empty);
        }

        /// <summary>
        /// 팝업 종료
        /// </summary>
        public void Close()
        {
            onClosed?.Invoke();

            Destroy(gameObject);
            Destroy(this);
        }
        #endregion

        #region event on click
        /// <summary>
        /// 결정 버튼 클릭
        /// </summary>
        public void OnClickDecide()
        {
            Submit(text);
        }

        /// <summary>
        /// 클리어 버튼 클릭
        /// </summary>
        public void OnClickDecideClearable()
        {
            DecideClearable();

            Close();
        }

        /// <summary>
        /// 취소 버튼 클릭
        /// </summary>
        public void OnClickClose()
        {
            Close();
        }
        #endregion
    }
}