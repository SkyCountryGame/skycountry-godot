using Godot;
using System;
using System.Collections.Generic;

//NOTE: do we want to make our own custom level class?
public partial class Level2 : Level
{		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.SceneTree = GetTree();	
		SceneManager.SetFloor(new List<StaticBody3D>(){GetNode<StaticBody3D>("Floor")});
		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
