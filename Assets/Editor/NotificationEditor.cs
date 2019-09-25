using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Notification)), CanEditMultipleObjects]
public class NotificationEditor : Editor
{
    private Notification notification;

    private const int MAX_ABBR_VALUE = 88;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        notification = (Notification)target;

        GUIStyle guiStyle = EditorStyles.textArea;
        guiStyle.wordWrap = true;

        string abbreviatedText = TruncateAbbreviatedText(notification.NormalText);

        EditorGUI.BeginChangeCheck();

        notification.Title = EditorGUILayout.TextField("Title: ", notification.Title);
        notification.Image = (Sprite)EditorGUILayout.ObjectField("Sprite: ", notification.Image, typeof(Sprite), true);
        EditorGUILayout.PrefixLabel("Normal Text: ");
        notification.NormalText = EditorGUILayout.TextArea(notification.NormalText, GUILayout.MaxHeight(40));
        EditorGUILayout.PrefixLabel("Abbreviated Text: ");
        notification.AbbreviatedText = EditorGUILayout.TextArea(abbreviatedText, GUILayout.MaxHeight(40));
        notification.Type = (NotificationType)EditorGUILayout.EnumPopup("Notification Type: ", notification.Type);
        notification.Pushed = EditorGUILayout.Toggle("Pushed: ", notification.Pushed);

        EditorGUI.EndChangeCheck();
    }

    private string TruncateAbbreviatedText(string text)
    {
        if (text.Length > MAX_ABBR_VALUE) {
            text = text.Substring(0, MAX_ABBR_VALUE - 1);
        }

        return text;
    }
}
