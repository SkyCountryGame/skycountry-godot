## Global
- an autoload class for the entire game
- holds references to objects that are frequently used throughout the codebase, such as current active level, player Node, the HUD. 
- also holds datastructures to associate between GameObjects and Nodes and Interactables. 

## Camera

there is a main camera that follows the player (though it can be set to follow any Node3D in the scene). 
there is also a script for alternate cameras for special scenarios, such as showing specific things in a level. these can either stay in one location. 

#### how to know when to switch to what camera?

each camera has a condition defining when it should be the one that the player is looking for. 

## Levels

each level should use the Level script (or a script that extends Level). 

## Dialogue
anything that initiates dialoge should have a Talker componenet Node as a child. the Talker has a json file to specify its dialogue. see [dialogue-json-format.md](./dialogue-json-format.md) for info.  

## Quests


## GameObject
