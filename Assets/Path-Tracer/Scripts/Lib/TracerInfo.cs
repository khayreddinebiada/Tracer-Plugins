using UnityEngine;

namespace path
{
    [System.Serializable]
    public class PointInfo
    {
        public Vector3 position;
        public Quaternion rotation;

        public PointInfo(Vector3 position)
        {
            this.position = position;
            rotation = Quaternion.identity;
        }

        public PointInfo(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    [CreateAssetMenu(fileName = "TraceInfo", menuName = "Tracer/Path", order = 3)]
    public class TracerInfo : ScriptableObject
    {
        [SerializeField]
        private TransformType _transformType;
        public TransformType transformType
        {
            get { return _transformType; }
        }

        [SerializeField]
        private float _totalDistance;
        public float totalDistance
        {
            get { return _totalDistance; }
        }

        [SerializeField]
        private PointInfo[] _points;
        public PointInfo[] points
        {
            get { return _points; }
            set { _points = value; }
        }

        public void SavePath(TracePoint[] tracePoints, TransformType transformType)
        {
            points = new PointInfo[tracePoints.Length];
            _transformType = transformType;

            for (int i = 0; i < tracePoints.Length; i++)
            {
                PointInfo pInfo = new PointInfo
                    (
                    (transformType == TransformType.Global) ? tracePoints[i].transform.position : tracePoints[i].transform.localPosition,
                    (transformType == TransformType.Global) ? tracePoints[i].transform.rotation : tracePoints[i].transform.localRotation
                     );

                points[i] = pInfo;

            }

            CalculateDistance();
        }

        public void LoadPath(Transform parent)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                
                GameObject point = new GameObject("Point (" + parent.childCount + ")");
                point.AddComponent(typeof(TracePoint));

                point.transform.SetParent(parent);
                if (_transformType == TransformType.Global)
                {
                    point.transform.position = _points[i].position;
                    point.transform.rotation = _points[i].rotation;
                }
                else
                {
                    point.transform.localPosition = _points[i].position;
                    point.transform.localRotation = _points[i].rotation;
                }
            }
        }

        private void CalculateDistance()
        {
            _totalDistance = 0;
            for (int i = 1; i < points.Length; i++)
            {
                _totalDistance  += Vector3.Distance(points[i].position, points[i - 1].position);
            }
        }

        public void ReplacePoints(PointInfo[] points, TransformType transformType)
        {
            _points = points;
            _transformType = transformType;
        }
    }
}
