character description json file format: this is a representation of all the serializable data of a character that can be loaded and saved 

need to figure out what stuff is stored in the scene file and what properties should be exported to be editable in editor. 

* all the general properties of NPCs: hp, inventory, armor,  
* some of these properties can actually just be stored in the character ".tscn" file. need to figure out what should be exposed to editor and to if should use godot Resource in addition to or instead of the json. 
* portrait: the avatar portrait that's displayed ingame for the characters. might not be necessary because that can be included in the character scene file. 
* inventory:
* location: this is updated with the character's current location. 
* 