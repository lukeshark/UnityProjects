#pragma strict

/* SCRIPT INFO
This script is intended to be used on objects that have a SkinnedMeshRenderer attached, or objects
that have children who have SkinnedMeshRenderers attached.  It will save a new mesh version of those
objects, with a normal Mesh Renderer.  If used on an environment piece that has Blend Shapes, the 
Blend Shapes will be saved (although no longer editable).  If used on a character (often the children
of the main object will be separate meshes), each individual mesh will be saved as a new mesh in the
current form it is in.  That means you can save a pose from an animation as well as adjustments made
to the Blend Shape values.

The script will create an organized file list, and also replace the in-scene versions with the new
"Statue" versions.  This way, an entire scene filled with environment pieces that have
SkinnedMeshRenderers attached can be saved as static meshes and replaced in a single click.

For characters, it's best to make a copy of your character before you run the script, as the character
will be replaced.

Names that have duplicates in the file structure will have a number added to the end of them, so be
aware of the potential for duplicate meshes, although you won't have to worry about overwriting
already exported meshes.

This script is intended for editor use only.  It may work during runtime (perhaps with some
modification) but will likely not be very fast.

Please visit http://www.InfinityPBR.com to check out everything we produce.
*/

// This puts an option in the menu bar to run the script.
@MenuItem ("Window/SFBayStudios/Save As Mesh(es)")
static function Init () {
	// Make sure we have all the needed folders
	SFB_CreateFolder("Assets", "SFBayStudios");
	SFB_CreateFolder("Assets/SFBayStudios", "Exported Meshes");

	// Run through each object that is currently selected
	for (var i : int; i < Selection.objects.Length; i++){
		var currentPercent : float	= (i + 1.0) / (Selection.objects.Length * 1.0);											// Percent complete
		// Display a progress bar with detailed info
		EditorUtility.DisplayProgressBar("Creating Meshes", "(" + (i + 1) + " of " + Selection.objects.Length + ") Working on " + Selection.objects[i].name, currentPercent);
		var thisObject		: GameObject	= Selection.objects[i];									// Variable for this object
		var objectsSMR		: GameObject[];															// Array for all objects with a SkinnedMeshRenderer on it
		objectsSMR	= SFB_GetSMRObjects(thisObject);												// Populate with all objects that have SkinnedMeshRenderer attached 
		if (objectsSMR.Length != 0)																	// If we have objects to work with
		{
			var parentFolder	: String	= "Assets/SFBayStudios/Exported Meshes";				// Path to the folder that holds all of our exports
			var folderName		: String	= SFB_GetFolderName(parentFolder, thisObject.name);		// Get a new folder name for this object
			var path			: String	= parentFolder + "/" + folderName;						// Full path
			SFB_CreateFolder(parentFolder, folderName);												// Create the folder

			for (var o : int; o < objectsSMR.length; o++){											// For each object
				var obj						= objectsSMR[o];										// Assign the current object
				var thisRenderer			= obj.GetComponent.<Renderer>();						// Get the renderer component
				var thisMaterial			= thisRenderer.sharedMaterial;							// Save the material we're using
				var mesh					= SFB_BakeMesh(path, obj);								// Save the mesh
				DestroyImmediate(thisRenderer);														// Remove the SkinnedMeshRenderer
				var newMeshFilter : MeshFilter	= obj.AddComponent.<MeshFilter>() as MeshFilter;	// Add a normal mesh filter
				newMeshFilter.mesh				= mesh;												// Assign the mesh to the mesh filter
				var newRenderer	: MeshRenderer	= obj.AddComponent.<MeshRenderer>() as MeshRenderer;// Add a normal mesh renderer
				newRenderer.sharedMaterial		= thisMaterial;										// Put the material on the new mesh
				var thisSFBBlendShapes			= obj.GetComponent.<SFB_BlendShapes>();				// See if the object has a SFB_BlendShapes script attached
				if (thisSFBBlendShapes)																// If it does...
					DestroyImmediate(thisSFBBlendShapes);
				var animator					= obj.GetComponent.<Animator>();					// See if the object has an animator attached
				if (animator)																		// if it does...
					DestroyImmediate(animator);														// Remove the animator component
			}
			var animatorParent				= thisObject.GetComponent.<Animator>();					// See if the object has an animator attached
			if (animatorParent)																		// if it does...
				DestroyImmediate(animatorParent);
			SFB_SaveAsPrefab(path + "/" + folderName + ".prefab", thisObject);						// Save this as a prefab
		}
		EditorUtility.ClearProgressBar();															// Clear the progress bar
	}
}

