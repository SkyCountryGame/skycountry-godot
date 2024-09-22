every inventory item also has a node which is what the thing looks like as an in-game object. 
therefore we have 2 classes: InventoryItemNode (extends Node) and InventoryItemProperties (extends Resource).

this pattern can also be observed with: (not using exact class names as they currently exist)
PlayerNode -> PlayerModel
CharacterNode -> CharacterModel  (for NPCs and enemies)
GameObjectNode -> GameObjectProperties
InventoryItemNode -> InventoryItemProperties
(what else?)

it is basically the Controller and Model components of MVC architecture. (where node is the controller)

the node is what handles in game happenings like collisions etc., and it modifies the properties of its own model. the model can be saved to disk and loaded from disk, since it is a Godot.Resource. 

Let's look at an example case, the pickaxe. the pickaxe is a tool used by the player for mining, most probably mining rocks and minerals. 
if the pickaxe is just sitting on the ground, it's just a normal game object, with a mesh and a collisionzone. at this point it doesn't even have to have an internal model representing its properties relating to mining (durability, mass, sharpness).
it is however also an interactable, because the player needs to be able to pick it up, after he has gotten close enough to it such that the pickaxe is in the player's pickupable-range collisionzone. 

#possibly a picture/diagram of the objects currently (PickaxeGameObject, PlayerNode, PlayerModel)

so the player picks it up, which means that HandleInteract() has been called after the user pressed the pickup button. 
in this method, we get the payload from the interactable, then continue on to perform the correct logic which depends on the InteractionType of the interactable, in this case Pickup. the payload of a pickup should be an inventory item. at this point, we should have removed the PickaxeNode from the game world (and played the player-pickup animation etc.). 
we don't even need the Node of the inventory item, because it's just in the player's inventory, however we'll need it eventually if the item ever gets dropped. at this point, we only know what kind of inventory item it is based on the ItemType enum of InventoryItemProperties. This enum is what will dictate what logic to perform when the player attempts to "use" the item. If it's aid, for example, like a health potion or can of soda, then it's just consumed (removed from inv) and some properties of the PlayerModel are changed. if it's apparel, then the in-game PlayerNode changes such that it looks like the player is wearing the item. the Mesh that the player is wearing could very well be different from the Mesh that represents that apparel item when it's just sitting on the ground. if the item is a weapon or a shield, then some new Node3D with a Mesh needs to be attached to the player's hand. maybe some item types can't be "used", like junk and minerals. some might bring up a GUI pane, like reading a note. 

so let's say we've equipped a pickaxe, so we have a new RigidBody3D (or StaticBody3D?) attached to the player's hand which has an Area3D as a child for the hitbox. the player knows to play the swing animation upon the correct action input because somewhere there exists a map which associates specific items to player actions, player states, and animations as necessary. 
might look something like:
    pickaxe -> [(IdleState, IdlePickAnimation), (SwingingState, SwingingPickAnimation)]
    sword -> [(IdleState, IdleSwordAnimation), (SlashingState, SlashingAnimation), (StabbingState, StabbingAnimation), (BlockingSwordState, BlockingSwordAnimation), etc.]
    waterbucket -> [(IdleState, IdleBucketAnimation), (WateringState, WateringAnimation)]
    hookshot -> [(IdleState, IdleHookshotAnimation), (AimingState, AimingHookshotAnimation), (ShootingHookshotState, ShootingHookshotAnimation), (HookshotPullingState, HookshotPullingAnimation)]

where this relationship shall exist in memory (which object/s should hold it) i do not yet know. the important thing is that when the player equips an item or uses an item, he needs to know the following: which activityState to enter, what animation to enter, if need to instantiate a child node on his hand, if need to return to other state or when to return to another state. 
it looks like "IdleState" is just the player's default state, and most of the toolaction states are just player's attacking or using state.   
i'm going to guess that the idle state when holding most items will be the same. probably just one-handed vs two-handed. some items will probably have to have some rotation offset when held by player. 
some of these situations will have both the player and the equipped item animating, so multiple animations need to be triggered by the item use. 

the constructor for an InventoryItem should be able to get all the information that it needs from its InventoryItemProperties resource, so we don't need to pass any parameters other than the filepath of that resource. 

so now when the player is in the vicinity of a mineable interactable and he attempts to use the pickaxe, after verifying that mining is possible, we know what animation play and we can get the payload from Rock::Interact(). 

----