using UnityEditor;

namespace Components.UI.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _dialogBoxProperty;
        private SerializedProperty _onStartProperty;
        private SerializedProperty _onFinishProperty;

        private SerializedProperty _modeProperty;
        private SerializedProperty _boundModeProperty;
        private SerializedProperty _externalModeProperty;

        private void OnEnable()
        {
            _dialogBoxProperty = serializedObject.FindProperty("_dialogBox");
            _onStartProperty = serializedObject.FindProperty("_onStart");
            _onFinishProperty = serializedObject.FindProperty("_onFinish");

            _modeProperty = serializedObject.FindProperty("_mode");
            _boundModeProperty = serializedObject.FindProperty("_bound");
            _externalModeProperty = serializedObject.FindProperty("_external");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_dialogBoxProperty);
            EditorGUILayout.PropertyField(_onStartProperty);
            EditorGUILayout.PropertyField(_onFinishProperty);

            EditorGUILayout.PropertyField(_modeProperty);

            Mode mode = (Mode)_modeProperty.intValue;
            switch (mode)
            {
                case Mode.Bound:
                    EditorGUILayout.PropertyField(_boundModeProperty);
                    break;
                case Mode.External:
                    EditorGUILayout.PropertyField(_externalModeProperty);
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}