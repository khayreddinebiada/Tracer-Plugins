using UnityEditor;
using UnityEngine;

namespace path
{
    [CustomEditor(typeof(LoadPathManager))]
    [CanEditMultipleObjects]
    public class LoadPathMaker : Editor
    {

        SerializedProperty tracerInfo;

        private void OnEnable()
        {
            tracerInfo = serializedObject.FindProperty("tracerInfo");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(tracerInfo);

            LoadPathManager loadPath = (LoadPathManager)target;
            if (GUILayout.Button("Load Path", GUILayout.Height(50)))
            {
                loadPath.LoadPath();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}