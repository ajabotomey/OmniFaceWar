using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Notification)), CanEditMultipleObjects]
public class NotificationEditor : Editor
{
    private Notification notification;

    private const int MAX_ABBR_VALUE = 88;
    private const int MAX_NORMAL_VALUE = 280;

    SerializedProperty title;
    SerializedProperty image;
    SerializedProperty normalText;
    SerializedProperty abbrvText;
    SerializedProperty type;
    SerializedProperty pushed;

    void OnEnable()
    {
        title = serializedObject.FindProperty("title");
        image = serializedObject.FindProperty("image");
        normalText = serializedObject.FindProperty("normalText");
        abbrvText = serializedObject.FindProperty("abbreviatedText");
        type = serializedObject.FindProperty("type");
        pushed = serializedObject.FindProperty("hasBeenPushed");
    }

    public override void OnInspectorGUI()
    {
        string normal = TruncateNormalText(normalText.stringValue);
        normalText.stringValue = normal;

        string abbreviatedText = TruncateAbbreviatedText(normalText.stringValue);
        abbrvText.stringValue = abbreviatedText;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(title, new GUIContent("Title:"));
        EditorGUILayout.PropertyField(image, new GUIContent("Image: "));
        EditorGUILayout.PropertyField(normalText, new GUIContent("Normal Text: "), GUILayout.Height(80));
        EditorGUILayout.PropertyField(abbrvText, new GUIContent("Abbreviated Text: "), GUILayout.Height(60));
        EditorGUILayout.PropertyField(type, new GUIContent("Notification Type: "));
        EditorGUILayout.PropertyField(pushed, new GUIContent("Pushed: "));

        serializedObject.ApplyModifiedProperties();

        EditorGUI.EndChangeCheck();
    }

    private string TruncateAbbreviatedText(string text)
    {
        if (text.Length > MAX_ABBR_VALUE) {
            text = text.Substring(0, MAX_ABBR_VALUE - 4);
            text += "...";
        }

        return text;
    }

    private string TruncateNormalText(string text)
    {
        if (text.Length > MAX_NORMAL_VALUE) {
            text = text.Substring(0, MAX_NORMAL_VALUE - 1);
        }

        return text;
    }
}
