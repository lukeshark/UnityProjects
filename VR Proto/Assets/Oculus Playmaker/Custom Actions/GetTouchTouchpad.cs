using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Sends an Event when a Button is touched.")]
    public class GetTouchTouchpad : FsmStateAction
    {
        [Tooltip("Specify Controller Type.")]
        public controllerEnum touchController;

        //public OVRInput.Controller controller;

        [Tooltip("Select touch or Press for trigger type.")]
        public setButton button;

        [Tooltip("Select touch or Press for trigger type.")]
        public setButtonType buttonType;

        public enum controllerEnum
        {
            LeftController,
            RightController,
        };

        public enum setButton
        {
            One,
            Two,
            PrimaryThumbRest,
            PrimaryIndexTrigger,
            PrimaryThumbstick,
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
                case setButton.One:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Touch.One, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Touch.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Touch.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
                case setButton.Two:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Touch.Two, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Touch.Two, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Touch.Two, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
                case setButton.PrimaryThumbRest:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Touch.PrimaryThumbRest, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Touch.PrimaryThumbRest, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
                case setButton.PrimaryIndexTrigger:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Touch.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Touch.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    break;
                case setButton.PrimaryThumbstick:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Touch.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Touch.One, controllerInput))
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
