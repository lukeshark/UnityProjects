using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Local Angular Velocity of the Controller.")]
    public class GetTouchControllerAngularVelocity : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Vector3 variable of the Controller Local Angular Velocity.")]
        public FsmVector3 controllerAngularVelocity;

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
            controllerAngularVelocity = null;
        }

        public override void OnUpdate()
        {
            DoGetVelocity();
        }

        void DoGetVelocity()
        {
            controllerAngularVelocity.Value = OVRInput.GetLocalControllerAngularVelocity(controllerInput);
        }
    }
}
