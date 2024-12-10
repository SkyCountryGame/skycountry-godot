thoughts during demo development:
    - do we want physics bodies of NPCs to be CharacterBody or RigidBody?
    - all NPCs should have physics body, correct? or might there be a shopkeeper that you can never physically reach?
    - are there ever any "stateless" nodes that have components?
    - StateManager 
    - i think usable is different from equippable
    - register gameobjects with all of the nodes that comprise the object
    - do gameobjects need UUIDs?
    - how to let talker know that dialogue is finished. some possible approaches: 
        1: player holders reference to current interaction so he can notify it when complete. 
        2: broadcast dialogue started/ended event. list all the things that need to happen when dialogue starts/ends. 
    - need to add option to motionmodule for natural walking