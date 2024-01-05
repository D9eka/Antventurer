using Components.UI.Dialogs;
using UnityEditor;
using UnityEngine;

namespace Components.LevelManagement.Editor
{
    [CustomEditor(typeof(LoadLevelComponent))]
    public class LoadLevelComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _windowProperty;
        private SerializedProperty _modeProperty;
        private SerializedProperty _cleanPlayerPrefsProperty;

        private SerializedProperty _sceneNameProperty;
        private SerializedProperty _positionProperty;
        private SerializedProperty _invertScaleProperty;

        private void OnEnable()
        {
            _windowProperty = serializedObject.FindProperty("_window");
            _modeProperty = serializedObject.FindProperty("_mode");
            _cleanPlayerPrefsProperty = serializedObject.FindProperty("_cleanPlayerPrefs");

            _sceneNameProperty = serializedObject.FindProperty("_sceneName");
            _positionProperty = serializedObject.FindProperty("_position");
            _invertScaleProperty = serializedObject.FindProperty("_invertScale");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_windowProperty);
            EditorGUILayout.PropertyField(_modeProperty);
            EditorGUILayout.PropertyField(_cleanPlayerPrefsProperty);
 
            LoadingMode mode = (LoadingMode)_modeProperty.intValue;
            switch (mode)
            {
                case LoadingMode.Manually:
                    EditorGUILayout.PropertyField(_sceneNameProperty);
                    EditorGUILayout.PropertyField(_positionProperty);
                    EditorGUILayout.PropertyField(_invertScaleProperty);
                    break;
                default:
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}