using Godot;
using System;

public partial class Door : Node3D, Interactable
{
    public InteractionType interactionType => InteractionType.General;

    public InteractionMethod interactionMethod => InteractionMethod.Contact;

	[Export] private Level destination; //where door leads to

    public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

    public dynamic Interact()
    {
        //Global.level.ChangeLevel(destination);
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
        throw new NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

}
