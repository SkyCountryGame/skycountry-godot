this is the sky country unity game converted to a godot game. 

* development plan:
here are some notes to organize my thoughts on the current architecture, how to approach development, and the current steps. 

TODO:
	- automatically set CollisionShape transform from parent Body3D on load
	- currently collision logic is handled in Player, because it extends Collideable. decide if this is the best implementation. will there ever be a case where it better to have logic in the object, inanimate or not?
	- traversing up node tree to find associated node, for maps. did i already deal with this? 
	- Interactor.HandleInteract() should take a GameObject instead of Node probably 
	
	
- FOLDER STRUCTURE:
	- currently it's a little messy, as i'm leaving almost all files in the root directory, because i don't want to prematurely confine us to any organizational system yet.
	- some of the classes aren't even used. we will clean things up as the system evolves
	
- currently i'm in the process of setting up a way to connect "sky country game objects" to "godot scene nodes", so that in the game logic code, we only have to deal with skycountry game concepts. look at GameObject, GameObjectConnector, ResourceManager.RegisterGameObject(). it's still a WIP, and will probably evolve quite a bit. 

- next i'm going to implement level switching, clean up some of the files, and implement lamp light toggling 

FILE CLEANUP and ORGANIZE:
	- categories of scenes (*.tscn)
		- things that spawn into the world upon certain events, like bullets or a specific NPC
		- levels. a relationship between nodes that comprise a playable world of game
	- categories of code (*.cs)
		- 
	- what do with Controllers. 
		
		
EXPLANATION of SYSTEMS:
	- game object registration. ResourceManager.RegisterGameObject()
-----