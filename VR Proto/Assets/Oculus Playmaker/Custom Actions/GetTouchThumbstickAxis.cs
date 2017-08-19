using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the value of the Thumbstick Axis.")]
    public class GetTouchThumbstickAxis : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
        public FsmFloat multiplier = 1;


        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeX;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeY;

        private OVRInput.Controller controllerInput;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        }

        public override void Reset()
        {
            multiplier = 1f;
            storeX = null;
            storeY = null;
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

        public override void OnUpdate()
        {
            var axisValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerInput);


                    axisValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerInput);
                    if (!multiplier.IsNone)
                    {
                        axisValue *= multiplier.Value;
                    }

                    storeX.Value = axisValue.x;
                    storeY.Value = axisValue.y;

        }
    }
}