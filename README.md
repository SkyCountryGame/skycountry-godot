this is the sky country unity game converted to a godot game. 

* development plan:
here are some notes to organize my thoughts on the current architecture, how to approach development, and the current steps. 

- FOLDER STRUCTURE:
	- currently it's a little messy, as i'm leaving almost all files in the root directory, because i don't want to prematurely confine us to any organizational system yet.
	- some of the classes aren't even used. we will clean things up as the system evolves
	
- currently i'm in the process of setting up a way to connect "sky country game objects" to "godot scene nodes", so that in the game logic code, we only have to deal with skycountry game concepts. look at GameObject, GameObjectConnector, ResourceManager.RegisterGameObject(). it's still a WIP, and will probably evolve quite a bit. 

- next i'm going to implement level switching, clean up some of the files, and implement lamp light toggling 

-----