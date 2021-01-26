namespace tracer
{
    public class StaticMove : MoveOnTrace
    {
        public float movingSpeed
        {
            set { mSpeed = value; }
            get { return mSpeed; }
        }
    }
}