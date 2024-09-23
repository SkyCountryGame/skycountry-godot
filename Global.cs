using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class Global : Node
{

	public static Global Instance {get; private set; } //the instance
	public static Global _; //shorhand for the instance
	
	//GAME NODES
	public static PlayerModel playerModel; //set by Player, persists between scenes
	public static Player playerNode;  //set by Player 
	public static Camera cam; //set by Camera on ready
	public static SceneTree sceneTree; // set by each level on ready TODO probably wont need this
	public static HUDManager hud; // set by HUDManager on ready
	public static NavigationRegion3D navRegion; // set by each level
	public static Level level; // the current "level", set by each level on ready
	
	//GAME LOGIC MANAGER THINGS
	public static SceneManager sceneManager; // constructed here in init()
	public static AtmosphereManager atmosphereManager; // constructed here in init()
	//public static EffectsManager effectsManager; // constructed here in init() //TODO actually might only have static methods

	//GAME OBJECT PREFABS (PackedScene)
	public static Dictionary<string, PackedScene> prefabs; 

	//TODO static reference for ease of use? 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null){ //only preload the stuff once on level start (not scene start)
            init();
            Instance = this;
            _ = Instance;
        }
	}

	public void init(){
		sceneTree = GetTree();
		sceneManager = new SceneManager();
		atmosphereManager = new AtmosphereManager();
		prefabs = new Dictionary<string, PackedScene>  //TODO should probably have a folder that is automatically scanned
        {
            { "FloatingText", ResourceLoader.Load<PackedScene>("res://gameobjects/floatingtext.tscn") },
            { "ERROR", ResourceLoader.Load<PackedScene>("res://gameobjects/error.tscn") },
            { "Player", ResourceLoader.Load<PackedScene>("res://player/player.tscn") },
			{ "MarkerPoint", ResourceLoader.Load<PackedScene>("res://gameobjects/markerpoint.tscn") },
        };
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
