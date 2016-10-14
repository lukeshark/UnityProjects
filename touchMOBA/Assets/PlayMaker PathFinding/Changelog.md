#PathFinding Change log

###1.0.2
**New:** `SetAgentBaseOffset`   
**Fix:** `NavMeshFindClosestEdge` custom editor mask code
 

###1.0.1

**Fix:** Fixed *SetAgentDestinationAsGameObject.cs* to refresh onEnter and force update of destination when coming back to the state. 

###1.0.0
**Note:**	Initial Public Release  

**New:** Github Repository as a [SubModule](https://github.com/jeanfabre/PlayMaker--Unity--PathFinding_U5-SubModule-), and as a regular dedicated [Unity Project Repository](https://github.com/jeanfabre/PlayMaker--Unity--PathFinding_U5) for packaging ( on [the U5 Ecosystem Repository](https://github.com/PlayMakerEcosystem/PlayMakerCustomActions_U5)) and samples  
**New:** Refactored all area related actions from *Layer* to *Area* ( *layer* within PathFinding is now obsolete, *Area* is the very same, but just a different name). See [Unity Doc on Areas](http://docs.unity3d.com/Manual/nav-AreasAndCosts.html)
