public enum InteractionType
{ //more than anything, this specifies the datatype of the payload
	Dialogue, //payload = Dialogue (start talking to a character and see dialogue UI)
	Inventory, //payload = Inventory (opening a chest to view inventory transfer UI)
	Pickup, //payload = InventoryItem,  (picking up a box of dandy boy apples)
	General, //? TODO remove if not use
	Function //payload = Function (? remove if not use)
}

public enum InteractionMethod
{
	Contact, //walk onto object
	Select, //click mouse or "selection" action
	Click, //left click only for now 
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
	bool IsInteractionValid(Interactor interactor); //TODO remove if not use
}
