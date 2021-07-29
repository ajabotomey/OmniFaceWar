#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

[CustomEditor(typeof(UIRebindControl))]
public class UIRebindControlEditor : Editor
{
    private SerializedProperty m_ActionProperty;
    private SerializedProperty m_BindingIdProperty;
    private SerializedProperty m_DisplayStringOptions;

    private SerializedProperty m_DeviceDisplaySettings;
    private SerializedProperty m_ActionLabel;
    private SerializedProperty m_InputButton;
    private SerializedProperty m_InputButtonLabel;
    private SerializedProperty m_InputButtonImage;
    private SerializedProperty m_ResetButton;

    private GUIContent m_BindingLabel = new GUIContent("Binding");
    private GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");

    private GUIContent[] m_BindingOptions;
    private string[] m_BindingOptionValues;
    private int m_SelectedBindingOption;
    
    // Start is called before the first frame update
    protected void OnEnable()
    {
        m_ActionProperty = serializedObject.FindProperty("actionReference");
        m_BindingIdProperty = serializedObject.FindProperty("bindingID");
        m_DisplayStringOptions = serializedObject.FindProperty("displayStringOptions");
        m_DeviceDisplaySettings = serializedObject.FindProperty("deviceDisplaySettings");
        m_ActionLabel = serializedObject.FindProperty("actionLabel");
        m_InputButton = serializedObject.FindProperty("inputButton");
        m_InputButtonLabel = serializedObject.FindProperty("inputButtonLabel");
        m_InputButtonImage = serializedObject.FindProperty("inputButtonImage");
        m_ResetButton = serializedObject.FindProperty("resetButton");

        RefreshBindingOptions();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // Binding section
        EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);

        EditorGUILayout.PropertyField(m_ActionProperty);

        var newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
        if (newSelectedBinding != m_SelectedBindingOption)
        {
            var bindingId = m_BindingOptionValues[newSelectedBinding];
            m_BindingIdProperty.stringValue = bindingId;
            m_SelectedBindingOption = newSelectedBinding;
        }

        var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptions.intValue;
        //var optionsOld = InputBinding.DisplayStringOptions.DontIncludeInteractions;
        var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
        if (optionsOld != optionsNew)
            m_DisplayStringOptions.intValue = (int)optionsNew;

        EditorGUILayout.Space();

        // Device Display Settings
        EditorGUILayout.PropertyField(m_DeviceDisplaySettings);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_ActionLabel);
    
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_InputButton);
        EditorGUILayout.PropertyField(m_InputButtonLabel);
        EditorGUILayout.PropertyField(m_InputButtonImage);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_ResetButton);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            RefreshBindingOptions();
        }
    }
    
    protected void RefreshBindingOptions()
    {
        var actionReference = (InputActionReference)m_ActionProperty.objectReferenceValue;
        var action = actionReference?.action;

        if (action == null)
        {
            m_BindingOptions = new GUIContent[0];
            m_BindingOptionValues = new string[0];
            m_SelectedBindingOption = -1;
            return;
        }

        var bindings = action.bindings;
        var bindingCount = bindings.Count;

        m_BindingOptions = new GUIContent[bindingCount];
        m_BindingOptionValues = new string[bindingCount];
        m_SelectedBindingOption = -1;

        var currentBindingId = m_BindingIdProperty.stringValue;
        for (var i = 0; i < bindingCount; ++i)
        {
            var binding = bindings[i];
            var bindingId = binding.id.ToString();
            var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

            // If we don't have a binding groups (control schemes), show the device that if there are, for example,
            // there are two bindings with the display string "A", the user can see that one is for the keyboard
            // and the other for the gamepad.
            var displayOptions =
                InputBinding.DisplayStringOptions.DontUseShortDisplayNames | InputBinding.DisplayStringOptions.IgnoreBindingOverrides;
            if (!haveBindingGroups)
                displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

            // Create display string.
            var displayString = action.GetBindingDisplayString(i, displayOptions);

            // If binding is part of a composite, include the part name.
            if (binding.isPartOfComposite)
                displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

            // Some composites use '/' as a separator. When used in popup, this will lead to to submenus. Prevent
            // by instead using a backlash.
            displayString = displayString.Replace('/', '\\');

            // If the binding is part of control schemes, mention them.
            if (haveBindingGroups)
            {
                var asset = action.actionMap?.asset;
                if (asset != null)
                {
                    var controlSchemes = string.Join(", ",
                        binding.groups.Split(InputBinding.Separator)
                            .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                    displayString = $"{displayString} ({controlSchemes})";
                }
            }

            m_BindingOptions[i] = new GUIContent(displayString);
            m_BindingOptionValues[i] = bindingId;

            if (currentBindingId == bindingId)
                m_SelectedBindingOption = i;
        }
    }

    private static class Styles
    {
        public static GUIStyle boldLabel = new GUIStyle("BoldLabel");
    }
}
#endif