using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Sets the Rate and Magnitude of Vibration for more precise control.")]
    public class GetTouchControllerVibration : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Rate of Vibration.")]
        [HasFloatSlider(0, 1)]
        public FsmFloat Frequency;

        [Tooltip("Magnitude of Vibration.")]
        [HasFloatSlider(0, 1)]
        public FsmFloat Amplitude;

        [Tooltip("Duration of Vibration. Set to -1 for infinite duration.")]
        public FsmFloat duration;

        public FsmEvent finishEvent;

        private OVRInput.Controller controllerInput;
        private float timer;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        }

        public override void Reset()
        {
            Amplitude = null;
            Frequency = null;
            duration = 1f;
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
            OVRInput.SetControllerVibration(Frequency.Value, Amplitude.Value, (controllerInput));
            if (duration.Value == 0)
            {
                OVRInput.SetControllerVibration(0, 0, (controllerInput));
                Fsm.Event(finishEvent);
                return;
            }

            timer = 0f;
        }
        public override void OnUpdate()
        {

            OVRInput.SetControllerVibration(Frequency.Value, Amplitude.Value, (controllerInput));
            timer += Time.deltaTime;

            if(duration.Value == -1)
            {
            }
            else
            {
                if (timer >= duration.Value)
                {
                    OVRInput.SetControllerVibration(0, 0, (controllerInput));
                    if (finishEvent != null)
                    {
                        Fsm.Event(finishEvent);
                    }
                }
            }
        }
        public override void OnExit()
        {
            OVRInput.SetControllerVibration(0, 0, (controllerInput));
        }
    }
}
