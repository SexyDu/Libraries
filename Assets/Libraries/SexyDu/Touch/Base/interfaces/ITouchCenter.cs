using UnityEngine;

namespace SexyDu.Touch
{
    public interface ITouchCenter : ITouchEventReceiver
    {
        // TouchConfig 인스턴스
        /// TouchConfig에 대한 접근은 여기서 수행한다.
        public static TouchConfig Config => TouchConfigSingleton.Ins;

        public Camera MainCam { get; }
    }
}