using Godot;
using System;
using System.Collections.Generic;

//base class for levels
public partial class Level : Node3D
{
	private DirectionalLight3D sunlight;
	private List<Node3D> neighborLevels = new List<Node3D>(); //the other levels (scenes) that are accesesible from this scene
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTree old = Global.sceneTree; //testing
		Global.sceneTree = GetTree(); //TODO remove if not use
		
		SceneManager.SetFloor(new List<StaticBody3D>(){GetNode<StaticBody3D>("Floor")});
		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//TODO where change lighting for time of day? maybe a timer that repeats every several minutes to slightly change the color.
		
		
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
