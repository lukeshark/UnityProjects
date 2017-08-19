using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Local Acceleration of the Controller.")]
    public class GetTouchControllerAcceleration : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Vector3 variable of the Controller Local Acceleration.")]
        public FsmVector3 controllerAcceleration;

        private OVRInput.Controller controllerInput;

        public bool everyFrame;

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
            DoGetAcceleration();
            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void Reset()
        {
            controllerAcceleration = null;
        }

        public override void OnUpdate()
        {
            DoGetAcceleration();
        }

        void DoGetAcceleration()
        {
            controllerAcceleration.Value = OVRInput.GetLocalControllerAcceleration(controllerInput);
        }
    }
}
