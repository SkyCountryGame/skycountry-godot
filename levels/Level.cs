using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timer = Godot.Timer;

//base class for levels
public partial class Level : Node
{
	public HashSet<EventType> eventTypes => new HashSet<EventType>(){EventType.CustomScene1}; //TODO

	[Export(PropertyHint.File, "Without 'res://'")] 
	public string tscnFilePath; //tscn file which represents this level Node

	//the properties that are common to all levels
	[Export] public DirectionalLight3D sunlight;
	private float sunlightTheta; //the current angle, relative to +X, assuming that the sun orbits on the x-y plane. used to figure sun orbit

	[Export] public NavigationRegion3D navRegion;
	private Rid navMapID;
	//private List<NPCNode> npcs;

	public Aabb worldBounds; //the current bounds of all the meshes in the world
	
	public Vector3 WORLD_ORIGIN = new Vector3(0,0,0); //sunlight will always point here
	public int DURATION_DAY = 48; //in seconds

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		//if (!ValidateLevel()){
			//GD.PushError("Level is not valid");
			//return;
		//}
		Global.navReady = false;
		Global.currentLevel = this;
		if (navRegion != null) { // not all levels necessarily have nav regions. if it does, it's set in editor
			NavigationServer3D.MapChanged += (arg) => { 
				Global.navReady = true;
				navMapID = navRegion.GetNavigationMap();
			};
			Global.navRegion = navRegion;
		}

		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor

		//update sunlight position
		if (sunlight != null){
			float sunlightRadius = sunlight.Position.DistanceTo(WORLD_ORIGIN);
			sunlightTheta = Mathf.Acos(sunlight.Position.X / sunlightRadius); 
			float sunlightAngleDelta = .01745f; //1 degrees
			float sunlightAngularVelocity = 2*MathF.PI / (float) DURATION_DAY;
			float sunlightAngleUpdateInterval = sunlightAngleDelta / sunlightAngularVelocity; //how long to wait between each update such that the sun rotates around in DURATION_DAY seconds
			

			Timer sunlightUpdateTimer = new Timer();
			sunlightUpdateTimer.Timeout += ()=> {
				sunlightTheta += sunlightAngleDelta;
				sunlight.Position = new Vector3(sunlightRadius*MathF.Cos(sunlightTheta),sunlightRadius*MathF.Sin(sunlightTheta),0);
				sunlight.LookAt(WORLD_ORIGIN);
				if (sunlightTheta > 2*MathF.PI){
					sunlightTheta = 0;
					Global.HUD.LogEvent("new day!");
					GD.Print("new day");
				}
			};
			AddChild(sunlightUpdateTimer);
			sunlightUpdateTimer.Start(sunlightAngleUpdateInterval);
		}

		if (navRegion != null){
			//navRegion.BakeNavigationMesh();
		}
		worldBounds = GetWorldBounds();
		
		//grab all the nodes with group "building", "prop", and process them
		ProcessNodeGroups(); 

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

	private void ProcessNodeGroups(){
		GD.Print("processing node groups");
		foreach (Node n in NodeUtils.GetChildrenRec(this)){
			foreach (string group in n.GetGroups()){
				NodeUtils.AddToGroup(n, group);
			}
		}
		//TODO fix the redundancy that happens
	}

	//get a point that's on the navmesh that's closest to the given point
	public Vector3 GetNavPoint(Vector3 pos){
		return NavigationServer3D.MapGetClosestPoint(navMapID, pos);
	}

	//gets some random point within the world bounds
	public Vector3 GetRandomPoint(){
		return new Vector3(
			(float)GD.RandRange(worldBounds.Position.X, worldBounds.End.X),
			0,
			(float)GD.RandRange(worldBounds.Position.Z, worldBounds.End.Z)
		);
	}
	
	//gets some random point that can be navigated to
	public Vector3 GetRandomNavPoint()
	{
		Vector3 randomPoint = GetRandomPoint();
		Vector3 closestPoint = NavigationServer3D.MapGetClosestPoint(navMapID, randomPoint);
	
		if (closestPoint != Vector3.Zero)
		{
			return closestPoint;
		}
		//try again if not valid
		const int maxAttempts = 10;
		for (int i = 0; i < maxAttempts; i++)
		{
			randomPoint = GetRandomPoint();
			closestPoint = NavigationServer3D.MapGetClosestPoint(navMapID, randomPoint);
			if (closestPoint != Vector3.Zero)
			{
				return closestPoint;
			}
		}
		GD.Print("Failed to find a valid random nav point");
		return Vector3.Zero;
	}

	//gets the bounds of all the meshes in the world by combining all their axis-aligned bounding boxes
	public Aabb GetWorldBounds()
	{
		worldBounds = new Aabb();
		bool firstMesh = true;
		foreach (Node node in GetTree().GetNodesInGroup("floor"))
		{
			if (node is MeshInstance3D mesh)
			{
				Aabb meshBounds = mesh.GetAabb();

				if (firstMesh)
				{
					worldBounds = meshBounds;
					firstMesh = false;	
				}
				else
				{
					worldBounds = worldBounds.Merge(meshBounds);
				}
			}
		}
		return worldBounds;
	}

	//make sure level has the proper nodes
	public bool ValidateLevel(){
		bool valid = true;
		//make sure the tscn path exists and things are set up correctly
		if (tscnFilePath == null){
			GD.PushWarning($"Level tscn file path is not set. Will try to find a file in /levels/ with the same name as this Node name {Name} ");
			tscnFilePath = PrefabManager.FindSceneFile(Name, "levels");
		}
		if (tscnFilePath == null){
			valid = false;
		}
		//check hud, pausemenu, camera, light?
		return valid;
	}

	public void LoadSaveData(ConfigFile cfg){
		string levelName = (string) cfg.GetValue("level", "name");
		Global.ChangeLevel(levelName);
		//TODO activeScene if in a sublevel
		//TODO time, npcs, enemies				
	}
}
