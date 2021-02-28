using UnityEngine;

namespace path
{  
    public class TracePoint : MonoBehaviour
    {
        public bool useGizmas = true;
        private Transform nextPoint;

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

        public bool CheckIfLastPoint()
        {
            if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDrawGizmos()
        {
            if (useGizmas)
            {
                nextPoint = NextChild();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, nextPoint.position);
                Gizmos.DrawSphere(transform.position, 0.1f);
            }
        }

        private Transform NextChild()
        {
            int thisIndex = transform.GetSiblingIndex();
            if (transform.parent == null)
                return null;

            if (transform.parent.childCount <= thisIndex + 1)
                return transform.parent.GetChild(0).GetComponent<Transform>();

            return transform.parent.GetChild(thisIndex + 1).GetComponent<Transform>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (0 <= transform.childCount)
            {

            }
        }
#endif
    }

}