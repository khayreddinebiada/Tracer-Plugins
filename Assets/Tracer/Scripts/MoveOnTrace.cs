using System;
using System.Linq;
using UnityEngine;

namespace tracer
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

        private void Awake()
        {
            if (_points == null)
                Debug.LogError("You need add tracer info...");

            if (_target == null)
                _target = transform;
        }


        // Start is called before the first frame update
        protected void Start()
        {
            InitializateTrace();
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (!_isMoving || _target == null)
                return;

            _target.position = Vector3.MoveTowards(_target.position, _nextPoint, _mSpeed * Time.fixedDeltaTime);
            //_target.rotation = Quaternion.RotateTowards(_target.rotation, _nextPoint.rotation, _rotateSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(_target.position, _nextPoint) <= 0.05f)
            {
                GoNext();
            }
        }

        private void InitializateTrace()
        {
            // Check if there is any error.
            if (_points.points.Length == 0)
            {
                Debug.LogError("No points for trace...");
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