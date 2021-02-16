using System;
using System.Linq;
using UnityEngine;

namespace path
{
    public class MoveOnTrace : MonoBehaviour
    {
        [Header("Transformation")]
        [SerializeField]
        protected bool _isMoving = false;
        public bool isMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }

        [SerializeField]
        private float _mSpeed = 10;
        protected float mSpeed
        {
            get { return _mSpeed; }
            set { _mSpeed = value; }
        }


        [SerializeField]
        protected TracerInfo _points;

        [SerializeField]
        protected bool _initializeByFirstPoint;

        [SerializeField]
        protected Transform _target;

        protected Vector3 _nextPoint;
        protected int _indexNextPoint;

        [SerializeField]
        private bool _isLooping = false;

        public delegate void Action();
        protected event Action onPathEnd;

        private TracerInfo.TransformType _transformType;

        private void Awake()
        {
            if (_points == null)
            {
                Debug.LogError("You need add tracer info...");
                enabled = false;
            }

            if (_target == null)
                _target = transform;

            _transformType = _points.transformType;
        }

        // Start is called before the first frame update
        protected void Start()
        {
            InitializateTrace();
        }

        private void InitializateTrace()
        {
            // Check if there is any error.
            if (_points.points.Length == 0)
            {
                Debug.LogError("No points for trace...");
                enabled = false;
                return;
            }
            if (_initializeByFirstPoint && _points.points.Length == 1)
            {
                Debug.LogError("There is just one point it's not enough");
                return;
            }
            if (_target == null)
            {
                Debug.Log("No target right now ...");
                return;
            }

            // Initialization the values.
            if (_initializeByFirstPoint)
            {
                _indexNextPoint = 1;
                _nextPoint = _points.points[_indexNextPoint].position;
                _target.position = _points.points[0].position;
            }
            else
            {
                _indexNextPoint = 0;
                _nextPoint = _points.points[_indexNextPoint].position;
            }
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (!_isMoving || _target == null)
                return;

            if (_transformType == TracerInfo.TransformType.Global)
            {
                _target.position = Vector3.MoveTowards(_target.position, _nextPoint, _mSpeed * Time.fixedDeltaTime);
                if (Vector3.Distance(_target.position, _nextPoint) <= 0.05f)
                {
                    GoNext();
                }
            }
            else
            {
                _target.localPosition = Vector3.MoveTowards(_target.localPosition, _nextPoint, _mSpeed * Time.fixedDeltaTime);
                if (Vector3.Distance(_target.localPosition, _nextPoint) <= 0.05f)
                {
                    GoNext();
                }
            }

            if (Vector3.Distance(_target.position, _nextPoint) <= 0.05f)
            {
                GoNext();
            }
        }

        private void GoNext()
        {
            _indexNextPoint++;
            if (_indexNextPoint == _points.points.Length)
            {
                EndPath();
                _indexNextPoint = 0;
            }
            _nextPoint = _points.points[_indexNextPoint].position;
        }

        private void EndPath()
        {
            if(onPathEnd != null)
                onPathEnd.Invoke();

            if (!_isLooping)
            {
                _isMoving = false;
            }
        }

        public void StopMoving()
        {
            _isMoving = false;
        }


        public float TotalDistance()
        {
            return _points.totalDistance;
        }
    }
}