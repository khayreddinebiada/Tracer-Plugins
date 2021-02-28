using UnityEditor;
using UnityEngine;

namespace path
{
    [CustomEditor(typeof(TraceManager))]
    [CanEditMultipleObjects]
    public class TraceMaker : Editor
    {
        SerializedProperty localScaleInstantiateObject;
        SerializedProperty instantiateObject;
        SerializedProperty tracerName;
        SerializedProperty path;

        private void OnEnable()
        {
            localScaleInstantiateObject = serializedObject.FindProperty("localScaleInstantiateObject");
            instantiateObject = serializedObject.FindProperty("instantiateObject");
            tracerName = serializedObject.FindProperty("tracerName");
            path = serializedObject.FindProperty("path");
        }

        public override void OnInspectorGUI()
        {
            GUIStyle header = new GUIStyle(GUI.skin.label);
            header.margin = new RectOffset(25, 20, 20, 5);
            header.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField("Settings", header);

            serializedObject.Update();
            EditorGUILayout.PropertyField(localScaleInstantiateObject);
            EditorGUILayout.PropertyField(instantiateObject);
            EditorGUILayout.PropertyField(tracerName);
            EditorGUILayout.PropertyField(path);

            EditorGUILayout.LabelField("Buttons", header);

            TraceManager traceManager = (TraceManager)target;
            if (GUILayout.Button("Make Trace", GUILayout.Height(30)))
            {
                traceManager.AddTracePoint();
                Selection.activeGameObject = traceManager.lastChild;
            }
            
            if (GUILayout.Button("Activated All Instantiates", GUILayout.Height(30)))
            {
                traceManager.AllInstantiatesActive(true);
            }
            if (GUILayout.Button("Deactivated All Instantiates", GUILayout.Height(30)))
            {
                traceManager.AllInstantiatesActive(false);
            }

            if (GUILayout.Button("Delete All Points", GUILayout.Height(20)))
            {
                if (EditorUtility.DisplayDialog("Delete all points", "You will delete all TracePoint in children of TraceManager.", "Delete", "Cancel"))
                {
                    traceManager.DeleteAllPoints();
                }
            }

            EditorGUILayout.LabelField("Generate", header);
            if (GUILayout.Button("Generate", GUILayout.Height(50)))
            {
                traceManager.Generate();
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
