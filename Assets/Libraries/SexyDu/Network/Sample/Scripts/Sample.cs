using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace SexyDu.Network.Sample
{
    public class Sample : MonoBehaviour
    {
        private const string URL = "https://docs.google.com/spreadsheets/d/1k5_O590zRYRgkiiIqCjqD3rXEsz4ebSNEXeLJS01QkA/export?format=tsv";// "https://docs.google.com/spreadsheets/d/1k5_O590zRYRgkiiIqCjqD3rXEsz4ebSNEXeLJS01QkA/edit?usp=sharing";

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