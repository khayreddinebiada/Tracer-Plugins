using UnityEditor;
using UnityEngine;

namespace path
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

        public string tracerName = "New TraceInfo";
        public string path = "Assets/Tracer/Paths/";

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
                obj.transform.position = lastChild.position;
                obj.transform.rotation = lastChild.rotation;
            }

            if (isInstantiateObject)
            {
                GameObject instace = Instantiate(instantiateObject, obj.transform.position, obj.transform.rotation, obj.transform);
                instace.transform.localScale = localScaleInstantiateObject;
            }

            return obj;
        }

        public void Generate()
        {
#if UNITY_EDITOR
            TracerInfo asset = ScriptableObject.CreateInstance("TracerInfo") as TracerInfo;
            asset.SavePath(GetComponentsInChildren<TracePoint>());

            AssetDatabase.CreateAsset(asset, path + tracerName + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            // Selection.activeObject = asset;
            EditorUtility.SetDirty(this);
#endif
        }

        public void DeleteAllPoints()
        {
            TracePoint[] tracePoints = GetComponentsInChildren<TracePoint>();

            for (int i = 0; i < tracePoints.Length; i++)
            {
                if (0 < tracePoints[i].transform.childCount)
                {
                    DestroyImmediate(tracePoints[i].gameObject);
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