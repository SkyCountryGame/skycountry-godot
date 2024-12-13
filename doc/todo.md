todo:
* initial combat implementation
* saving/loading games
* getting movement controls to feel good
* dynamically spawn items in certain areas on the floor
* architecture for sending data between gameplay and HUD
* triplanar shader? 
* http audio stream
* camera updates
* work out gameobject system/abstractions
* Player has lots of updates for using AnimController and StateHolder
* improve HUD
* explore features of NavAgent, NavMesh, CharacterBody. 
    * later might want to make nav baking more efficient by specifying which nodes to scan instead of doing all recursively
* NPC todo
    * add option to motionmodule for natural walking (don't ocsillate around destination)
    * actual nav target position is some random point within radius of target position
    * enemy goes to player, moves towards, updates target pos every 5 sec
* make GameObject system testable 
* make Equipable a Component
* file organization: prefabs folder. Pickup etc. out of components/. 