using UnityEditor;
using UnityEngine;

namespace path
{
    [CustomEditor(typeof(TraceManager))]
    [CanEditMultipleObjects]
    public class TraceMaker : Editor
    {
        SerializedProperty activeInstantiates;
        SerializedProperty localScaleInstantiateObject;
        SerializedProperty instantiateObject;
        SerializedProperty tracerName;
        SerializedProperty replacePath;
        SerializedProperty transformType;

        private void OnEnable()
        {
            localScaleInstantiateObject = serializedObject.FindProperty("localScaleInstantiateObject");
            instantiateObject = serializedObject.FindProperty("instantiateObject");
            activeInstantiates = serializedObject.FindProperty("activeInstantiates");
            tracerName = serializedObject.FindProperty("tracerName");
            replacePath = serializedObject.FindProperty("replacePath");
            transformType = serializedObject.FindProperty("transformType");
        }

        public override void OnInspectorGUI()
        {
            GUIStyle header = new GUIStyle(GUI.skin.label);
            header.margin = new RectOffset(25, 20, 20, 5);
            header.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField("Settings", header);

            TraceManager traceManager = (TraceManager)target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(activeInstantiates);
            if (traceManager.activeInstantiates)
            {
                EditorGUILayout.PropertyField(instantiateObject);
                EditorGUILayout.PropertyField(localScaleInstantiateObject);
            }

            EditorGUILayout.PropertyField(transformType);
            EditorGUILayout.PropertyField(tracerName);
            EditorGUILayout.PropertyField(replacePath);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Path Info saved in:   " + traceManager.pathFile);
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Buttons", header);

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
