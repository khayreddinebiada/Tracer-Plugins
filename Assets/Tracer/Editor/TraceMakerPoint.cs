using game.lib;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TracePoint))]
public class TraceMakerPoint : Editor
{

    SerializedProperty speedOnPoint;

    private void OnEnable()
    {
        speedOnPoint = serializedObject.FindProperty("speedOnPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(speedOnPoint);

        if (GUILayout.Button("Add Trace Point", GUILayout.Height(50)))
        {
            TracePoint tracePoint = (TracePoint)target;
            Selection.activeGameObject = tracePoint.traceManager.AddTracePoint();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
