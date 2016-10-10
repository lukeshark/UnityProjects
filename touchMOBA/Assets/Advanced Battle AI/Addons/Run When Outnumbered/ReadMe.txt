++ ADD-ON FOR "INSTANT AI BOTS for PLAYMAKER" ++

ADD-ON NAME: RunWhenOutNumbered

ADD-ON AUTHOR: www.AIBotSystem.com

FUNCTIONALITY: Enables a bot to attempt to run away whenever it detects a high enemy-to-ally ratio nearby.  In other words, if a bot becomes surrounded by enemy units and notices that # enemies outnumber allies, it will *attempt* to run away.

MIN REQUIREMENTS: 
	1) Unity Free Edition, 4.5+
	2) PlayMaker 1.7+ (not included)
	3) Instant AI Bots for PlayMaker (not included)





REQUIRES THE FOLLOWING FSMs AND VARIABLE NAMES (should be already setup in original package):

1) charSTAT_SightRange (FSM Awareness)
2) charSTAT_EnemyTag (FSM Awareness)
3) charSTAT_AwarenessUpdateTime (FSM Awareness)
4) intel_EnemiesNearby (FSM Awareness)
5) intel_AlliesNearby (FSM Awareness)
6) FSM Movement
7) TACTICS_CurrentCommand (FSM Tactics)
8) TACTICS_PatrolChase (FSM Tactics)
9) runningAway (FSM HealthAndDamage)

Basically, if you purchased the Instant AI Bots package and did not change any FSM variable names, then you should be all set!


INSTALLATION:  Simply drop this entire folder into the "INSTANT AI BOTS for PLAYMAKER / Addons" folder


HOW TO USE:  Add the .DLL in this folder as a component to the Bot gameobject.  Must be on the same parent object level as all the FSMs.  Then set the Enemy Ratio.  For example, Enemy Ratio = 4 means the bot will run away when enemies outnumber allies by 4 to 1.

This add-on auto updates in the background, based on an interval. That interval is set using the bot's charSTAT_AwarenessUpdateTime (FSM Awareness).

Thank you for purchasing! Hope you enjoy.