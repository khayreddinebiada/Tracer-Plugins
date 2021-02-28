using UnityEditor;
using UnityEngine;

namespace path
{
    [CustomEditor(typeof(TracePoint))]
    public class TraceMakerPoint : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Add Trace Point", GUILayout.Height(30)))
            {
                TracePoint tracePoint = (TracePoint)target;
                Selection.activeGameObject = tracePoint.traceManager.AddTracePoint();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}