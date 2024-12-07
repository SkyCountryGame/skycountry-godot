using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class Global : Node
{

	public static Global Instance {get; private set; } //the instance
	public static Global _; //shorhand for the instance

	public static int saveSlot = -1; //set by MainMenu on game start

	//GAME NODES
	public static PlayerModel playerModel; //set by Player, persists between scenes
	public static Player playerNode;  //set by Player 
	public static Camera cam; //set by Camera on ready (probably will change because alternate cameras)
	public static SceneTree sceneTree; // set by each level on ready TODO probably wont need this
	public static HUDManager HUD; // set by HUDManager on ready
	public static PauseMenu pauseMenu; // set by PauseMenu on ready
	public static PrefabManager prefabMgr; // constructed here in init()
	public static Dictionary<string, PackedScene> prefabs;
	public static NavigationRegion3D navRegion; // set by each level
	public static Level level; // the current "level", set by each level on ready
	
	//GAME LOGIC MANAGER THINGS
	public static AtmosphereManager atmosphereManager; // constructed here in init()
	//public static EffectsManager effectsManager; // constructed here in init() //TODO actually might only have static methods 

	public Dictionary<PackedScene, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it)

	public static Dictionary<Node, GameObject> gameObjects = new Dictionary<Node, GameObject>();  //map godot nodes to game objects
	public static Dictionary<GameObject, Interactable> mapGameObjectToInteractable = new Dictionary<GameObject, Interactable>();

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
		atmosphereManager = new AtmosphereManager();
		prefabMgr = new PrefabManager();
		prefabs = prefabMgr.prefabs; //for shorthand
		ProcessMode = ProcessModeEnum.Always;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _Input(InputEvent @event){
		if (Input.IsActionJustPressed("pause")){
			TogglePause();
		}
	}

	public static void RegisterGameObject(Node node, string name, GameObjectType type){
		GameObject go;
		if (!gameObjects.ContainsKey(node)){
			go = new GameObject(node);
			gameObjects.Add(node, go);
		} else {
			go = gameObjects[node];
		}
		switch(type){
			case GameObjectType.Interactable:
				mapGameObjectToInteractable.Add(go, (Interactable)node);
				break;
			case GameObjectType.SpawnPoint:
				//spawnPoints.Add((SpawnPoint)node);
				break;
			
			default:
				break;
		}

	}
	public static void RegisterGameObject(Node node, GameObjectType type){
		RegisterGameObject(node, node.Name, type);
	}

	public static GameObject GetGameObject(Node n){
		if (gameObjects.ContainsKey(n)){
			return gameObjects[n];
		}
		while (n.GetParent() != null){
			n = n.GetParent();
			if (gameObjects.ContainsKey(n)){
				return gameObjects[n];
			}
		}
		return null;
	}

	public static Interactable GetInteractable(Node n, bool strict = false){
        if (strict){
            if (n is Interactable interactable){ return interactable; }
            else { return null; }
        }
        while (n != sceneTree.Root){
            if (n is Interactable interactable)
            {
                return interactable;
            }
            n = n.GetParent();
        }
        return null;
    }
	public static Interactable GetInteractable(GameObject go){
		if (mapGameObjectToInteractable.ContainsKey(go)){
			return mapGameObjectToInteractable[go];
		}
		return null;
	}

	public static void TogglePause(){
		_.GetTree().Paused = !_.GetTree().Paused;
		pauseMenu.Visible = _.GetTree().Paused;
	}
	public static void ResumeGame(){
		_.GetTree().Paused = false;
		pauseMenu.Visible = false;
	}

	public static void SaveGame(){
		//GetSaveData() for each of player, level, enemies, NPCs, weather, (some may be part of level)
		//https://docs.godotengine.org/en/stable/tutorials/io/binary_serialization_api.html
		//https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html
		//might just make 1 Resource class that has all the attributes to save from every game object
		//saving entire nodes as PackedScene is probably overkill because extra members, but should experiment with this. 

		ConfigFile cfg = new ConfigFile(); //this is editable. use FileAccess for binary encoding to obfuscate
		cfg.SetValue("player", "position", playerNode.GlobalPosition);
		cfg.SetValue("player", "transform", playerNode.GlobalTransform);
		cfg.SetValue("player", "rotation", playerNode.Rotation);
		cfg.SetValue("player", "model", playerModel);
		cfg.SetValue("level", "name", level.Name);
		cfg.SetValue("level", "activeScene", playerNode.GetTree().CurrentScene);
		//cfg.SetValue("level", "time", );
		//foreach (NPCNode npcn in level.npcs){
		//    cfg.SetValue("level", "npcs", $"test-{npcn.Name}");
		//}
		//TODO enemies
		//maybe pass the cfg object to Level.PopulateCfg(cfg) to delegate business logic
		cfg.Save($"user://savegame{saveSlot}.cfg");
	}

	public static void LoadGame(){
		ConfigFile cfg = new ConfigFile(); 
		cfg.Load($"user://savegame{saveSlot}.cfg");
		playerNode.LoadSaveData(cfg);
		level.LoadSaveData(cfg);
	}
}
