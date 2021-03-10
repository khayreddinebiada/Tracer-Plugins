using UnityEngine;

namespace path
{
    public class DynamicMove : MoveOnTrace
    {
        [Header("Graph")]
        [SerializeField]
        private AnimationCurve _movingSpeedGraph;

        private float _movingSpeed = 10;
        private float _time = 0;

        // Start is called before the first frame update
        private new void Start()
        {
            base.Start();
            _time = 0;
            onPathEnd += OnEndPath;
            _movingSpeed = movingSpeed;
        }

        // Update is called once per frame
        public new void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isMoving || _target == null)
                return;

            _time += Time.fixedDeltaTime;
            movingSpeed = _movingSpeedGraph.Evaluate(_time) * _movingSpeed;

        }

        private void OnDestroy()
        {
            onPathEnd -= OnEndPath;
        }

        public void OnEndPath()
        {
            _time = 0;
        }

        /*
#if UNITY_EDITOR
        private void OnValidate()
        {
            _movingSpeedGraph.AddKey(new Keyframe(1, 1));
            if (_target == null)
                _target = base.transform;
        }
#endif*/
    }
}
