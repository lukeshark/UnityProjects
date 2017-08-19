using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using AxlPlay;


namespace AxlPlay {

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;

        fow.FindVisibleTargets();
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewDistance);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewDistance);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewDistance);
       
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }

        DrawFieldOfView(fow);
        //if (!EditorApplication.isPlaying)
        //    DrawFieldOfView(fow);
    }
    void DrawFieldOfView(FieldOfView fow)
    {
       
        float stepAngleSize = fow.viewAngle / fow.numRays;

        for (int i = 0; i <= fow.numRays; i++)
        {
            float angle = fow.transform.eulerAngles.y - fow.viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle, fow);

            Handles.color = Color.cyan;

            Handles.DrawLine(fow.transform.position, newViewCast.point);
           

        }

    }

    ViewCastInfo ViewCast(float globalAngle, FieldOfView fow)
    {
        Vector3 dir = fow.DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(fow.transform.position, dir, out hit, fow.viewDistance, fow.obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, fow.transform.position + dir * fow.viewDistance, fow.viewDistance, globalAngle);
        }
    }
 

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

}
}
