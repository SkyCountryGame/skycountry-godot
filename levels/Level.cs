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

	//TODO actually gonna store the levels in Global, because need to load and access before Level node loaded
	[Export(PropertyHint.File, "Without 'res://'")] public Godot.Collections.Dictionary<string, string> levelSceneFilenames = new Godot.Collections.Dictionary<string, string>(); //subsequent levels that can be accessed from this level
	private Dictionary<string, PackedScene> levelScenes = new Dictionary<string, PackedScene>(); //subsequent levels that can be accessed from this level

	//the properties that are common to all levels
	[Export] public DirectionalLight3D sunlight;
	private float sunlightTheta; //the current angle, relative to +X, assuming that the sun orbits on the x-y plane. used to figure sun orbit

	[Export] public NavigationRegion3D navRegion;
	private List<Node3D> neighborLevels = new List<Node3D>(); //the other levels (scenes) that are accesesible from this scene
	//public List<NPCNode> npcs;
	public Aabb worldBounds; //the current bounds of all the meshes in the world
	
	public Vector3 WORLD_ORIGIN = new Vector3(0,0,0); //sunlight will always point here
	public int DURATION_DAY = 48; //in seconds

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.level = this;
		
		if (navRegion != null) { // not all levels necessarily have nav regions. if it does, it's be set in editor
			Global.navRegion = navRegion;
		}

		//load the levels that can be accessed from this level
		foreach (KeyValuePair<string, string> level in levelSceneFilenames){
			string fn = "";
			if (level.Value.Substr(0, 5) != "levels/"){
				fn = "levels/";
			} 
			if (level.Value.Substr(level.Value.Length - 5, 5) != ".tscn"){
				fn += level.Value + ".tscn";
			} else {
				fn += level.Value;
			}
			levelScenes[level.Key] = ResourceLoader.Load<PackedScene>("res://" + fn); 
        }
		
		//dynamically spawn things
		//health pickups
		//enemies
		//find random position on floor

		//update sunlight position
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
		};
		AddChild(sunlightUpdateTimer);
		sunlightUpdateTimer.Start(sunlightAngleUpdateInterval);
		if (navRegion != null){
			navRegion.BakeNavigationMesh();
		}
		worldBounds = GetWorldBounds();
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

	//gets some random point within the world bounds
	public Vector3 GetRandomPoint(){
		//worldBounds = GetWorldBounds();
		return new Vector3(
			(float)GD.RandRange(worldBounds.Position.X, worldBounds.Size.X),
			0,
			(float)GD.RandRange(worldBounds.Position.Z, worldBounds.Size.Z)
		);
	}
	//gets some random point that can be navigated to
	public Vector3 GetRandomNavPoint(){
		Rid navMapID = Global.navRegion.GetNavigationMap();
		Vector3 res = NavigationServer3D.MapGetClosestPoint(navMapID, GetRandomPoint());
		while (res.X == 0 && res.Z == 0){
			res = NavigationServer3D.MapGetClosestPoint(navMapID, GetRandomPoint());
		}
		GD.Print("random nav point:" + res);
		return res;
	}

	//gets the bounds of all the meshes in the world by combining all their axis-aligned bounding boxes
	public Aabb GetWorldBounds()
	{
		worldBounds = new Aabb();
		bool firstMesh = true;
		foreach (Node node in GetTree().GetNodesInGroup("mesh"))
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

	public void ChangeLevel(string levelName){
		if (levelScenes.ContainsKey(levelName)){
			GetTree().ChangeSceneToPacked(levelScenes[levelName]);
        }
	}

	//make sure level has the proper nodes
	public bool ValidateLevel(){
		//check hud, pausemenu, camera, light?
		return true;
	}

	public void LoadSaveData(ConfigFile cfg){
		string levelName = (string) cfg.GetValue("level", "name");
		ChangeLevel(levelName);
		//TODO activeScene if in a sublevel
		//TODO time, npcs, enemies				
	}
}
