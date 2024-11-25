using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using SexyDu.Animation;
using SexyDu.Tools;

namespace SexyDu.FileIO
{
    public class TestFileIO : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private void Awake()
        {
            SexyMainThreadDispatcher.Instance.Run();
        }

        [SerializeField] private string path = string.Empty;
        FileReaderAcync fileReader = new FileReaderAcync();

        private float startTime;

        private void ReadFileBytesContinueWith()
        {
            Debug.Log("Run ContinueWith");

            startTime = Time.time;
            fileReader.ReadAllBytesAsync(path).ContinueWith(task =>
            {
                byte[] bytes = task.Result;
                SexyMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    Debug.LogFormat("ContinueWith : {0} bytes, {1}ms", bytes.Length, Time.time - startTime);
                });
            });
        }

        private void ReadFileBytesCoroutine()
        {
            Debug.Log("Run Coroutine");

            StartCoroutine(CoRead());
        }

        private IEnumerator CoRead()
        {
            yield return null;
            startTime = Time.time;
            var task = fileReader.ReadAllBytesAsync(path);
            yield return new WaitUntil(() => task.IsCompleted);
            byte[] bytes = task.Result;
            Debug.LogFormat("Coroutine : {0} bytes, {1}ms", bytes.Length, Time.time - startTime);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "ContinueWith"))
            {
                ReadFileBytesContinueWith();
            }
            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Coroutine"))
            {
                ReadFileBytesCoroutine();
            }
            if (GUI.Button(new Rect(200f, 0f, 100f, 100f), "MainThreadDispatcher"))
            {
                SexyMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    Debug.Log("MainThreadDispatcher");
                });
            }
        }
    }

    public class FileReaderAcync
    {
        public async Task<byte[]> ReadAllBytesAsync(string path)
        {
            return await File.ReadAllBytesAsync(path);
        }
    }
}
