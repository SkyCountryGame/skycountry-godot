using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

//a thing that talks (has dialogue)
public partial class Talker : Node3D, Interactable {
	public InteractionType interactionType { get => InteractionType.Dialogue; }
	public InteractionMethod interactionMethod { get => InteractionMethod.Use; }

	//[Export(PropertyHint.None, "dialogue")]
	[Export(PropertyHint.File, "dialogue-for-dude.json")]
	public String dialogueFilename;
	

	public Dialogue dialogue;
	public List<Dialogue> dialogues; //each character has his own set of dialogues
    
	public override void _Ready(){
        Global.RegisterGameObject(this, GameObjectType.Interactable);
        //dialogue = ResourceLoader.Load<Dialogue>(dialogueFilename);
        dialogue = new Dialogue(dialogueFilename);
    }
	

	//start dialogue when player interacts
	public dynamic Interact()
	{
		if (dialogue != null){
			return dialogue.Next();
		}
		return null;
	}

	//get the appropriate dialogue based on the state of things
	public Dialogue GetDialogue(){
		dialogue.title = "Talking to " + Name; //TODO how do dialogue title? is it ever dynamic depending on game state, or can be defined in json? 
		//printing file name
		GD.Print("dialogue to be referenced: " + dialogueFilename);
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

}
