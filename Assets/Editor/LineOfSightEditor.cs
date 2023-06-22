using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuardController))]
public class LineOfSightEditor : Editor
{
    private void OnSceneGUI()
    {
        GuardController los = (GuardController) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(los.transform.position, Vector3.up, Vector3.forward, 360, los.radius);

        Vector3 viewAngle01 = DirectionFromAngle(los.transform.eulerAngles.y, -los.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(los.transform.eulerAngles.y, los.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle01 * los.radius);
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle02 * los.radius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float anglesInDegrees)
    {
        anglesInDegrees += eulerY;

        return new Vector3(Mathf.Sin(anglesInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(anglesInDegrees * Mathf.Deg2Rad));
    }
}