// This will save the object as a prefab
static function SFB_SaveAsPrefab(path : String, obj : GameObject){
	PrefabUtility.CreatePrefab(path, obj);															// Creates a prefab of obj at path
}

// This function will create a new mesh asset of the selected SkinnedMeshRenderer and save it to the
// Project
static function SFB_BakeMesh(path : String, obj : GameObject) : Mesh {
	var mesh	: Mesh 			= new Mesh();														// Create a new mesh
	var skinnedMeshRenderer		= obj.GetComponent.<SkinnedMeshRenderer>();							// Get the component
	skinnedMeshRenderer.BakeMesh(mesh);																// Bake the current mesh geometry to the new mesh we created
	var filePath				= path + "/" + obj.name + ".asset";									// File path to saved mesh
	AssetDatabase.CreateAsset(mesh, filePath);														// Save the mesh to Project
	return mesh;																					// Return the mesh asset
}

// This will find all the objects that have the SkinnedMeshRenderer component attached, returning
// an array
static function SFB_GetSMRObjects(thisObject : GameObject) : Array {
	var returnArray = new Array ();																	// Create a new array
	var childrenSMR	: Component[] = thisObject.GetComponentsInChildren(SkinnedMeshRenderer, true);	// Find all SkinnedMeshRenderer components
	for (var childSMR : SkinnedMeshRenderer in childrenSMR)											// For all we found
	{
		returnArray.Push (childSMR.gameObject);														// Push the GameObject into the array
	}
	return returnArray;																				// Return the Array
}

// This function will create the correct path folders if they do not already exist.
static function SFB_CreateFolder(path : String, value : String){
	if (!AssetDatabase.IsValidFolder(path + "/" + value))											// If it isn't a folder already
		AssetDatabase.CreateFolder(path, value);													// Create the Folder
}

// This function will check the parent folder for a folder that already exists with the name we
// want to use.  If it does, it'll send back the next available name, with numbers after it.
static function SFB_GetFolderName(parentFolder : String, value : String) : String {
	if (AssetDatabase.IsValidFolder(parentFolder + "/" + value))									// If the name is a valid folder, it means we already have one
	{
		var x			: int		= 2;															// Lets start this at #2
		var foundName	: boolean	= false;														// We have not yet found the name
		while (!foundName){																			// do this while we haven't found the name
			if (!AssetDatabase.IsValidFolder(parentFolder + "/" + value + " " + x))					// If we don't have a folder by the name of "[value] [x]"...
			{
				return value + " " + x;																// return "[value] [x]"
			}
			x++;																					// Increase value of x by 1
		}
	}
	return value;																					// Return the value
}

// This will delete the Animator component, if there is one attached
static function SFB_RemoveAnimator(obj : GameObject){
	yield (0.1);																					// Wait 0.1 second
	var animatorComponent		= obj.GetComponent.<Animator>();									// Get component for Animator
	DestroyImmediate(animatorComponent);															// Remove the component
}

// This script will remove SFB_BlendShapes.js from the object, if it's attached.  It's used for
// anything using my blend shapes script that makes it easier to modify exposed shapes.  Specific
// to my own packages and assets, or anyone else who decides to adopt the same naming convention.
static function SFB_RemoveBlendShapesScript(obj : GameObject){
	var thisSFBBlendShapes		= obj.GetComponent.<SFB_BlendShapes>();								// Get component for SFB_BlendShapes if it's attached
	if (thisSFBBlendShapes)																			// If the script is attached
	{
		SFB_DeleteBlendShapesScripts(obj);															// Run this function
		Debug.Log("has the script");
		thisSFBBlendShapes.enabled	= false;														// Disable the script
	}
}

// This will remove the script, but delayed 1/10th of a second, to avoid errors with other scripts / unity stuff.
// It is only intended for objects that use SFBayStudios proprietary Blend Shape manager.
static function SFB_DeleteBlendShapesScripts(obj : GameObject){
	Debug.Log("SFB_RemoveScripts(" + obj.name + ")");
	yield (0.1);																					// Wait 0.1 second
	var thisSFBBlendShapes		= obj.GetComponent.<SFB_BlendShapes>();								// Get component for SFB_BlendShapes
	DestroyImmediate(thisSFBBlendShapes);															// Remove the script
}