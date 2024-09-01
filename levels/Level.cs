using Godot;
using System;
using System.Collections.Generic;

//base class for levels
public partial class Level : Node
{
	private DirectionalLight3D sunlight;
	private List<Node3D> neighborLevels = new List<Node3D>(); //the other levels (scenes) that are accesesible from this scene
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.SceneTree = GetTree();
		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float r = (float)Math.Sin(.3f*Time.GetTicksMsec() / 1000f);
		float g = (float)Math.Sin(.8f*Time.GetTicksMsec() / 1000f);
		float b = (float)Math.Cos(-.5f*Time.GetTicksMsec() / 1000f);
		//sunlight.LightColor = new Color(r, g, b, .8f);
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
