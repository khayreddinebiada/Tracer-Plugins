using UnityEditor;
using UnityEngine;
using game.lib;


[CustomEditor(typeof(TraceManager))]
[CanEditMultipleObjects]
public class TraceMaker : Editor
{
    SerializedProperty localScaleInstantiateObject;
    SerializedProperty isInstantiateObject;
    SerializedProperty instantiateObject;
    SerializedProperty initDistance;
    SerializedProperty isStaticDistance;

    private void OnEnable()
    {
        localScaleInstantiateObject = serializedObject.FindProperty("localScaleInstantiateObject");
        isInstantiateObject = serializedObject.FindProperty("isInstantiateObject");
        instantiateObject = serializedObject.FindProperty("instantiateObject");
        isStaticDistance = serializedObject.FindProperty("isStaticDistance");
        initDistance = serializedObject.FindProperty("initDistance");
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

        if (GUILayout.Button("Delete All Instantiates", GUILayout.Height(50)))
        {
            traceManager.DeleteAllInstantiates();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
