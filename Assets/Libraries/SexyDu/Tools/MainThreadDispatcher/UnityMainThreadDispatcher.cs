/// SexyMainTreadDispatcher를 사용하기 위해 비활성화
///  * 추후 이걸 사용하려면 활성화 하고 SexyMainThreadDispatcher 비활성화
#if false
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SexyDu.Tool
{
    /// <summary>
    /// 유니티 Update기반 Main Thread Dispatcher
    /// * 오픈소스를 기반의 방식으로 작성
    ///  Url : https://github.com/PimDeWitte/UnityMainThreadDispatcher/blob/master/Runtime/UnityMainThreadDispatcher.cs
    /// </summary>
    public class UnityMainThreadDispatcher : MonoBehaviour, IMainThreadDispatcher
    {
        #region Singleton
        private static UnityMainThreadDispatcher _instance = null;

        public static bool Exists()
        {
            return _instance != null;
        }

        public static UnityMainThreadDispatcher Instance => _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnDestroy()
        {
            Debug.LogFormat("UnityMainThreadDispatcher가 파괴되었습니다. 이러면 안됩니다. 원인을 파악하십쇼 휴우먼.");
            _instance = null;
        }
        #endregion

        #region Proxy IMainThreadDispatcher
            // 메인 스레드 디스패처 수행
        private readonly MainThreadDispatcher dispatcher = new MainThreadDispatcher();

        private void Update()
        {
            dispatcher.Execute();
        }

        public void Enqueue(Action action)
        {
            dispatcher.Enqueue(action);
        }

        public Task EnqueueAsync(Action action)
        {
            return dispatcher.EnqueueAsync(action);
        }
        #endregion
    }
}
#endif
