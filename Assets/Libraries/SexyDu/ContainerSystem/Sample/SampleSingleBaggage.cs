using SexyDu.ContainerSystem;

namespace SexyDu.Sample
{
    public class SampleSingleBaggage : ISingleBaggage
    {
        private int a;
        private string b;

        public int A => a;
        public string B => b;

        public void SetA(int a)
        {
            this.a = a;
        }

        public void SetB(string b)
        {
            this.b = b;
        }
    }
}