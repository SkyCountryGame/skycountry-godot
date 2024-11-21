using Godot;
using System;

//a door that leads to another level. implementation might change.
public partial class Door : Node3D, Interactable
{
    public InteractionType interactionType => InteractionType.General;

    public InteractionMethod interactionMethod => InteractionMethod.Contact;

	[Export] private string destination; //where door leads to

    public override void _Ready()
	{
		if (destination == null){
		}
	}

	public override void _Process(double delta)
	{
	}

    public dynamic Interact()
    {
		if (destination == null){
			Global.hud.LogEvent("Door leads nowhere.");
		} else {
			GD.Print("Door leads to " + destination);
        	Global.level.ChangeLevel(destination);
		}
		return true;
    }

    public void Retain()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public string Info()
    {
        return "A door";
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

}
