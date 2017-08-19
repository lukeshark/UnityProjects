using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("Agents AI")]
    public class FieldOfView : FsmStateAction
    {
        #region Public Variables

        public float viewDistance;

        [Tooltip("Target mask, Pick only from these layers.")]
        [Range(0, 360)]
        public float viewAngle;

        [Tooltip("Target mask, Pick only from these layers.")]
        [UIHint(UIHint.Layer)]
        public FsmInt[] targetMask;


        [Tooltip("Obstacle Mask, Pick only from these layers.")]
        [UIHint(UIHint.Layer)]
        public FsmInt[] obstacleMask;

        public int numRaysScene = 20;


        [Tooltip("The closest target.")]
        [UIHint(UIHint.FsmGameObject)]
        public FsmGameObject StoreTaget;

        [Tooltip("Event to send if get a target object.")]
        [UIHint(UIHint.Variable)]
        public FsmEvent hitEvent;
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        #endregion

        #region Private Variables
        List<Transform> visibleTargets = new List<Transform>();
        #endregion

        public override void Reset()
        {
	        targetMask = new FsmInt[1];
	        obstacleMask = new FsmInt[1];
            viewAngle = 90;
            viewDistance = 5f;
            everyFrame = false;
        }

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if (!everyFrame)
            {
                Finish();
            }

        }
        public override void OnUpdate()
        {
            StoreTaget.Value = FindVisibleTargets();

            if (StoreTaget.Value != null && hitEvent != null)
            {
                Fsm.Event(hitEvent);
                Finish();
            }
        }
        public override void OnDrawActionGizmos()
        {
#if UNITY_EDITOR
            if (Owner != null)
            {
                FindVisibleTargets();

                Handles.color = Color.white;
                Handles.DrawWireArc(Owner.transform.position, Vector3.up, Vector3.forward, 360, viewDistance);
                Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
                Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

                Handles.DrawLine(Owner.transform.position, Owner.transform.position + viewAngleA * viewDistance);
                Handles.DrawLine(Owner.transform.position, Owner.transform.position + viewAngleB * viewDistance);

                foreach (var visibleTarget in visibleTargets)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(Owner.transform.position, visibleTarget.transform.position);
                }

                DrawFieldOfView();
            }
#endif
        }

        public GameObject FindVisibleTargets()
        {
            GameObject closest = null;
            visibleTargets.Clear();
            if (targetMask.Length > 0)
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(Owner.transform.position, viewDistance, ActionHelpers.LayerArrayToLayerMask(targetMask, false));

                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - Owner.transform.position).normalized;
                    if (Vector3.Angle(Owner.transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dstToTarget = Vector3.Distance(Owner.transform.position, target.position);
                        if (obstacleMask.Length > 0)
                        {
                            if (!Physics.Raycast(Owner.transform.position, dirToTarget, dstToTarget, ActionHelpers.LayerArrayToLayerMask(obstacleMask, false)))
                            {
                                visibleTargets.Add(target);

                            }
                        }
                    }
                }
            }


            if (visibleTargets.Count > 0)
            {
                
                float distance = Mathf.Infinity;

                foreach (var go in visibleTargets)
                {
                    Vector3 diff = go.transform.position - Owner.transform.position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = go.gameObject;

                        distance = curDistance;
                    }


                }
                
            }
            return closest;


        }
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += Owner.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
#if UNITY_EDITOR   
        void DrawFieldOfView()
	    {
	    	
       
		    float stepAngleSize = viewAngle / numRaysScene;

            for (int i = 0; i <= numRaysScene; i++)
            {

                float angle = Owner.transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);


                Handles.color = Color.cyan;

                Handles.DrawLine(Owner.transform.position, newViewCast.point);

            }

        }
#endif
        ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(Owner.transform.position, dir, out hit, viewDistance, ActionHelpers.LayerArrayToLayerMask(obstacleMask, false)))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, Owner.transform.position + dir * viewDistance, viewDistance, globalAngle);
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
