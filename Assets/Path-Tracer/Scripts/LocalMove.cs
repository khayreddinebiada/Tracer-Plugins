using UnityEngine;

namespace path
{
    public class LocalMove : MonoBehaviour
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
        protected int _indexNextPoint;


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

        protected void Awake()
        {
            if (_points == null)
            {
                Debug.LogError("You need add tracer info...");
                enabled = false;
            }

            if (_target == null)
                _target = transform;
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
                _target.localPosition = _points.points[0].position;
            }
            else
            {
                _indexNextPoint = 0;
                _nextPosition = _points.points[_indexNextPoint].position;
            }
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (!_isMoving || _target == null)
                return;

            _target.localPosition = Vector3.MoveTowards(_target.localPosition, _nextPosition, _movingSpeed * Time.fixedDeltaTime);
            if (Vector3.Distance(_target.localPosition, _nextPosition) <= 0.05f)
            {
                GoNext();
            }
        }

        private void GoNext()
        {
            _target.localPosition = _points.points[_indexNextPoint].position;

            _indexNextPoint++;
            if (_indexNextPoint == _points.points.Length)
            {
                EndPath();
                _indexNextPoint = 0;
            }

            _nextPosition = _points.points[_indexNextPoint].position;
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