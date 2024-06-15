#define CHANGE_PARSE_WAY

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SexyDu.Tool
{
    /// <summary>
    /// JsonParsing Tool
    /// </summary>
    public struct JsonParser
    {
#if CHANGE_PARSE_WAY
        private static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            DateParseHandling = DateParseHandling.None
        };
#endif

        /// <summary>
        /// 초기 JObject 형식 사용 생성자
        /// </summary>
        public JsonParser(string json)
        {
            try
            {
#if CHANGE_PARSE_WAY
                root = JsonConvert.DeserializeObject<JToken>(json, JsonSerializerSettings);
#else
                root = JToken.Parse(json);
#endif
            }
            catch (Exception e)
            {
                throw new JsonException(
                    string.Format("json deserialize (to JToken) 도중 에러가 발생하였습니다.\n- json : {0}", json),
                    e);
            }

            selected = root;
        }

        /// <summary>
        /// 초기 JArray 형식 사용 생성자
        /// </summary>
        public JsonParser(string json, int index)
        {
            JArray array = null;
            try
            {
#if CHANGE_PARSE_WAY
                array = JsonConvert.DeserializeObject<JArray>(json, JsonSerializerSettings);
#else
                JArray array = JArray.Parse(json);
#endif
            }
            catch (Exception e)
            {
                throw new JsonException(
                    string.Format("json deserialize (to JArray) 도중 에러가 발생하였습니다.\n- json : {0}", json),
                    e);
            }

            if (array == null)
            {
                throw new NullReferenceException(
                    string.Format("JArray가 없습니다.\n- json : {0}", json));
            }
            else if (array.Count <= index)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("array[{0}]", index),
                    string.Format("인덱스({0})가 JArray의 범위를 벗어났습니다.\n- json : {1}", index, json));
            }
            else
                selected = root = array[index];
        }

        
        private JToken root; // 루트 토큰
        private JToken selected; // 선택 토큰
        
        /// <summary>
        /// 선택 토큰 초기화
        /// </summary>
        public void Reset()
        {
            selected = root;
        }

        /// <summary>
        /// 선택 토큰 설정
        /// </summary>
        public JsonParser Set(int index)
        {
            if (selected is JArray)
            {
                JArray array = selected as JArray;
                if (index < array.Count)
                {
                    this.selected = array[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format("selected[{0}]", index),
                        string.Format("인덱스({0})가 JArray의 범위를 벗어났습니다.\n- selected : {1}\n- root : {2}",
                        index, ToStringFromSelected(), ToStringFromRoot()));
                }
            }
            else
            {
                throw new NotImplementedException(
                    string.Format("현재 선택된 토큰은 JArray 형식이 아닙니다.\n- selected : {0}\n- root : {1}",
                    ToStringFromSelected(), ToStringFromRoot()));
            }

            return this;
        }

        /// <summary>
        /// 선택 토큰 설정
        /// </summary>
        public JsonParser Set(string tokenKey)
        {
            JToken token = selected.SelectToken(tokenKey);
            if (token != null)
            {
                this.selected = token;
            }
            else
            {
                throw new ArgumentNullException(
                    string.Format("TokenKey : {0}", tokenKey),
                    string.Format("키({0})에 맞는 토큰을 찾을 수 없습니다.\n- selected : {1}\n- root : {2}",
                    tokenKey, ToStringFromSelected(), ToStringFromRoot()));
            }

            return this;
        }

        /// <summary>
        /// 선택 토큰 Object화 함수
        /// </summary>
        public T Extract<T>()
        {
            if (selected != null)
                return this.selected.ToObject<T>();
            else
            {
                UnityEngine.Debug.LogErrorFormat("selected가 없습니다.\n- root : {0}", ToStringFromRoot());
                return default(T);
            }
        }

        /// <summary>
        /// 루트 토큰 string화 함수
        /// </summary>
        private string ToStringFromRoot()
        {
            if (root == null)
                return "[ null ]";
            else
                return root.ToString();
        }

        /// <summary>
        /// 선택 토큰 string화 함수
        /// </summary>
        private string ToStringFromSelected()
        {
            if (selected == null)
                return "[ null ]";
            else
                return selected.ToString();
        }
    }
}