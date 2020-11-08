using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Notification)), CanEditMultipleObjects]
public class NotificationEditor : Editor
{
    private Notification notification;

    private const int MAX_ABBR_VALUE = 110;
    private const int MAX_NORMAL_VALUE = 280;

    SerializedProperty title;
    SerializedProperty image;
    SerializedProperty normalText;
    SerializedProperty abbrvText;
    SerializedProperty type;
    SerializedProperty pushed;
    SerializedProperty action;

    private int characterCount;

    void OnEnable()
    {
        title = serializedObject.FindProperty("title");
        image = serializedObject.FindProperty("image");
        normalText = serializedObject.FindProperty("normalText");
        abbrvText = serializedObject.FindProperty("abbreviatedText");
        type = serializedObject.FindProperty("type");
        pushed = serializedObject.FindProperty("hasBeenPushed");
        action = serializedObject.FindProperty("rewiredAction");
    }

    public override void OnInspectorGUI()
    {
        string normal = TruncateNormalText(normalText.stringValue);
        normalText.stringValue = normal;

        string abbreviatedText = TruncateAbbreviatedText(normal);
        abbrvText.stringValue = abbreviatedText;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(title, new GUIContent("Title:"));
        EditorGUILayout.PropertyField(image, new GUIContent("Image: "));
        EditorGUILayout.PropertyField(normalText, new GUIContent("Normal Text: "), GUILayout.Height(80));
        EditorGUILayout.PropertyField(abbrvText, new GUIContent("Abbreviated Text: "), GUILayout.Height(60));
        EditorGUILayout.PropertyField(type, new GUIContent("Notification Type: "));
        EditorGUILayout.PropertyField(pushed, new GUIContent("Pushed: "));
        EditorGUILayout.PropertyField(action, new GUIContent("Rewired Action: "));

        serializedObject.ApplyModifiedProperties();

        EditorGUI.EndChangeCheck();
    }

    private string TruncateAbbreviatedText(string text)
    {
        int maxLength = CheckEmojis(text, MAX_ABBR_VALUE);

        if (text.Length > maxLength) {
            text = text.Substring(0, maxLength - 4);
            text += "...";
        }

        return text;
    }

    private string TruncateNormalText(string text)
    {
        int maxLength = CheckEmojis(text, MAX_NORMAL_VALUE);

        if (text.Length > maxLength) {
            text = text.Substring(0, maxLength - 1);
        }

        return text;
    }

    private int CheckEmojis(string text, int maxLength)
    {
        char[] separators = { '[', ']' };

        // Go through and detect parenthesis and make substrings
        string[] textChunks = text.Split(separators);

        for (int i = 0; i < textChunks.Length; i++) {
            if (textChunks[i].Contains(":")) {
                maxLength += (textChunks[i].Length + 2); // Account for the square parentheses
            }
        }

        return maxLength;
    }
}
