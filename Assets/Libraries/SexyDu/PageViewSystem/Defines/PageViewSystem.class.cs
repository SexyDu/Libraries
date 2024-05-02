namespace SexyDu.PageViewSystem
{
    public class InitializeArgumentBase
    {
        private readonly string from = string.Empty;

        public string From { get { return from; } }

        public InitializeArgumentBase(string from)
        {
            this.from = from;
        }
    }
}