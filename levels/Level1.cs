using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime;

//an example sky country level. don't know if we need to have an abstract base class yet. 
public partial class Level1 : Level
{		

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		IntroTask();
	}


	private void IntroTask(){
		Task addPlayerTask = new Task(()=>{
			System.Threading.Thread.Sleep(300);
			GD.Print("welcome to cutscene");
			System.Threading.Thread.Sleep(800);
			GD.Print("spawn soon");
			System.Threading.Thread.Sleep(600);
			CallDeferred("InstantiatePlayer");
		});
		addPlayerTask.Start();
	}

	private void InstantiatePlayer(){
		Player p = Global.prefabs["Player"].Instantiate() as Player;
		Global.playerNode = p;
		Global.cam.target = p;
		AddChild(p); //already in a deferred call
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
