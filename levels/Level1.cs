using Godot;
using System;
using SkyCountry;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime;

//an example sky country level. don't know if we need to have an abstract base class yet. 
public partial class Level1 : Node3D
{
	private Player player;
	private DirectionalLight3D sunlight;
	private HUDManager HUD;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//for this level, we need to spawn the player after loading. for example maybe there's a cutscene
		
		//lampPost = GetNode<OmniLight3D>("LampPost/CollisionShape3D/StaticBody3D/OmniLight3D");
		sunlight = GetNode<DirectionalLight3D>("DirectionalLight3D");
		HUD = GetNode<HUDManager>("HUD");
		//Callable introCallable = new Callable(this, "IntroTask");
		//HUD.Connect("ReadySignal", introCallable); //wait for the HUD to be ready beforedoing the intro thing
		CallDeferred("IntroTask");
	}


	private void IntroTask(){
		Task addPlayerTask = new Task(()=>{
			System.Threading.Thread.Sleep(300);
			GD.Print("welcome to cutscene");
			System.Threading.Thread.Sleep(2000);
			GD.Print("spawn soon");
			System.Threading.Thread.Sleep(1000);
			Player p = ResourceLoader.Load<PackedScene>("res://player.tscn").Instantiate() as Player;
			GetTree().Root.CallDeferred("add_child", p);
			Global.Cam.target = p;	
		});
		addPlayerTask.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float r = (float)Math.Sin(.3f*Time.GetTicksMsec() / 1000f);
		float g = (float)Math.Sin(.1f*Time.GetTicksMsec() / 1000f);
		float b = (float)Math.Cos(-.05f*Time.GetTicksMsec() / 1000f);
		sunlight.LightColor = new Color(r, g, b, .8f);
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
