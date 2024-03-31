using Godot;
using System;

public partial class Chest : RigidBody3D, Interactable
{
    public InteractionType interactionType => throw new NotImplementedException();

    public InteractionMethod interactionMethod => throw new NotImplementedException();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } mb)
		{
			if (mb.IsReleased())
			{
				GD.Print("chest clicked");	
			}
			else
			{
				
			}
			
		}
	}

    public dynamic Interact()
    {
        GD.Print("interact");
		return null;
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

    public bool IsInteractionValid(GameObject interactor)
    {
        throw new NotImplementedException();
    }

}
