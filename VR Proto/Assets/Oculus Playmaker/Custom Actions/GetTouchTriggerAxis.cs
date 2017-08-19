/*
Receives the axis of the trigger based on press.
*/
using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the value of the Trigger Axis.")]
    public class GetTouchTriggerAxis : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Select touch or Press for trigger type.")]
        public axisTriggerType triggerType;

        [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
        public FsmFloat multiplier = 1;

        [Tooltip("Store the result in a float variable.")]
        public FsmFloat store;

        private OVRInput.Controller controllerInput;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        };

        public enum axisTriggerType
        {
            PrimaryIndexTrigger,
            PrimaryHandTrigger,
        };

        public override void Reset()
        {
            multiplier = 1f;
            store = null;
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
            var axisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerInput);
            switch (triggerType)
            {
                case axisTriggerType.PrimaryIndexTrigger:
                    axisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerInput);

                    if (!multiplier.IsNone)
                        {
                            axisValue *= multiplier.Value;
                        }

                        store.Value = axisValue;
                    break;
                case axisTriggerType.PrimaryHandTrigger:
                    axisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerInput);

                    if (!multiplier.IsNone)
                    {
                        axisValue *= multiplier.Value;
                    }

                    store.Value = axisValue;
                    break;
            }

        }
    }
}