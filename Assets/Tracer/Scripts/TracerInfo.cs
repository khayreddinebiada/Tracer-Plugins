using UnityEngine;

namespace tracer
{
    [System.Serializable]
    public class PointInfo
    {
        public Vector3 position;

        public PointInfo(Vector3 position)
        {
            this.position = position;
        }
    }

    [CreateAssetMenu(fileName = "TraceInfo", menuName = "Tracer/Path", order = 3)]
    public class TracerInfo : ScriptableObject
    {
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

        public void SavePath(TracePoint[] tracePoints)
        {
            points = new PointInfo[tracePoints.Length];

            for (int i = 0; i < tracePoints.Length; i++)
            {
                PointInfo pInfo = new PointInfo(tracePoints[i].transform.position);
                points[i] = pInfo;
            }

            CalculateDistance();
        }

        private void CalculateDistance()
        {
            _totalDistance = 0;
            for (int i = 1; i < points.Length; i++)
            {
                _totalDistance  += Vector3.Distance(points[i].position, points[i - 1].position);
            }
        }
    }
}
