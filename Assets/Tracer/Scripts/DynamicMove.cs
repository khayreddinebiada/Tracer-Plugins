using UnityEngine;

namespace tracer
{
    public class DynamicMove : MoveOnTrace
    {
        [SerializeField]
        private float _speedLength = 10;
        [SerializeField]
        private AnimationCurve _speedMoving;

        [SerializeField]
        private float _time = 0;

        // Start is called before the first frame update
        private new void Start()
        {
            base.Start();
            _time = 0;
            onPathEnd += OnEndPath;
        }

        // Update is called once per frame
        public new void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isMoving)
                return;

            _time += Time.fixedDeltaTime;
            mSpeed = _speedMoving.Evaluate(_time) * _speedLength;
        }

        public void OnEndPath()
        {
            _time = 0;
        }
    }
}
