#pragma strict

var mixerController		: mixerControl;				// Controller Reference
var mixer				: AudioMixer;				// Mixer we're using
var slider				: UI.Slider;				// This slider

function Start () {
	mixer		= mixerController.mixer;
	slider		= GetComponent.<UI.Slider>();
}

function Update(){
	if (mixerController.selectedTrack != name)			// Only call this if we aren't currently clicking on this slider
	{
		var currentValue	: float;					// Varibale to hold value
		if (mixer.GetFloat(name, currentValue))			// If there is a value for name...
			slider.value	= currentValue;				// Set the value of the slider to currentValue
	}
}