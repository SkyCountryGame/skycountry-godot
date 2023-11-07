using Godot;
using System;
using SkyCountry;

//currently this is the only script attached to the player, so it is acting as the playercontroller. 
//the reason it's a RigidBody3D is because that's what the root scenenode of player is
public partial class Player : RigidBody3D//, InputActionListener
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("player ready");
		//register controller
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _Input(InputEvent @event){
		//TODO make InputManager class
		if (Input.IsActionPressed("player_action1"))
		{ //set destination
			InputEventMouseButton mEvent = ((InputEventMouseButton)@event);
			//mEvent.Position;
		}
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		GD.Print($"{position}, {normal}, {shape_idx}");
	}

	public void SetTravelDestination(Vector3 pos)
	{
		GD.Print("setting player destination");
	}
	
	/*
	public void HandleActionEnable(InputEventAction type, bool en)
	{
		//in the future i might actually want to use my own InputAction enum for further decoupling from godot or any engine
		string t = type.AsText();
		switch (t)
		{
			case "left":
				break;
		}
	}

	public void HandleContinuousAction(InputEventAction a)
	{
		throw new NotImplementedException();
	}*/
}
