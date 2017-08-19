using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Sends an Event when a Rift Remote Button is pressed.")]
    public class GetRiftRemoteButton : FsmStateAction
    {

        [Tooltip("Select the button to be pressed.")]
        public setButton button;

        [Tooltip("Select how the button type should send the event.")]
        public setButtonType buttonType;
        
        public enum setButton
        {
            One,
            Two,
            DpadUp,
            DpadDown,
            DpadLeft,
            DpadRight,
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

        }

        public override void OnUpdate()
        {
            var controllerInput = OVRInput.Controller.Remote;

            switch (button)
            {
                

                case setButton.One:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.One, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.One, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.One, controllerInput))
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
                            if (OVRInput.Get(OVRInput.Button.Two, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.Two, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.Two, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    
                    break;
                case setButton.DpadUp:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.DpadUp, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.DpadUp, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.DpadUp, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    
                    break;
                case setButton.DpadDown:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.DpadDown, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.DpadDown, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.DpadDown, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    
                    break;
                case setButton.DpadLeft:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.DpadLeft, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.DpadLeft, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.DpadLeft, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }
                    
                    break;
                case setButton.DpadRight:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.DpadRight, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.DpadRight, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.DpadRight, controllerInput))
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
