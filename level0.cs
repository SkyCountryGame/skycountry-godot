using Godot;
using System;
using SkyCountry;

public partial class level0 : Node3D
{
	private Player player; //so that we can tell it to walk somewhere. TODO this should probably be done through some event handler
	private World w;
	private OmniLight3D lampPost;
	private DirectionalLight3D sunlight;
	private HUDManager HUD;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player =  GetNode<Player>("Player");
		//lampPost = GetNode<OmniLight3D>("LampPost/CollisionShape3D/StaticBody3D/OmniLight3D");
		sunlight = GetNode<DirectionalLight3D>("DirectionalLight3D");
		HUD = GetNode<HUDManager>("HUD");
		w = new World();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float r = (float)Math.Sin(.3f*Time.GetTicksMsec() / 1000f);
		float g = (float)Math.Sin(.8f*Time.GetTicksMsec() / 1000f);
		float b = (float)Math.Sin(-.5*delta / 1000f);
		sunlight.LightColor = new Color(r, g, b, 1);
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
