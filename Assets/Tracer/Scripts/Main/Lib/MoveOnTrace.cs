using game.lib;
using UnityEngine;
using System.Collections;

public class MoveOnTrace : MonoBehaviour
{
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }

    [SerializeField]
    private bool _isLooping = false;

    [SerializeField]
    private bool _initializeByFirstPoint;
    [SerializeField]
    private float _movingSpeed = 10;
    [SerializeField]
    private float _rotateSpeed = 10;

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
        }
        else
        {
            _indexNextPoint = 0;
            _nextPoint = _points[_indexNextPoint].transform;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isMoving || _target == null)
            return;

        _target.position = Vector3.MoveTowards(_target.position, _nextPoint.position, _movingSpeed * Time.fixedDeltaTime);
        _target.rotation = Quaternion.RotateTowards(_target.rotation, _nextPoint.rotation, _rotateSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(_target.position, _nextPoint.position) <= 0.05f)
        {
            GoNextPosition();
        }
    }

    private void GoNextPosition()
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

}
