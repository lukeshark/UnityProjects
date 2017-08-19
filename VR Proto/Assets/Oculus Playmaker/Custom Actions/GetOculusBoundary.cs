using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Play Area and OuterBoundary into a Vector3.")]
    public class GetOculusBoundary : FsmStateAction
    {
        [Tooltip("The rectangular space within the outer boundary.")]
        public FsmVector3 playArea;

        [Tooltip("The space that the user defined during configuration.")]
        public FsmVector3 OuterBoundary;

        public override void Reset()
        {
            playArea = null;
            OuterBoundary = null;
        }

        public override void OnEnter()
        {
            playArea.Value = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            OuterBoundary.Value = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.OuterBoundary);
        }


    }
}
