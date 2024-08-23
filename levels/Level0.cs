using Godot;
using System;
using System.Collections.Generic;

//NOTE: do we want to make our own custom level class?
public partial class Level0 : Level
{
	// Called when the node enters the scene tree for the first time.
	/*public override void _Ready()
	{
		SceneManager.SetFloor(new List<StaticBody3D>(){GetNode<StaticBody3D>("Floor")});
	}*/

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	
	public override void _UnhandledInput(InputEvent @event){

	}

	//TODO ?? thinking about moving this here
	public void ChangeLevel(){}
}
