using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the value of the Xbox Thumbstick Axis.")]
    public class GetXboxThumbstickAxis : FsmStateAction
    {

        [Tooltip("Select touch or Press for trigger type.")]
        public axisTriggerType triggerType;

        [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
        public FsmFloat multiplier = 1;

        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeX;

        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeY;

        private OVRInput.Controller controllerInput;

        public enum axisTriggerType
        {
            LeftThumbstick,
            RightThumbstick,
        };

        public override void Reset()
        {
            multiplier = 1f;
            storeX = null;
            storeY = null;
        }
        public override void OnEnter()
        {
            controllerInput = OVRInput.Controller.Gamepad;
        }

        public override void OnUpdate()
        {
            var axisValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerInput);
            switch (triggerType)
            {
                case axisTriggerType.LeftThumbstick:
                    axisValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerInput);

                    if (!multiplier.IsNone)
                        {
                            axisValue *= multiplier.Value;
                        }

                        storeX.Value = axisValue.x;
                        storeY.Value = axisValue.y;

                    break;
                case axisTriggerType.RightThumbstick:
                    axisValue = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, controllerInput);

                    if (!multiplier.IsNone)
                    {
                        axisValue *= multiplier.Value;
                    }

                    storeX.Value = axisValue.x;
                    storeY.Value = axisValue.y;
                    break;
            }

        }
    }
}