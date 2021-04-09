using System.IO;
using UnityEditor;
using UnityEngine;

namespace path
{
    public enum TransformType
    {
        Local, Global
    }

    public class TraceManager : MonoBehaviour
    {

        public bool activeInstantiates;
        public Vector3 localScaleInstantiateObject = new Vector3(1, 1, 1);
        public GameObject instantiateObject;
        public bool replacePath = false;
        public TransformType transformType = TransformType.Global;
        public string tracerName = "Name";
        private string _folderName = "Paths";

        private string _pathFolder = "/Resources/";
        public string pathFile
        {
            get { return _pathFolder + _folderName; }
        }

        public GameObject lastChild
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
            GameObject obj = new GameObject("Point");
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

            if (instantiateObject != null)
            {
                GameObject instace = Instantiate(instantiateObject, obj.transform.position, obj.transform.rotation, obj.transform);
                instace.transform.localScale = localScaleInstantiateObject;
            }

            return obj;
        }

        public void Generate()
        {

#if UNITY_EDITOR
            if (!Directory.Exists(pathFile))
            {
                Directory.CreateDirectory(Application.dataPath + pathFile);
            }

            TracerInfo asset;
            if (!replacePath)
            {
                asset = CreateTracerInfo();
                AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath("Assets" + pathFile + "/" + tracerName + ".asset"));
            }
            else
            {
                string [] assetNames  = AssetDatabase.FindAssets(tracerName + " t:TracerInfo", new[] { "Assets" + pathFile });
                if (assetNames != null && 0 < assetNames.Length)
                {
                    asset = AssetDatabase.LoadAssetAtPath<TracerInfo>(AssetDatabase.GUIDToAssetPath(assetNames[0]));

                    for (int i = 1; i < assetNames.Length; i++)
                    {
                        TracerInfo tr = AssetDatabase.LoadAssetAtPath<TracerInfo>(AssetDatabase.GUIDToAssetPath(assetNames[i]));

                        if (tr != null && tr.name == tracerName)
                        {
                            asset = tr;
                        }
                    }
                }
                else
                {
                    asset = CreateTracerInfo();
                    AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath("Assets" + pathFile + "/" + tracerName + ".asset"));
                }

                asset.SavePath(GetComponentsInChildren<TracePoint>(), transformType);
            }

            if (replacePath && EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to replace path and lose the old one ?", "Replace", "Cancel"))
            {
                AssetDatabase.SaveAssets();
                LoadPathManager loadPathManager = GetComponent<LoadPathManager>();

                if (loadPathManager != null)
                    loadPathManager.tracerInfo = asset;

                EditorUtility.FocusProjectWindow();

                EditorGUIUtility.PingObject(asset);
                EditorUtility.SetDirty(this);

                AssetDatabase.Refresh();

                Debug.Log("The path is replaced!");
            }

#endif
        }

#if UNITY_EDITOR
        private TracerInfo CreateTracerInfo()
        {
            TracerInfo asset = ScriptableObject.CreateInstance("TracerInfo") as TracerInfo;
            asset.SavePath(GetComponentsInChildren<TracePoint>(), transformType);

            return asset;
        }

#endif
        public void DeleteAllPoints()
        {
            TracePoint[] tracePoints = GetComponentsInChildren<TracePoint>();

            Debug.Log(tracePoints.Length);
            foreach (TracePoint tracePoint in tracePoints)
            {
                DestroyImmediate(tracePoint.gameObject);
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