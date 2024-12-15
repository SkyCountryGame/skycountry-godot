using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{

	public static Global Instance {get; private set; } //the instance
	public static Global _; //shorhand for the instance

	public static int saveSlot = -1; //set by MainMenu on game start

	//GAME NODES
	public static PlayerModel playerModel; //set by Player, persists between scenes
	public static Player playerNode;  //set by Player 
	public static DialogueFunctionController dialogueFunctionController;
	public static Camera cam; //set by Camera on ready (probably will change because alternate cameras)
	public static HUDManager HUD; // set by HUDManager on ready
	public static PauseMenu pauseMenu; // set by PauseMenu on ready
	public static NavigationRegion3D navRegion; // set by each level
	public static Level currentLevel; // the current "level", set by each level on ready
	
	//GAME LOGIC MANAGER THINGS
	public static AtmosphereManager atmosphereManager; // constructed here in init()
	//public static EffectsManager effectsManager; // constructed here in init() //TODO actually might only have static methods 

	public Dictionary<PackedScene, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it)

	public static Dictionary<Node, GameObject> mapNodeGameObjects = new Dictionary<Node, GameObject>();  //map godot nodes to game objects
	public static Dictionary<GameObject, Interactable> mapGameObjectToInteractable = new Dictionary<GameObject, Interactable>();
	public static Dictionary<GameObject, List<Node>> mapGameObjectNodes = new Dictionary<GameObject, List<Node>>(); //map game objects to their nodes
	public static Dictionary<GameObjectType, HashSet<GameObject>> mapTypeGameObjects = new Dictionary<GameObjectType, HashSet<GameObject>>(); //map types to game objects

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
		PrefabManager.Init();
		atmosphereManager = new AtmosphereManager();
		dialogueFunctionController = new DialogueFunctionController();
		ProcessMode = ProcessModeEnum.Always;
		foreach (GameObjectType t in Enum.GetValues(typeof(GameObjectType))){
			mapTypeGameObjects.Add(t, new HashSet<GameObject>());
		}
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

	public static void ChangeLevel(string levelName, Node previousScene = null){
		Level nextLevel = (Level) ResourceLoader.Load<PackedScene>(levelName).Instantiate();
		if (previousScene == null){
			if (currentLevel == null){
				GD.PushError("No previous scene. Perhaps trying to enter a level from menu and forgot to pass previousScene?");
			} else {
				previousScene = currentLevel;
			}
		}
		_.GetTree().Root.AddChild(nextLevel);
		
		if (playerNode!=null){
			playerNode.CallDeferred("reparent", nextLevel);
		}
		else { //NOTE shouldn't assume that the next level has player by default. it is currently done this way to persist child nodes of player such as equiped items. 
			playerNode = (Player) ResourceLoader.Load<PackedScene>("res://player/player.tscn").Instantiate();
			nextLevel.AddChild(playerNode);
		}

		playerNode.Position = ((Node3D) nextLevel.FindChild("SpawnLocation")).GlobalPosition; //TODO use exported spawnpoint node instead, or have Level handle player spawning and positioning. 
		_.GetTree().Root.CallDeferred("remove_child", previousScene);
		currentLevel = nextLevel;		
	}

	//associate a game object with all of the nodes that comprise it
	public static void RegisterGameObject(List<Node> nodes, GameObjectType type){
		GameObject go = null;
		//first check to see there is already a game object associated with any of the nodes.
		foreach (Node node in nodes){
			if (mapNodeGameObjects.ContainsKey(node)){
				go = mapNodeGameObjects[node];
				break;
			}
		}
		if (go == null){
			go = new GameObject(type);
			GD.Print("NEW GAME OBJECT " + go.id);
			mapGameObjectNodes.Add(go, nodes); //associate all the nodes with the game object
			foreach (Node node in nodes){
				mapNodeGameObjects.Add(node, go);
				if (node is Interactable){
					GD.Print("ADDING INTERACTABLE GAME OBJ " + node.Name);
					mapGameObjectToInteractable.Add(go, (Interactable)node); //this assumes that each gameobject can have only one interactable
				}
			}
			mapTypeGameObjects[type].Add(go);
			
		} else {
			mapGameObjectNodes[go].AddRange(nodes);
		}
		//now associate the type. 
		foreach (GameObjectType t in Enum.GetValues(typeof(GameObjectType))){
			if ((type & t) != 0 && !mapTypeGameObjects[t].Contains(go)){
				mapTypeGameObjects[t].Add(go);

			}
		}
		return;

	}

	public static void RegisterGameObject(Node node, string name, GameObjectType type){
		RegisterGameObject(GetGameObjectNodes(node), type);
	}

	public static void RegisterGameObject(Node node, GameObjectType type){
		RegisterGameObject(node, node.Name, type);
	}

	public static GameObject GetGameObject(Node n){
		if (mapNodeGameObjects.ContainsKey(n)){
			return mapNodeGameObjects[n];
		}
		/*while (n.GetParent() != null){
			n = n.GetParent();
			if (mapNodeGameObjects.ContainsKey(n)){
				return mapNodeGameObjects[n];
			}
		}*/
		return null;
	}

	//helper functin to get all the nodes that comprise a game object, given its root node
    public static List<Node> GetGameObjectNodes(Node n){
		List<Node> res = new List<Node>(){n};
        res.AddRange(n.GetChildren());
		return res;
	}

	public static Interactable GetInteractable(Node n, bool strict = false){
		GameObject go = GetGameObject(n);
		return GetInteractable(go);
	}
	public static Interactable GetInteractable(GameObject go){
		if (go != null && mapTypeGameObjects[GameObjectType.Interactable].Contains(go)){

			return mapGameObjectToInteractable[go];
		}
		return null;
	}

	public static void RegisterInteractable(Interactable interactable){
		GameObject go = mapNodeGameObjects[(Node)interactable];
		if (go != null){
			mapGameObjectToInteractable.Add(go, interactable);
		}
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
		cfg.SetValue("level", "name", currentLevel.Name);
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
		currentLevel.LoadSaveData(cfg);
	}
}
