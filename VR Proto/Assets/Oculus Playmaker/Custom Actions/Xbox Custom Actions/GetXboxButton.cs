using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Oculus")]
    [Tooltip("Sends an Event when an Xbox Button is pressed.")]
    public class GetXboxButton : FsmStateAction
    {


        //public OVRInput.Controller controller;

        [Tooltip("Select touch or Press for trigger type.")]
        public setButton button;

        [Tooltip("Select touch or Press for trigger type.")]
        public setButtonType buttonType;


        public enum setButton
        {
            One,
            Two,
            Three,
            Four,
            Start,
            Back,
            PrimaryThumbstick,
            SecondaryThumbstick,
            PrimaryShoulder,
            SecondaryShoulder,
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
            var controllerInput = OVRInput.Controller.Gamepad;

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
                case setButton.Three:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.Three, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.Three, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.Three, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.Four:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.Four, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.Four, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.Four, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.Start:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.Start, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.Start, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.Start, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.Back:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.Back, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.Back, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.Back, controllerInput))
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
                            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.SecondaryThumbstick:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.PrimaryShoulder:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.PrimaryShoulder, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.PrimaryShoulder, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.PrimaryShoulder, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                    }

                    break;
                case setButton.SecondaryShoulder:
                    switch (buttonType)
                    {
                        case setButtonType.getButton:
                            if (OVRInput.Get(OVRInput.Button.SecondaryShoulder, controllerInput))
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
                            if (OVRInput.GetDown(OVRInput.Button.SecondaryShoulder, controllerInput))
                            {
                                storeResult.Value = true;
                                Fsm.Event(sendEvent);
                            }
                            break;
                        case setButtonType.getButtonUp:
                            if (OVRInput.GetUp(OVRInput.Button.SecondaryShoulder, controllerInput))
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
