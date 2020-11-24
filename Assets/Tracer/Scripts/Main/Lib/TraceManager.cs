using UnityEngine;

namespace game.lib
{
    public class TraceManager : MonoBehaviour
    {
        
        public Vector3 localScaleInstantiateObject;
        public bool isInstantiateObject;
        public GameObject instantiateObject;
        public bool _activeInstantiates;
        public bool activeInstantiates
        {
            set
            {
                AllInstantiatesActive(value);
                _activeInstantiates = value;
            }
        }
        public bool isStaticDistance = true;
        public float initDistance = 1;



        public GameObject _lastChild
        {
            get 
            {
                if (transform.childCount == 0)
                    return null;
                else
                    return transform.GetChild(transform.childCount - 1).gameObject;
            }
        }

        private float DefineDistance()
        {
            if (isStaticDistance)
                return initDistance;
            else
            {
                if(3 <= transform.childCount)
                {
                    return Vector3.Distance(transform.GetChild(transform.childCount - 2).position, transform.GetChild(transform.childCount - 3).position);
                }
                else
                    return initDistance;
            }
        }

        private Transform[] GetTwoLastPoints()
        {
            if (transform.childCount == 0)
                return null;
            if (transform.childCount == 1)
            {
                return new Transform[]
                {
                transform,
                transform.GetChild(0)
                };
            }
            else
            {
                return new Transform[]
                {
                transform.GetChild(transform.childCount - 2),
                transform.GetChild(transform.childCount - 1)
                };
            }
        }

        private Vector3 GetRayBetweenTwoPoints()
        {
            Transform[] transforms = GetTwoLastPoints();
            if(transforms == null)
            {
                Debug.LogError("You have not any point you can't check the rays");
                return Vector3.zero;
            }

            print(transforms[1].position + " " + transforms[0].position);
            return (transforms[1].position - transforms[0].position).normalized;
        }

        public GameObject AddTracePoint()
        {
            GameObject obj = new GameObject("Point (" + transform.childCount + ")");
            obj.transform.SetParent(transform);
            obj.AddComponent<TracePoint>();

            if (transform.childCount == 1)
            {
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Transform lastChild = transform.GetChild(transform.childCount - 2);
                obj.transform.position = lastChild.position + transform.forward * DefineDistance();
                obj.transform.rotation = lastChild.rotation;
            }

            if (isInstantiateObject)
            {
                GameObject instace = Instantiate(instantiateObject, obj.transform.position, obj.transform.rotation, obj.transform);
                instace.transform.localScale = localScaleInstantiateObject;
            }

            return obj;
        }

        public void DeleteAllInstantiates()
        {
            TracePoint[] tracePoints = GetComponentsInChildren<TracePoint>();

            for (int i = 0; i < tracePoints.Length; i++)
            {
                if (0 < tracePoints[i].transform.childCount)
                {
                    DestroyImmediate(tracePoints[i].transform.GetChild(0).gameObject);
                }
            }
        }

        public void AllInstantiatesActive(bool activate)
        {
            TracePoint[] tracePoints = GetComponentsInChildren<TracePoint>();

            for (int i = 0; i < tracePoints.Length; i++)
            {
                if(0 < tracePoints[i].transform.childCount)
                    tracePoints[i].transform.GetChild(0).gameObject.SetActive(activate);
            }
        }

    }
}