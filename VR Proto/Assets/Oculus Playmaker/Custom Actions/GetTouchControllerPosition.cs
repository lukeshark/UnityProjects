using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Local Position of the Controller.")]
    public class GetTouchControllerPosition : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Vector3 variable of the Controller Local Position.")]
        public FsmVector3 controllerPosition;

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

        }

        public override void Reset()
        {
            controllerPosition = null;
        }

        public override void OnUpdate()
        {
            controllerPosition.Value = OVRInput.GetLocalControllerPosition(controllerInput);
        }
    }
}
