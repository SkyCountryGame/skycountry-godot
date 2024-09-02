using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timer = Godot.Timer;

//base class for levels
public partial class Level : Node3D
{
	//the properties that are common to all levels
	[Export] public DirectionalLight3D sunlight;
	private float sunlightTheta; //the current angle, relative to +X, assuming that the sun orbits on the x-y plane. used to figure sun orbit

	private List<Node3D> neighborLevels = new List<Node3D>(); //the other levels (scenes) that are accesesible from this scene
	public List<NPCNode> npcs;
	
	public Vector3 WORLD_ORIGIN = new Vector3(0,0,0); //sunlight will always point here
	public int DURATION_DAY = 1200; //in seconds

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTree old = Global.sceneTree; //testing
		Global.sceneTree = GetTree(); //TODO remove if not use
		
		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor

		//update sunlight position
		float sunlightRadius = sunlight.Position.DistanceTo(WORLD_ORIGIN);
		sunlightTheta = Mathf.Acos(sunlight.Position.X / sunlightRadius); 
		float sunlightAngleDelta = .0349f; //2 degrees
		float sunlightAngularVelocity = 2*MathF.PI / DURATION_DAY;
		float sunlightAngleUpdateInterval = sunlightAngleDelta / sunlightAngularVelocity; //how long to wait between each update such that the sun rotates around in DURATION_DAY seconds
		Timer sunlightUpdateTimer = new Timer();
		sunlightUpdateTimer.Timeout += ()=> {
			sunlightTheta += sunlightAngleDelta;
			sunlight.Position = new Vector3(sunlightRadius*MathF.Cos(sunlightTheta),0,sunlightRadius*MathF.Sin(sunlightTheta));
		};
		sunlightUpdateTimer.Start();
		//GetTree().CreateTimer(sunlightAngleUpdateInterval);

	/*
		Task.Run(()=>{
			int timerSleepDuration = 10000; //placeholder. TODO calculate time so that we do 1-3 degrees of rotation
			while (true){
				Thread.Sleep(timerSleepDuration);
				
			}
		});
	 */   
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//TODO where change lighting for time of day? maybe a timer that repeats every several minutes to slightly change the color.
		
		
	}
	
	public override void _UnhandledInput(InputEvent @event){

	}
}
