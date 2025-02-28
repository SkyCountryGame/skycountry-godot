## Components. 
### the purpose of this doc is help conceptualize components, and propose how they should be implemented. should they all be Nodes, Resources, or some of each? what values do they need to export?  

## how components work:
    - each component listens for state changes on the parent node to which it is attached, which must be a stateholder
    - it has some number of nodes (exported) upon which it acts, based on state change
    - 

StateHolder

Talker
    - allows the node to be talked to by the player, initiated via interaction by player
    - contains a set of dialogues, where which one to use in a scenario is chosen by some conditions 
    - conditions for dialogue selection are specified as a set of "quest flags", indicating the player's progression through the game, or what has happened in the story

Patroller
    - allows the node to "patrol", travel between a set of points in the world, pausing at each for some specified interval (can be random within some range)
    - contains a set of Vector3, to indicate the positions
    - avoids obstacles by walking around

Follower
    - enables the node to follow some other node in the world

QuestGiver
    - allows the node to initiate quests, via interaction by player, 
    - contains a list of quests, probably as a Queue
    - will give the first one, if conditions are met (quest flags), else will tell player what quests need to be completed
    - this will often be used in conjuction with Talker

Seeker
    - enables the node to search for some other node in the world, and then follow it
    - has a radius of awareness, or just use a collisionzone


