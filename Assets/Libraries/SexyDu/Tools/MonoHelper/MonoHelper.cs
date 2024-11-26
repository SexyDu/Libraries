using System;
using System.Collections;
using UnityEngine;

namespace SexyDu.Tool
{
    public static class MonoHelper
    {
        private static readonly Lazy<MonoBehindWorker> worker = new Lazy<MonoBehindWorker>(() => {
            // Worker 생성
            GameObject gameObject = new GameObject("MonoBehindWorker");
            
            return gameObject.AddComponent<MonoBehindWorker>().Initialize(gameObject);
        });
        private static MonoBehindWorker Worker => worker.Value;
        public static GameObject WorkerGameObject => Worker._gameObject;

        /// <summary>
        /// 워커를 통한 코루틴 실행 함수
        /// </summary>
        /// <param name="ie">코루틴 IEnumerator</param>
        /// <returns>생성된 헬퍼 코루틴 인터페이스</returns>
        public static CoroutineCommander StartCoroutine(IEnumerator ie)
        {
            try
            {
                return new CoroutineCommander(ie).Run(Worker);
            }
            catch (MissingReferenceException)
            {
                throw new MissingReferenceException("MonoBehindWorker가 사라졌는데 실행됐음..");
            }
        }

        private sealed class MonoBehindWorker : MonoBehaviour
        {
            public GameObject _gameObject { get; private set; }

            public MonoBehindWorker Initialize(GameObject obj)
            {
                _gameObject = obj;
                DontDestroyOnLoad(gameObject);

                return this;
            }
        }
    }
}