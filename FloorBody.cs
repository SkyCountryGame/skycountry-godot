using Godot;
using System;

public partial class FloorBody : StaticBody3D
{
	private Player2 player; //so that we can tell it to walk somewhere. TODO this should probably be done through some event handler
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player =  GetNode<Player2>("../../../Player2");
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	//input event is called after automatic raycast from screen point to world object has occured. 
	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } mb)
		{
			if (mb.IsReleased())
			{
				GD.Print("set player destination");
				player.SetTravelDestination(position);	
			}
			else
			{
				//do something fancy to the cursor
			}
			
		}
	}
}
