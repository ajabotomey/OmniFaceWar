using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfViewSensor))]
public class FieldOfViewSensorEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfViewSensor fov = (FieldOfViewSensor)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.back, Vector2.up, 360, fov.ViewRadius);

        Vector3 viewAngleA = fov.DirFromAngle(-fov.ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.ViewAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets) {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
