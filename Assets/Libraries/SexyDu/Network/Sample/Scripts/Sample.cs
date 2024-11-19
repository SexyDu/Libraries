using UnityEngine;

namespace SexyDu.Network.Sample
{
    public class Sample : MonoBehaviour
    {
        // 원본 공유 Url : https://drive.google.com/file/d/12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA/view?usp=sharing
        // 실제 URL
        /// URL을 바꾼 이유는 원본 공유 URL은 해당 파일의 구글 드라이브 미리보기 화면에 대한 URL이라 제대로 데이터가 받아지지 않는다.
        /// 하여 'https://drive.google.com/uc?export=download&id={FILE_ID}' 형식으로 수정 되어야한다.
        private const string URL = "https://drive.google.com/uc?export=download&id=12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA";

        public long code;
        [TextArea] public string text;
        public string error;
        public string result;
        [TextArea]
        public string headers;

        public void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Run"))
            {
                new SexyRESTWorker().
                Request(new UnityRESTReceipt(RESTMethod.GET).SetUri(URL)).
                Subscribe((res) =>
                {
                    code = res.code;

                    Debug.Log($"res.text : {res.text}");
                    if (res.text.Length > 100)
                        text = res.text.Substring(0, 100);
                    else
                        text = res.text;

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
                });
            }
        }
    }
}