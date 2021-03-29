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
        private float _movingSpeed = 10;
        protected float movingSpeed
        {
            get { return _movingSpeed; }
            set { _movingSpeed = value; }
        }

        [SerializeField]
        private bool _isLooping = false;

        [SerializeField]
        protected TracerInfo _points;



        [Header("Settings")]
        [SerializeField]
        protected bool _initializeByFirstPoint = true;

        [SerializeField]
        protected Transform _target;

        protected Vector3 _nextPosition;
        protected Quaternion _nextRotation;
        protected int _indexNextPoint;

        private float _rotateSpeed = 0;


        public delegate void Action();
        private event Action _onPathEnd;
        public event Action onPathEnd
        {
            add
            {
                _onPathEnd += value;
            }
            remove
            {
                _onPathEnd -= value;
            }
        }

        private TransformType _transformType;

        protected void Awake()
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
                _nextPosition = _points.points[_indexNextPoint].position;
                _nextRotation = _points.points[_indexNextPoint].rotation;
                _target.position = _points.points[0].position;
                _target.rotation = _points.points[0].rotation;
            }
            else
            {
                _indexNextPoint = 0;
                _nextPosition = _points.points[_indexNextPoint].position;
                _nextRotation = _points.points[_indexNextPoint].rotation;
            }

            DefineRotateSpeed();
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (!_isMoving || _target == null)
                return;

            if (_transformType == TransformType.Global)
            {
                _target.position = Vector3.MoveTowards(_target.position, _nextPosition, _movingSpeed * Time.fixedDeltaTime);
                if (Vector3.Distance(_target.position, _nextPosition) <= 0.05f)
                {
                    GoNext();
                }
            }
            else
            {
                _target.localPosition = Vector3.MoveTowards(_target.localPosition, _nextPosition, _movingSpeed * Time.fixedDeltaTime);
                if (Vector3.Distance(_target.localPosition, _nextPosition) <= 0.05f)
                {
                    GoNext();
                }
            }

            _target.rotation = Quaternion.RotateTowards(_target.rotation, _nextRotation, _rotateSpeed * Time.fixedDeltaTime);
        }

        private void GoNext()
        {

            _target.rotation = _points.points[_indexNextPoint].rotation;
            _target.position = _points.points[_indexNextPoint].position;

            _indexNextPoint++;
            if (_indexNextPoint == _points.points.Length)
            {
                EndPath();
                _indexNextPoint = 0;
            }
            _nextPosition = _points.points[_indexNextPoint].position;
            _nextRotation = _points.points[_indexNextPoint].rotation;

            DefineRotateSpeed();
        }

        private void DefineRotateSpeed()
        {
            float distancePoints = Vector3.Distance(_nextPosition, _target.position);
            float deferenceAngle = Quaternion.Angle(transform.rotation, _nextRotation);
            
            _rotateSpeed = deferenceAngle * _movingSpeed / distancePoints;
        }

        private void EndPath()
        {
            if (_onPathEnd != null)
                _onPathEnd.Invoke();

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