using Godot;
using System;

public partial class Orbowl : StaticBody3D
{
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
		GD.Print("orbbowl input event");
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } mb)
		{
			if (mb.IsReleased())
			{
				GD.Print("clicked a static orb-bowl thing");	
			}
			else
			{
				
			}
			
		}
	}
}
