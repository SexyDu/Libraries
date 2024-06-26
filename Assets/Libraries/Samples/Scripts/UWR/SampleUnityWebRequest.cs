using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SexyDu.Sample
{
    public class SampleUnityWebRequest : MonoBehaviour
    {
        private const int MaxTryCount = 3;

        public void Get(string url, Action<ApiResponse> callback)
        {
            Uri uri = new Uri(url);
            StartCoroutine(CoGet(uri, callback));
        }

        public void Get(string url, string[] parameters, Action<ApiResponse> callback)
        {
            Get(CombineUrl(url, parameters), callback);
        }

        private IEnumerator CoGet(Uri uri, Action<ApiResponse> callback)
        {
            int tryCount = 0;

            do
            {
                ApiResponse res;

                using (UnityWebRequest req = UnityWebRequest.Get(uri))
                {
                    yield return req.SendWebRequest();

                    res = new ApiResponse(req.responseCode, req.downloadHandler.text,
                        req.error, req.result, req.GetResponseHeaders());
                }

                if (res.Result == UnityWebRequest.Result.ConnectionError)
                    tryCount++;
                else
                    callback?.Invoke(res);
            } while (tryCount < MaxTryCount);

            callback?.Invoke(ApiResponse.Failed);
        }

        public void Post(string url, string postData, Action<ApiResponse> callback)
        {
            Uri uri = new Uri(url);
            StartCoroutine(CoPost(uri, postData, RequestMethod.POST, callback));
        }

        public void Post(string url, string[] parameters, string postData, Action<ApiResponse> callback)
        {
            Post(CombineUrl(url, parameters), postData, callback);
        }

        public void Patch(string url, string postData, Action<ApiResponse> callback)
        {
            Uri uri = new Uri(url);
            StartCoroutine(CoPost(uri, postData, RequestMethod.PATCH, callback));
        }

        public void Patch(string url, string[] parameters, string postData, Action<ApiResponse> callback)
        {
            Patch(CombineUrl(url, parameters), postData, callback);
        }

        public void Delete(string url, string postData, Action<ApiResponse> callback)
        {
            Uri uri = new Uri(url);
            StartCoroutine(CoPost(uri, postData, RequestMethod.DELETE, callback));
        }

        public void Delete(string url, string[] parameters, string postData, Action<ApiResponse> callback)
        {
            Delete(CombineUrl(url, parameters), postData, callback);
        }

        private IEnumerator CoPost(Uri uri, string postData, RequestMethod method, Action<ApiResponse> callback)
        {
            int tryCount = 0;

            do
            {
                ApiResponse res;

                using (UnityWebRequest req = UnityWebRequest.Post(uri, postData))
                {
                    if (string.IsNullOrEmpty(postData))
                    {
                        byte[] postBin = new UTF8Encoding().GetBytes(postData);
                        req.uploadHandler = new UploadHandlerRaw(postBin);
                    }

                    req.method = method.ToString();

                    req.SetRequestHeader("Content-Type", "application/json");

                    yield return req.SendWebRequest();

                    res = new ApiResponse(req.responseCode, req.downloadHandler.text,
                        req.error, req.result, req.GetResponseHeaders());
                }

                if (res.Result == UnityWebRequest.Result.ConnectionError)
                    tryCount++;
                else
                    callback?.Invoke(res);
            } while (tryCount < MaxTryCount);

            callback?.Invoke(ApiResponse.Failed);
        }

        private string CombineUrl(string url, string[] parameters)
        {
            if (parameters == null || parameters.Length.Equals(0))
            {
                return url;
            }
            else
            {
                StringBuilder sb = new StringBuilder(url);
                if (parameters != null && parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        sb.AppendFormat("/{0}", parameters[i]);
                    }
                }

                return sb.ToString();
            }
        }
    }

    public enum RequestMethod : byte
    {
        GET = 0,
        POST,
        PATCH,
        DELETE
    }

    public struct ApiResponse
    {
        private readonly long code;
        private readonly string text;
        private readonly string error;
        private readonly UnityWebRequest.Result result;
        private readonly Dictionary<string, string> headers;

        /// External Access
        public long Code => code;
        public string Text => text;
        public string Error => error;
        public UnityWebRequest.Result Result => result;
        public Dictionary<string, string> Headers => headers;

        public ApiResponse(long code, string text, string error,
            UnityWebRequest.Result result, Dictionary<string, string> headers)
        {
            this.code = code;
            this.text = text;
            this.error = error;
            this.result = result;
            this.headers = headers;
        }

        public bool IsEmpty => code.Equals(default(long));

        public readonly static ApiResponse Failed
            = new ApiResponse(long.MinValue, string.Empty, string.Empty,
                UnityWebRequest.Result.ConnectionError, null);
    }
}