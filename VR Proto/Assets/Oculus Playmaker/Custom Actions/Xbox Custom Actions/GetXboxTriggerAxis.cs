using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Gets the value of the Xbox Trigger Axis.")]
    public class GetXboxTriggerAxis : FsmStateAction
    {

        [Tooltip("Select touch or Press for trigger type.")]
        public axisTriggerType triggerType;

        [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
        public FsmFloat multiplier = 1;

        [Tooltip("Store the result in a float variable.")]
        public FsmFloat store;

        private OVRInput.Controller controllerInput;

        public enum axisTriggerType
        {
            PrimaryIndexTrigger,
            SecondaryIndexTrigger,
        };

        public override void Reset()
        {
            multiplier = 1f;
            store = null;
        }
        public override void OnEnter()
        {
            controllerInput = OVRInput.Controller.Gamepad;
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
                case axisTriggerType.SecondaryIndexTrigger:
                    axisValue = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, controllerInput);

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