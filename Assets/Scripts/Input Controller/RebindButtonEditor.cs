using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(RebindButton))]
public class RebindButtonEditor : Editor
{
    SerializedProperty actionReferenceProp;
    SerializedProperty dimensionProp;
    SerializedProperty partProp;
    SerializedProperty bindingLabelProp;

    void OnEnable()
    {
        actionReferenceProp = serializedObject.FindProperty("actionReference");
        dimensionProp = serializedObject.FindProperty("dimension");
        partProp = serializedObject.FindProperty("part");
        bindingLabelProp = serializedObject.FindProperty("bindingLabel");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(actionReferenceProp);

        // Show bindingLabel field
        EditorGUILayout.PropertyField(bindingLabelProp);

        // Determine if actionReference is assigned and its type
        InputActionReference actionRef = actionReferenceProp.objectReferenceValue as InputActionReference;
        bool isKeyboard = true;

        if (actionRef != null && actionRef.action != null)
        {
            // Inspect control type
            var controlType = actionRef.action.expectedControlType;
            // If Vector2, composite axes
            if (controlType == "Vector2")
                isKeyboard = true; // still keyboard but composite
            else
                isKeyboard = true; // single button: keyboard/mouse
        }

        // Checkbox to indicate keyboard vs controller
        bool keyboardControls = EditorGUILayout.Toggle("Keyboard Controls", isKeyboard);

        if (actionRef != null && actionRef.action != null)
        {
            var controlType = actionRef.action.expectedControlType;

            if (controlType == "Vector2")
            {
                // Show dimension and part
                EditorGUILayout.PropertyField(dimensionProp);
                EditorGUILayout.PropertyField(partProp);
            }
            else
            {
                // Single button: only positive part exists
                EditorGUILayout.LabelField("Single Button Action (no X/Y)");
                // Still part: use Positive or Negative? maybe always Positive
                EditorGUILayout.PropertyField(partProp, new GUIContent("Part (Pos/Neg)"));
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign ActionReference to configure options.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
