using UnityEditor;
using UnityEngine;

namespace path
{
    [CustomEditor(typeof(TraceManager))]
    [CanEditMultipleObjects]
    public class TraceMaker : Editor
    {
        SerializedProperty localScaleInstantiateObject;
        SerializedProperty isInstantiateObject;
        SerializedProperty instantiateObject;
        SerializedProperty tracerName;
        SerializedProperty path;

        private void OnEnable()
        {
            localScaleInstantiateObject = serializedObject.FindProperty("localScaleInstantiateObject");
            isInstantiateObject = serializedObject.FindProperty("isInstantiateObject");
            instantiateObject = serializedObject.FindProperty("instantiateObject");
            tracerName = serializedObject.FindProperty("tracerName");
            path = serializedObject.FindProperty("path");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Settings");

            serializedObject.Update();
            EditorGUILayout.PropertyField(localScaleInstantiateObject);
            EditorGUILayout.PropertyField(isInstantiateObject);
            EditorGUILayout.PropertyField(instantiateObject);
            EditorGUILayout.PropertyField(tracerName);
            EditorGUILayout.PropertyField(path);

            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Buttons");

            TraceManager traceManager = (TraceManager)target;
            if (GUILayout.Button("Make Trace", GUILayout.Height(50)))
            {
                traceManager.AddTracePoint();
                Selection.activeGameObject = traceManager._lastChild;
            }

            if (GUILayout.Button("Activated All Instantiates", GUILayout.Height(30)))
            {
                traceManager.AllInstantiatesActive(true);
            }
            if (GUILayout.Button("Deactivated All Instantiates", GUILayout.Height(30)))
            {
                traceManager.AllInstantiatesActive(false);
            }

            EditorGUILayout.LabelField("");
            EditorGUILayout.HelpBox("If you will click 'Generate' you will replace old points",MessageType.Info);
            if (GUILayout.Button("Generate", GUILayout.Height(40)))
            {
                traceManager.Generate();
            }

            EditorGUILayout.LabelField("");
            EditorGUILayout.HelpBox("You will delete all points", MessageType.Warning);
            if (GUILayout.Button("Delete All Points", GUILayout.Height(20)))
            {
                traceManager.DeleteAllPoints();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
