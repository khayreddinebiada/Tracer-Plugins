namespace path
{
    public class StaticMove : MoveOnTrace
    {
        public float speed
        {
            set { movingSpeed = value; }
            get { return movingSpeed; }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_target == null)
                _target = transform;
        }
#endif
    }
}