using Godot;
using System;
using SkyCountry;
using System.Collections.Generic;

//NOTE: do we want to make our own custom level class?
public partial class level0 : Node3D
{
	private Player player; //so that we can tell it to walk somewhere. TODO this should probably be done through some event handler
	private World w; //TODO remove this if not used
	private DirectionalLight3D sunlight;
	private HUDManager HUD;
	private List<Node3D> neighborLevels = new List<Node3D>(); //the other levels (scenes) that are accesesible from this scene
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player =  GetNode<Player>("Player");
		sunlight = GetNode<DirectionalLight3D>("DirectionalLight3D");
		HUD = GetNode<HUDManager>("HUD");
		w = new World();
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
		float r = (float)Math.Sin(.3f*Time.GetTicksMsec() / 1000f);
		float g = (float)Math.Sin(.8f*Time.GetTicksMsec() / 1000f);
		float b = (float)Math.Cos(-.5f*Time.GetTicksMsec() / 1000f);
		sunlight.LightColor = new Color(r, g, b, .8f);
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
