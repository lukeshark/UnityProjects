using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Local Velocity of the Controller.")]
    public class GetTouchControllerVelocity : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Vector3 variable of the Controller Local Velocity.")]
        public FsmVector3 controllerVelocity;

        public bool everyFrame;

        private OVRInput.Controller controllerInput;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        }

        public override void OnEnter()
        {
            switch (touchController)
            {
                case controllerEnum.LeftController:
                    controllerInput = OVRInput.Controller.LTouch;
                    break;
                case controllerEnum.RightController:
                    controllerInput = OVRInput.Controller.RTouch;
                    break;
            }
            DoGetVelocity();

            if (!everyFrame)
            {
                Finish();
            }

        }

        public override void Reset()
        {
            controllerVelocity = null;
        }

        public override void OnUpdate()
        {
            DoGetVelocity();
        }

        void DoGetVelocity()
        {
            controllerVelocity.Value = OVRInput.GetLocalControllerVelocity(controllerInput);
        }
       
    }
}
