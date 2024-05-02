namespace SexyDu.PageViewSystem
{
    /// <summary>
    /// PageContent가 제거될 때 제거를 요청하는 인터페이스
    /// * PageContent가 그룹(리스트) 형식으로 관리되는 경우 그룹상 제거를 위해
    /// </summary>
    public interface IPageContentRemoveReceiver
    {
        public void Remove(PageContent content);
    }

    /// <summary>
    /// [Observer] PageConent 활성화 수 변경
    /// </summary>
    public interface IObserverContentsCount
    {
        public void OnContentsCountChanged(int count);
    }

    /// <summary>
    /// [Subject] PageContent 활성화 수 변경
    /// </summary>
    public interface ISubjectContentsCount
    {
        public int Count { get; }
        public void Subscribe(IObserverContentsCount observer);
        public void Unsubscribe(IObserverContentsCount observer);
    }
}