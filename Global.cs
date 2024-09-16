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
	public static PrefabManager prefabMgr; // set by PrefabManager on ready
	public static Dictionary<string, PackedScene> prefabs;
	public static NavigationRegion3D navRegion; // set by each level
	public static Level level; // the current "level", set by each level on ready
	
	//GAME LOGIC MANAGER THINGS
	public static AtmosphereManager atmosphereManager; // constructed here in init()
	//public static EffectsManager effectsManager; // constructed here in init() //TODO actually might only have static methods 

	public Dictionary<PackedScene, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it)

    public static Dictionary<Node, GameObject> gameObjects = new Dictionary<Node, GameObject>();  //map godot nodes to game objects
    public static HashSet<Interactable> interactables = new HashSet<Interactable>(); //interactable objects in the game TODO might not be in this class?
	public static Dictionary<GameObject, Interactable> mapGameObjectToInteractable = new Dictionary<GameObject, Interactable>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null){ //only preload the stuff once on level start (not scene start)
            init();
            Instance = this;
            _ = Instance;
			SceneTree st = this.GetTree(); //testing
			GD.Print(st.ToString());
        }
	}

	public void init(){
		sceneTree = GetTree();
		atmosphereManager = new AtmosphereManager();

		prefabs = new Dictionary<string, PackedScene>();
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://gameobjects/lamppost.tscn"));
        prefabs.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://gameobjects/floatingtext.tscn"));
        prefabs.Add("ERROR", ResourceLoader.Load<PackedScene>("res://gameobjects/error.tscn"));
        prefabs.Add("Player", ResourceLoader.Load<PackedScene>("res://player/player.tscn"));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
                interactables.Add((Interactable)node);
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

	public static Interactable GetInteractable(Node n){
		GameObject go = GetGameObject(n);
        if (go != null && mapGameObjectToInteractable.ContainsKey(go)){
            return mapGameObjectToInteractable[go];
        }
        return null;
	}
}
