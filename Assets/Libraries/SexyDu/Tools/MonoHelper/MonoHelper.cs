using System;
using System.Collections;
using UnityEngine;

namespace SexyDu.Tool
{
    public static class MonoHelper
    {
        private static readonly Lazy<MonoBehindWorker> worker = new Lazy<MonoBehindWorker>(Set());
        private static MonoBehindWorker Worker { get { return worker.Value; } }

        /// <summary>
        /// 워커 초기 생성 함수
        /// </summary>
        /// <returns>생성된 워커</returns>
        private static MonoBehindWorker Set()
        {
            GameObject gameObject = new GameObject("MonoBehindWorker");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            return gameObject.AddComponent<MonoBehindWorker>();
        }

        /// <summary>
        /// 워커를 통한 코루틴 실행 함수
        /// </summary>
        /// <param name="ie">코루틴 IEnumerator</param>
        /// <returns>생성된 헬퍼 코루틴 인터페이스</returns>
        public static IDisposable StartCoroutine(IEnumerator ie)
        {
            try
            {
                return new HelperCoroutine(Worker.StartCoroutine(ie));
            }
            catch(MissingReferenceException)
            {
                throw new MissingReferenceException("MonoBehindWorker가 사라졌는데 실행됐음..");
            }
        }

        /// <summary>
        /// 워커를 통해 수행되는 코르틴을 종료하는 함수
        /// </summary>
        /// <param name="co">종료할 코루틴</param>
        public static void StopCoroutine(Coroutine co)
        {
            try
            {
                Worker.StopCoroutine(co);
            }
            catch (MissingReferenceException)
            {
                Debug.Log("[앱 종료 시 정상] MonoBehindWorker가 사라졌는데 이를 통해 코루틴을 종료하여 시도함.");
            }
        }

        private sealed class MonoBehindWorker : MonoBehaviour
        {
        }
    }
}