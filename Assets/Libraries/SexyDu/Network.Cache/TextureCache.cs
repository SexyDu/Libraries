using System;
using SexyDu.Tool;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 텍스쳐 캐시
    /// </summary>
    public class TextureCache : BinaryCache//, ITextureDownloader
    {
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
        public virtual TextureCache Request(ICacheReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");

            worker = MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }

        // 텍스쳐 콜백
        private Action<IResponse<Texture2D>> callback = null;
        /// <summary>
        /// 콜백 등록
        /// </summary>
        /// <param name="callback">콜백</param>
        /// <returns>옵저거 서브젝트 인터페이스</returns>
        public TextureCache Subscribe(Action<IResponse<Texture2D>> callback)
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
                Texture2D tex2D = ConvertFromBytes.ToTexture2D(data);
                callback.Invoke(new Response<Texture2D>(tex2D, code, error, result));
            }
        }
    }
}