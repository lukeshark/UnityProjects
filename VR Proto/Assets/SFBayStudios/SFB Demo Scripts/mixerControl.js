#pragma strict

import UnityEngine.Audio;							// We wil be using this in the script

var mixer			: AudioMixer;					// The mixer object, which is not in the project itself, just the heirarchy
var selectedTrack	: String			= "Master";	// Name of the currently selected track.
var snapshots		: AudioMixerSnapshot[];			// Array of snapshots.
var newTrack		: boolean			= false;	// A check to make sure we don't set volume on the same frame a new track was selected

// These variables are old and will be removed in an update.
var mixerUnpaused	: AudioMixerSnapshot;			// OLD: A snapshot to be used when we are not paused
var mixerPaused		: AudioMixerSnapshot;			// OLD: A snapshot to be used when we are paused

private var snapshotTransitionSpeed	: float	= 0.2;	// How long will the snapshot transition last?

// If a user were to click on a new track in the UI and move the slider on the same frame, the previous track would be updated instead
// of the one they're currently sliding.  The newTrack boolean keeps this from occuring.

function Update(){
	if (newTrack)									// If we switched tracks last frame...				
		newTrack	= false;						// Set newTrack to false so we can SetVolume() again.
}

// Sets the selectedTrack variable
function SetSelectedTrack(value : String){
	selectedTrack	= value;						// Set selectedTrack to value
	newTrack		= true;							// Set boolean so that SetVolume() won't run this frame
}

// Sets the volume of a selectedTrack
function SetVolume(value : float){
	if (selectedTrack != "None" && !newTrack)		// Make sure we didn't just switch tracks & there is one selected
		mixer.SetFloat(selectedTrack, value);		// Set the volume of selectedTrack to value
}

// Use this function to switch to a snapshot from the shapshot[] array.
function SwitchToSnapshot(id : int){
	Debug.Log("Transition to Snapshot ID#" + id + " (Speed: " + snapshotTransitionSpeed + ")");
	snapshots[id].TransitionTo(snapshotTransitionSpeed);
}

// This function is old and will be removed in an update
function TransitionSnapshot(isPaused : boolean){
	if (isPaused)														// If we are now paused
		mixerPaused.TransitionTo(snapshotTransitionSpeed);				// Transition to paused snapshot
	else if (!isPaused)													// If we are not paused
		mixerUnpaused.TransitionTo(snapshotTransitionSpeed);			// Transition to unpaused snapshot
}

// Reset everything
function ResetValue(track : String){
	mixer.ClearFloat(track);						// Reset the track volume to its initial value
}