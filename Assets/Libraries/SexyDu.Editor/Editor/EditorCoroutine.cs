using System;
using System.Collections;
using UnityEditor;

namespace SexyDu.OnEditor
{
    /// <summary>
    /// Editor용 Coroutine 수행자
    /// </summary>
    public class EditorCoroutine : IDisposable
    {
        public static EditorCoroutine StartCoroutine(IEnumerator _routine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Start();
            return coroutine;
        }

        public void Dispose()
        {
            Stop();
        }

        readonly IEnumerator routine;
        private EditorCoroutine(IEnumerator _routine) => routine = _routine;

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (mode == UnityEngine.SceneManagement.LoadSceneMode.Single)
                EditorApplication.update -= Update;
        }

        private void Start()
        {
            EditorApplication.update += Update;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void Stop()
        {
            EditorApplication.update -= Update;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            if (!routine.MoveNext()) Stop();
        }
    }
}

