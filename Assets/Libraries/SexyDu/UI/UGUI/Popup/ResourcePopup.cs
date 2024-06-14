using System;
using UnityEngine;

namespace SexyDu.UI.UGUI
{
    public class ResourcePopup : MonoBehaviour
    {
        /// <summary>
        /// ResourcePopup 로드
        /// </summary>
        public static T Load<T>(string resourcePath, Transform parent = null) where T : ResourcePopup
        {
            T source = Resources.Load<T>(resourcePath);

            if (source != null)
                return Instantiate(source, parent);
            else
                throw new NullReferenceException($"전달받은 리소스 경로({resourcePath})에 ResourcePopup이 존재하지 않습니다");
        }
    }
}