namespace SexyDu.Pattern.Behavioral.Observer
{
    // 옵저버 인터페이스
    public interface IObserverSome
    {
        public void OnChanged();
    }
    
    // 서브젝트 인터페이스
    public interface ISubjectSome
    {
        public void Subscribe(IObserverSome observer);
        public void Unsubscribe(IObserverSome observer);
    }
}