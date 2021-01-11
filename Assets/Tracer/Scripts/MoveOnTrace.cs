using game.lib;
using UnityEngine;
using System.Collections;

public class MoveOnTrace : MonoBehaviour
{
    public enum TraceType { TracePosition, TraceRotation }

    public enum SpeedType { Static, Dynamic }
    
    [Header("Transformation")]
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }

    [SerializeField]
    private SpeedType _speedType = SpeedType.Static;

    [SerializeField]
    private float _movingSpeed = 10;
    private float _currentMovingSpeed;
    public float movingSpeed
    {
        get { return _movingSpeed; }
        set { _movingSpeed = value; }
    }

    [SerializeField]
    private float _deltaSpeed = 1f;

    [SerializeField]
    private float _rotateSpeed = 10;
    private float _currentRotateSpeed;


    [Header("Other Settings")]
    [SerializeField]
    public TraceType _traceType = TraceType.TracePosition;

    [SerializeField]
    private bool _isLooping = false;

    [SerializeField]
    private bool _initializeByFirstPoint;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform pointsContent;

    private TracePoint[] _points;
    private Transform _nextPoint;
    private int _indexNextPoint;


    // Start is called before the first frame update
    private void Awake()
    {
        if (pointsContent == null)
            pointsContent = transform;

        if (_target == null)
            _target = transform;

        _points = pointsContent.GetComponentsInChildren<TracePoint>();

        InitializateTrace();
    }

    private void InitializateTrace()
    {
        // Check if there is any error.
        if (_points.Length == 0)
        {
            Debug.LogError("No points for trace...");
            return;
        }
        if (_initializeByFirstPoint && _points.Length == 1)
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
            _nextPoint = _points[_indexNextPoint].transform;
            _target.position = _points[0].transform.position;
            _target.rotation = _points[0].transform.rotation;

            if (_speedType == SpeedType.Dynamic)
            {
                _currentMovingSpeed = _points[1].speedOnPoint * _movingSpeed;
                _currentRotateSpeed = _points[1].speedOnPoint * _rotateSpeed;
            }
            else
            {
                _currentMovingSpeed = _movingSpeed;
                _currentRotateSpeed = _rotateSpeed;
            }
        }
        else
        {
            _indexNextPoint = 0;
            _nextPoint = _points[_indexNextPoint].transform;

            if (_speedType == SpeedType.Dynamic)
            {
                _currentMovingSpeed = _points[0].speedOnPoint * _movingSpeed;
                _currentRotateSpeed = _points[0].speedOnPoint * _rotateSpeed;
            }
            else
            {
                _currentMovingSpeed = _movingSpeed;
                _currentRotateSpeed = _rotateSpeed;
            }
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isMoving || _target == null)
            return;

        ContollerSpeed();

        _target.position = Vector3.MoveTowards(_target.position, _nextPoint.position, _currentMovingSpeed * Time.fixedDeltaTime);
        _target.rotation = Quaternion.RotateTowards(_target.rotation, _nextPoint.rotation, _currentRotateSpeed * Time.fixedDeltaTime);

        if(_traceType == TraceType.TracePosition)
        {
            if (Vector3.Distance(_target.position, _nextPoint.position) <= 0.05f)
            {
                GoNext();
            }
        }
        else
        {
            if (Vector3.Distance(_target.eulerAngles, _nextPoint.eulerAngles) <= 0.05f)
            {
                GoNext();
            }
        }
    }

    private void ContollerSpeed()
    {
        if (_speedType == SpeedType.Dynamic)
        {
            _currentMovingSpeed = Mathf.MoveTowards(_currentMovingSpeed, _points[_indexNextPoint].speedOnPoint * _movingSpeed, _deltaSpeed * Time.fixedDeltaTime);
            _currentRotateSpeed = Mathf.MoveTowards(_currentRotateSpeed, _points[_indexNextPoint].speedOnPoint * _rotateSpeed, _deltaSpeed * Time.fixedDeltaTime);
        }
    }

    private void GoNext()
    {
        _indexNextPoint++;
        if (_indexNextPoint == _points.Length)
        {
            _indexNextPoint = 0;
            OnEndPath();
        }
        _nextPoint = _points[_indexNextPoint].transform;
    }

    private void OnEndPath()
    {
        if (!_isLooping)
        {
            _isMoving = false;
        }
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public Transform GetPoint(int index)
    {
        if (_points.Length <= index)
            return null;

        return _points[index].transform;
    }

    public float TotalDistance()
    {
        float distance = 0;

        for (int i = 1; i < _points.Length; i++)
        {
            distance += Vector3.Distance(_points[i - 1].transform.position, _points[i].transform.position);
        }

        return distance;
    }
}
