using game.lib;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TracePoint))]
public class TraceMakerPoint : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Add Trace Point", GUILayout.Height(50)))
        {
            TracePoint tracePoint = (TracePoint)target;
            Selection.activeGameObject = tracePoint.traceManager.AddTracePoint();
        }
    }
}
