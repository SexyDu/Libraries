using UnityEngine;

namespace SexyDu.Pattern.Structural.Proxy
{
    // 기존에 사용하던 인터페이스 및 클래스가 있다.
    public interface IHuman
    {
        public void Speech(string word);
    }
    public class Human : IHuman
    {
        public void Speech(string word)
        {
            Debug.LogFormat("Human : {0}", word);
        }
    }

    // 기존 코드 수정 없이 추가적인 기능 및 예외처리를 행하는 모양으로 구성 가능합니다.
    public class HumanProxy : IHuman
    {
        private readonly Human real = null;

        public HumanProxy(Human real)
        {
            this.real = real;
        }

        public void Speech(string word)
        {
            if (!IsSleeping)
                this.real.Speech(word);
            else
                Debug.LogWarning("This human is sleeping...ZzZz");
        }

        #region Additional
        private bool isSleeping = false;
        public bool IsSleeping { get => isSleeping; }

        public void SetSleep(bool isSleeping)
        {
            this.isSleeping = isSleeping;
        }
        #endregion
    }
}