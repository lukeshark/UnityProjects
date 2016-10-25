#PlayMaker PUN Change Log

###1.75
**Release**  
- 06/08/2016  

**Update**  
- Updated to PUN 1.75  

**Fix**  
- Pun api changes for `photonMessageInfo`

**New**   
- Action `PhotonNetworkGetRoomListProperties`  
- Action `PhotonNetworkGetPlayersProperties`  
- Action `PhotonViewRequestOwnerShip`  

###1.74
**Release**  
- 11/08/2016  

**Update**  
- Updated to PUN 1.74  

**Fix**  
- Pun obsolete properties fixed

**New**  
- PlayerTtl room option exposed


###1.71
**Release**  
- 22/06/2016  

**Update**  
- Updated to PUN 1.71  

**New** 
- Friend Support  
- event `PHOTON / FRIEND LIST UPDATED`  
- Action `PhotonNetworkFindFriends`  
- Action `PhotonNetworkGetFriendsOnlineStatus`  
- Action `PhotonNetworkGetFriendsinRoomStatus`  
- Action `PhotonNetworkGetFriendsRoomStatus`  

###1.69
**Release**  
- 24/05/2016  

**Update**  
- Updated to PUN 1.69  

###1.68
**Release**  
- 10/05/2016  

**Update**  
- Updated to PUN 1.68  

###1.67.1
**Release**  
- 06/05/2016  

**Fix**  
- missing event `PHOTON / MAX CCCU REACHED`  


###1.67
**Release**  
- 26/04/2016  

**Update**  
- Updated to PUN 1.65  
- Updated PlayMaker Utils
 
**Fix**  
- Pun+ warning in 5 is now removed  

###1.64.2
**Release**  
- 13/01/2016   

**Update**  
- Updated to PUN 1.65  
- Updated PlayMaker Utils
**New**  
- `PhotonNetworkGetSendRate`  
- `PhotonNetworkGetSendRateOnSerialize`  
- `PhotonNetworkSetSendRate`  
- `PhotonNetworkSetSendRateOnSerialize`

###1.64.2
**Release**  
- 18/11/2015  

**Update**  
- Updated to PUN 1.64.2  

###1.62
**Release**  
- 15/10/2015  
**Update**  
- Updated to PUN 1.62   

###1.61
**Release**  
- 24/09/2015  
**Update**  
- Updated to PUN 1.61  

**Fix**  
- matchmakingMode enum to use the right directive

###1.60
**Release**  
- 31/08/2015  
**Update**  
- Updated to PUN 1.60

###1.57
**Update**  
- Updated to PUN 1.57

**Fix**  
- Replaced [RPC] with [PunRPC] 

###1.55
**New**  
- Updated to PUN 1.55  
- Implemented PlayMakerUtils to prevent code redundancy

**Fix**  
- Fixed Wizard to accomodate api changes  


###1.5.1
**New**  
- updated to PUN 1.5.1  
- Take in account multiple observer for photon networkview

**Improvement**  
- Distribution via Ecosystem  
- Git submodule support for PUN  
- Demo made compliant with Ecosystem  

###1.28.3 ()
**New**  
- updated to PUN 1.28.3  
- added action PhotonViewGetIsMasterClient  

**Fix**  
- CheckPunPlus() override warning  
- OnPhotonInstantiate() composition, sometimes info.sender is null ( when opening a scene with pre existing scene instances for example)  

**Improvement**  
- improved action PhotonNetworkInstantiateSceneObject to only work if instance is the MasterClient.  

###1.25.2 ()
**New**  
- updated random join room action to match new signature ( typedLobby ignored for now)  
- updated createRoom to match new signature  
- updated createRoomAdvanced to match new signature  
- update OnPhotonPlayerPropertiesChanged which now has an object[] as parameter  

###1.24 ()
**New**  
- updated to PUN v1.24  
- updated PhotonNetworkingJoinRoom to include 'CreateIfNotExists' Flag  
- added PhotonViewGetIsSceneView action  
- added Authentication failure debug message and flag to PlayMakerPhotonProxy  
- added authentication failue property reset on PhotonNetworkConnectUsingSettings and PhotonNetworkConnectManually actions  
- added custom authentifaction access and message callback ( needs more testing)  

**Fix**  
- Fixed unsupported best server connection flag in various platforms in PhotonNetworkConnectUsingSettings  
- removed ArrayList depedencies for windows metro compliance, uses List<> instead.  
- spread usage of exitgames hashtable type version.  

###1.23 ()
**New**  
- updated to PUN v1.23  
- added connectToBestServer option in action   "connectUsingSettings"  
- added pun+ check for mobile pro requirement warning  


**Fix**  
- fixed Color streaming  


###1.21 ()
**New**  
- updated to PUN v1.21  
- added vector2, rect, color and quaternion variable   synch ability  
- added an advanced room creation to set custom   propreties and make them listed in the lobby  
- new action to Destroy gameobjects and players  
- new action to iterate through each rooms properties  
- new action to get the last cause of disconnection  

**Fix**  
- fixed bool synch  
- fixed custom player property action  