using System.Collections.Generic;
using UnityEngine;
using SexyDu.Network.Cache;

namespace SexyDu.Network.Sample
{
    public class Sample : MonoBehaviour
    {
        // 원본 공유 Url : https://drive.google.com/file/d/12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA/view?usp=sharing
        // 실제 URL
        /// URL을 바꾼 이유는 원본 공유 URL은 해당 파일의 구글 드라이브 미리보기 화면에 대한 URL이라 제대로 데이터가 받아지지 않는다.
        /// 하여 'https://drive.google.com/uc?export=download&id={FILE_ID}' 형식으로 수정 되어야한다.
        private const string googleDriveUrl = "https://drive.google.com/uc?export=download&id=12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA";

        private const string imageUrl = "https://pm.pstatic.net/resources/asset/sp_main.b27083b1.png";

        private void OnDestroy()
        {
            Clear();
        }

        public long code;
        public string error;
        public string result;
        [TextArea]
        public string headers;

        [Header("문자열 데이터")]
        [TextArea] public string text;
        [Header("텍스쳐 데이터")]
        public Texture texture;
        public KeyValuePair<string, string> pair;

        private void Set(IResponse res)
        {
            code = res.code;

            error = res.error;

            result = res.result.ToString();


            if (res.headers != null && res.headers.Count > 0)
            {
                foreach (var header in res.headers)
                {
                    headers = string.Format("{0}\n{1} : {2}", headers, header.Key, header.Value);
                }
            }
            else
                headers = string.Empty;
        }

        private void Clear()
        {
            code = 0;

            error = string.Empty;

            result = string.Empty;

            headers = string.Empty;

            text = string.Empty;

            if (texture != null)
            {
                Destroy(texture);
                texture = null;
            }
        }

        public void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Run"))
            {
                Clear();

                new SexyRESTWorker().
                Request(new RESTReceipt(NetworkMethod.GET).SetUri(googleDriveUrl)).
                Subscribe((res) =>
                {
                    Debug.Log($"res.text : {res.data}");
                    if (res.data.Length > 100)
                        text = res.data.Substring(0, 100);
                    else
                        text = res.data;

                    Set(res);
                });
            }

            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Texture"))
            {
                Clear();

                new SexyTextureDownloader().
                Request(new BinaryReceipt().SetUri(imageUrl)).
                Subscribe((res) =>
                {
                    texture = res.data;

                    Set(res);
                });
            }
        }
    }
}