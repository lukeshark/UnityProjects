using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the Local Rotation of the Controller.")]
    public class GetTouchControllerRotation : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Quaternion variable of the Controller Local Rotation.")]
        public FsmQuaternion controllerRotation;

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
            controllerRotation = null;
        }

        public override void OnUpdate()
        {
            controllerRotation.Value = OVRInput.GetLocalControllerRotation(controllerInput);
        }
    }
}
