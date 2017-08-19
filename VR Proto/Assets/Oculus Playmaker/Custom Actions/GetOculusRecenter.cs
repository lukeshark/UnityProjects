using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Recenters the Camera on start of state.")]
    public class GetOculusRecenter : FsmStateAction
    {

        public override void OnEnter()
        {
            OVRManager.display.RecenterPose();
            Finish();
        }
    }
}
