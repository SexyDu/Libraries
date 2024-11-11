namespace SexyDu.Touch
{
    /// <summary>
    /// [크기제한] 멀티터치 크기 변경 수행 행들
    /// </summary>
    public class TransformLimitedScaleHandle : TransformScaleHandle
    {
        private float minimum = 1f;
        private float maximum = 1f;

        public TransformLimitedScaleHandle(float minimum, float maximum) : base()
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        protected override void Set(float size)
        {
            if (size < minimum)
                size = minimum;
            else if (size > maximum)
                size = maximum;

            base.Set(size);
        }

        public TransformLimitedScaleHandle SetMinimum(float val)
        {
            minimum = val;

            return this;
        }

        public TransformLimitedScaleHandle SetMaximum(float val)
        {
            maximum = val;

            return this;
        }
    }
}