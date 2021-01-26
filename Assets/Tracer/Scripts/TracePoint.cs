using UnityEngine;

namespace tracer
{
    public class TracePoint : MonoBehaviour
    {
        private Transform nextPoint;
        private bool _isLastPoint = false;

        public TraceManager traceManager
        {
            get
            {
                TraceManager traceManager = GetComponentInParent<TraceManager>();
                if (traceManager != null)
                {
                    return traceManager;
                }
                else
                {
                    Debug.LogError("There is no component TraceManager in parent of this game object...");
                    return null;
                }
            }
        }

        private void Start()
        {
            CheckIfLastPoint();
        }

        private void CheckIfLastPoint()
        {
            if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
            {
                _isLastPoint = true;
            }
            else
            {
                _isLastPoint = false;
            }
        }

        private void OnDrawGizmos()
        {
            CheckIfLastPoint();
            if (!_isLastPoint)
            {
                nextPoint = NextChild();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, nextPoint.position);
            }
        }

        private Transform NextChild()
        {
            int thisIndex = this.transform.GetSiblingIndex();
            if (this.transform.parent == null)
                return null;

            if (this.transform.parent.childCount <= thisIndex + 1)
                return this.transform.parent.GetChild(0).GetComponent<Transform>();

            return this.transform.parent.GetChild(thisIndex + 1).GetComponent<Transform>();
        }
    }

}