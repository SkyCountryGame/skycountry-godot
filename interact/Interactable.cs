using System;
using System.Collections;
using System.Collections.Generic;

public enum InteractionType
{ //more than anything, this specifies the datatype of the payload
	Dialogue, 
	Inventory, 
	Pickup, //for now will always give InventoryItem,  
	General, 
	Mineable,
	Function //give a function to execute with player as parameter
}

public enum InteractionMethod
{
	Contact, //walk onto object
	Select, //click mouse or "selection" action
	Use //execute main "use" action, may require some prerequisite item to be equipped, within some distance, etc.
}

// -- ALL IMPLEMENTING CLASSES SHOULD SPECIFY THE PAYLOAD DATATYPE IN A COMMENT --
public interface Interactable
{
	InteractionType interactionType { get; }
	InteractionMethod interactionMethod {  get; } //TODO perhaps this should be a bitfield, because multiple interaction methods

	//bool delayed { get; }
	
	dynamic Interact(); //called when interactor "interacts" with this. and this returns the payload

	void Retain(); //leave the thing as is (e.g. don't pick up item)
	void Clear(); //this interaction has been used (e.g. item is picked up; chest inv is modified; NPC social state has changed)

	int GetHashCode();

	string Info();

	//can an interactor interact with this interactable? 
	bool IsInteractionValid(Interactor interactor);
}
