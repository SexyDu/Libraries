using System;
using System.Collections.Generic;
using SexyDu.Tool;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시
    /// </summary>
    /// <typeparam name="T">캐시 타입</typeparam>
    public class SexyCache<T> : BinaryCache where T : class
    {
        #if UNITY_EDITOR
        public SexyCache()
        {
            Type type = typeof(T);
            if (type == typeof(Sprite))
                Debug.LogWarning($"Sprite 타입 캐시 사용 시 리소스 해제에 유의하세요.");
        }
        #endif

        public override void Dispose()
        {
            base.Dispose();

            callback = null;
        }

        /// <summary>
        /// 요청 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>다운로더 인터페이스</returns>
        public virtual SexyCache<T> Request(ICacheReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");
            else if (!IsSupported())
                throw new NotSupportedException($"지원하지 않는 타입입니다. : {typeof(T).Name}");

            worker = MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }

        // 텍스쳐 콜백
        private Action<IResponse<T>> callback = null;
        /// <summary>
        /// 콜백 등록
        /// </summary>
        /// <param name="callback">콜백</param>
        /// <returns>옵저거 서브젝트 인터페이스</returns>
        public SexyCache<T> Subscribe(Action<IResponse<T>> callback)
        {
            this.callback = callback;

            return this;
        }
        /// <summary>
        /// 옵저버에 노티 (IBytesResponse 수신 데이터를 기반으로 수신 데이터 재구성)
        /// </summary>
        /// <param name="res">bytes 수신 데이터</param>
        protected override void Notify(byte[] data, long code, string error, NetworkResult result)
        {
            if (callback != null)
            {
                callback.Invoke(new Response<T>(Convert(data), code, error, result));
            }
        }

        /// <summary>
        /// 바이트 배열을 타입에 맞게 변환
        /// </summary>
        private T Convert(byte[] data)
        {
            if (IsSupported())
                return converters[typeof(T)](data);
            else
                throw new NotSupportedException($"지원하지 않는 타입입니다. : {typeof(T).Name}");
        }

        /// <summary>
        /// 현재 캐시 타입이 지원 가능한 타입인지 확인
        /// </summary>
        private bool IsSupported()
        {
            return converters.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 지원 가능한 타입인지 확인
        /// </summary>
        public static bool IsSupported(Type type)
        {
            return converters.ContainsKey(type);
        }

        /// <summary>
        /// 타입별 바이트 배열 변환 딕셔너리
        /// 사용 가능 타입 정의
        /// </summary>
        private static readonly Dictionary<Type, Func<byte[], T>> converters = new Dictionary<Type, Func<byte[], T>>()
        {
            { typeof(Texture2D), (bytes) => ConvertFromBytes.ToTexture2D(bytes) as T },
            { typeof(Sprite), (bytes) => ConvertFromBytes.ToSprite(bytes) as T }, // [SpriteContent 사용 권장] CacheCloud에서는 리소스 해제 이슈(Sprite.texture 해제 문제)로 사용하지 않습니다.
            { typeof(SpriteContent), (bytes) => new SpriteContent(ConvertFromBytes.ToSprite(bytes)) as T },
        };
    }
}