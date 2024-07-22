this is the sky country unity game converted to a godot game. 

* development plan:
here are some notes to organize my thoughts on the current architecture, how to approach development, and the current steps. 

TODO:
	- automatically set CollisionShape transform from parent Body3D on load
	- traversing up node tree to find associated node, for maps. did i already deal with this? 
	- Interactor.HandleInteract() should take a GameObject instead of Node probably, if we are going to end up using the GameObject idea. otherwise get ride of the concept of GameObject and don't set them in Global class.  
	
- FOLDER STRUCTURE:
	- currently it's a little messy, as i'm leaving almost all files in the root directory, because i don't want to prematurely confine us to any organizational system yet.
	- some of the classes aren't even used. we will clean things up as the system evolves
	
- currently i'm in the process of setting up a way to connect "sky country game objects" to "godot scene nodes", so that in the game logic code, we only have to deal with skycountry game concepts. look at GameObject, GameObjectConnector, Global.RegisterGameObject(). it's still a WIP, and will probably evolve quite a bit. 
	- note 20240722: not worrying about this too much right now. just moving forward with the game. if this system turns out to be useful, it will evolve, otherwise remove it. 

FILE CLEANUP and ORGANIZE:
	- categories of scenes (*.tscn)
		- things that spawn into the world upon certain events, like bullets or a specific NPC
		- levels. a relationship between nodes that comprise a playable world of game
	- categories of code (*.cs)
		- 
	- what do with Controllers. 
	- adding audio in ./assets/aud/
	- dialogue in ./assets/dialogue
		
		
EXPLANATION of SYSTEMS:
	- game object registration. Global.RegisterGameObject()
	- Player node and PlayerModel
		- the purpose of the PlayerModel class is to persist player data between scenes. the Player Node can be added as a separate thing to each scene, because it proved too convoluted to swap over the same Node into a newly instantiated and switched-to scene. now when a Player Node is loaded into a scene, it just grabs the global PlayerModel if there is one, otherwise it will attempt to load save data (not yet implemented), otherwise it will create new one. 
-----
