using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Sends an Event when a button is Nearly touched.")]
    public class GetTouchNearTouchpad : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        [Tooltip("Select the button to be pressed.")]
        public setButton button;

        [Tooltip("Select how the button type should send the event.")]
        public setButtonType buttonType;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        };

        public enum setButton
        {
            PrimaryIndexTrigger,
            PrimaryThumbButtons,
        };

        public enum setButtonType
        {
            getButton,
            getButtonDown,
            getButtonUp,
        };


        [Tooltip("Event to send if the button is pressed.")]
        public FsmEvent sendEvent;

        [Tooltip("Set to True if the button is pressed.")]
        [UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        private OVRInput.Controller controllerInput;

        public override void Reset()
        {

          
            sendEvent = null;
            storeResult = null;
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
            switch (button)
            {
                case setButton.PrimaryIndexTrigger:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            else
                            {
                                storeResult.Value = false;
                            }
                            break;
                        case setButtonType.getButtonDown:
                            if (OVRInput.GetDown(OVRInput.NearTouch.PrimaryIndexTrigger, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.NearTouch.PrimaryIndexTrigger, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
                case setButton.PrimaryThumbButtons:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            else
                            {
                                storeResult.Value = false;
                            }
                            break;
                        case setButtonType.getButtonDown:
                            if (OVRInput.GetDown(OVRInput.NearTouch.PrimaryThumbButtons, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.NearTouch.PrimaryThumbButtons, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
