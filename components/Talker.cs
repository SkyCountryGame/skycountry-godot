using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using State = StateManager.State;

//a thing that talks (has dialogue)
[GlobalClass]
public partial class Talker : Component, Interactable, EventListener {
	public InteractionType interactionType { get => InteractionType.Dialogue; }
	public InteractionMethod interactionMethod { get => InteractionMethod.Use; }

    public HashSet<EventType> eventTypes => new HashSet<EventType>(){EventType.Function}; 

    //[Export(PropertyHint.None, "dialogue")]
    [Export(PropertyHint.File, "dialogue-for-dude.json")]
	public String dialogueFilename = "assets/dialogue/0.json";

	public Dialogue dialogue;
	public List<Dialogue> dialogues; //each character has his own set of dialogues
    
	public override void _Ready(){
		EventManager.RegisterListener(this);
        //Global.RegisterGameObject(this, GameObjectType.Interactable);
        //dialogue = ResourceLoader.Load<Dialogue>(dialogueFilename);
        dialogue = new Dialogue(dialogueFilename);
    }
	

	//start dialogue when player interacts
	public dynamic Interact()
	{
		if (dialogue != null){
			((StateManager)subject).SetState(State.TALKING);
			return dialogue.Next();
		}
		return null;
	}

	//get the appropriate dialogue based on the state of things
	public Dialogue GetDialogue(){
		dialogue.title = "Talking to " + Name; //TODO how do dialogue title? is it ever dynamic depending on game state, or can be defined in json? 
		return dialogue; //TODO logic to get the correct dialogue
	}

	public void Retain()
	{
		//wat do?
	}

	public string Info()
	{
		return "A talking thing. ";
	}

	public bool IsInteractionValid(GameObject interactor)
	{
		return true;
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public bool IsInteractionValid(Interactor interactor)
	{
		throw new NotImplementedException();
	}

    public bool HandleEvent(Event e)
    {
        switch (e.eventType){
			case EventType.Function:
				string functionName = e.payload.Item1;
				List<Object> args = e.payload.Item2;
				Variant[] convertedArgs = new Variant[args.Count];
				for(int i = 0; i<args.Count; i++){
					if(Int32.TryParse((String)args[i], out int j)){
						convertedArgs[i]=j;
					}
					else {
						convertedArgs[i]=(String)args[i];
					}
				}
				Variant result = Call(functionName,convertedArgs);
				switch(result.VariantType){
					case Variant.Type.Bool:
						return (bool)result;
					default:
						return true;
				}
			default:
				break;
		}
		return true;
    }

	public bool CheckAndRemoveInventory(String itemName, int count){
		Dictionary<string,int> detailedItemList = Global.playerModel.inv.GetDetailedItemList();
		if(detailedItemList.ContainsKey(itemName) && detailedItemList[itemName]>=count){
			for(int i = 0; i<count; i++){
				bool temp = Global.playerModel.inv.RemoveItemByName(itemName);
				if(!temp){
					GD.PrintErr("uh oh something fucked up here alert alert");
					return false;
				} 
			}
			return true;
		}
		return false;	
	}

}
