using Godot;
using System;

public partial class Player : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("player ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//testing input. TODO move into an inputer manager
		float input = Input.GetActionStrength("left");
		ApplyCentralForce(input*Vector3.Forward*1200*(float)delta);
	}
	
	public override void _Input(InputEvent @event){
		//TODO make InputManager class
		GD.Print(@event);
	}
}
