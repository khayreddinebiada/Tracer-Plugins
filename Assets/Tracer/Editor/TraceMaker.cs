using UnityEditor;
using UnityEngine;

namespace tracer
{
    [CustomEditor(typeof(TraceManager))]
    [CanEditMultipleObjects]
    public class TraceMaker : Editor
    {
        SerializedProperty localScaleInstantiateObject;
        SerializedProperty isInstantiateObject;
        SerializedProperty instantiateObject;
        SerializedProperty initDistance;
        SerializedProperty isStaticDistance;
        SerializedProperty tracerInfo;

        private void OnEnable()
        {
            localScaleInstantiateObject = serializedObject.FindProperty("localScaleInstantiateObject");
            isInstantiateObject = serializedObject.FindProperty("isInstantiateObject");
            instantiateObject = serializedObject.FindProperty("instantiateObject");
            isStaticDistance = serializedObject.FindProperty("isStaticDistance");
            initDistance = serializedObject.FindProperty("initDistance");
            tracerInfo = serializedObject.FindProperty("tracerInfo");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Settings");

            serializedObject.Update();
            EditorGUILayout.PropertyField(localScaleInstantiateObject);
            EditorGUILayout.PropertyField(isInstantiateObject);
            EditorGUILayout.PropertyField(instantiateObject);
            EditorGUILayout.PropertyField(initDistance);
            EditorGUILayout.PropertyField(isStaticDistance);
            EditorGUILayout.PropertyField(tracerInfo);

            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Buttons");
            TraceManager traceManager = (TraceManager)target;
            if (GUILayout.Button("Add Trace Point", GUILayout.Height(50)))
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
            EditorGUILayout.LabelField("");
            EditorGUILayout.HelpBox("If you will click 'Generate' you will delete all points",MessageType.Info);
            if (GUILayout.Button("Generate", GUILayout.Height(30)))
            {
                traceManager.Generate();
                //traceManager.DeleteAllPoints();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
