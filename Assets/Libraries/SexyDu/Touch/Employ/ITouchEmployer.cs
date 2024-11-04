namespace SexyDu.Touch
{
    public interface ITouchEmployer
    {
        /// <summary>
        /// employee의 동작에 따른 보고를 받는 함수
        /// </summary>
        public void ReceiveReport();

        /// <summary>
        /// 해당 fingerId의 터치가 employer에서 유효한지 반환
        /// </summary>
        /// <param name="fingerId"></param>
        /// <returns></returns>
        public bool ValidTouch(int fingerId);
    }
}