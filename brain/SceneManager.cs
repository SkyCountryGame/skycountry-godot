using Godot;
using System;
using System.Collections.Generic;

/**
 * a singleton to manage:
    - loading levels
    - switching between scenes of levels and levels.
    - loading gameobjects
    - keeping track of relevant scenes (nodes) 
 * holds a collection of levels and a collection of game objects, loaded from given folder path. 
 * you can also use a config file to map what levels point to which other levels (TODO).
 * we use ChangeSceneToPacked() for scene switching. performance seems to be fine. we only load PackedScenes from disk on level start, not on each scene start. each scene just instantiates that PackedScene (which is in RAM) into a Node3D.  
    */
public partial class SceneManager : Node, EventListener {

    public static SceneManager Instance {get; private set; } //the instance

    public HashSet<EventType> eventTypes => new HashSet<EventType>(){EventType.CustomScene1};


    public static SceneManager _; //shorhand for the instance 
    
    //LEVEL STUFF
    //experimenting with how to deal with this. 
    public Dictionary<string, PackedScene> levelScenesPacked;
    public Dictionary<string, Node> activeLevelScenes; //scenes that have been instantiated during this session. 
    
    [Export]
    public string levelname; //this is the folder path in which to scan for level scenes
    public List<PackedScene> levelScenesList; //this is populated by scanning the scene files in the folder

    public Node currentLevelScene; //level that the player is in
    public HashSet<Node> activeLevelScenesSet; 
    
    //GAME OBJECT STUFF
    public Dictionary<string, PackedScene> prefabs; //prefabs are just PackedScenes that are used to instantiate game objects
    public HashSet<Node>activeNodes; //game object nodes that have been instantiated during this session and are currently active in the scene
    public Dictionary<PackedScene, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it)

    public AtmosphereManager atmosphereManager; //NOTE: putting this here for now. in another branch i will be reorganization Global, SceneManager, etc. 

    //associate each godot node with its "sky country game object" 
    public static Dictionary<Node, GameObject> gameObjects = new Dictionary<Node, GameObject>();  //map godot nodes to game objects
    public static HashSet<Interactable> interactables = new HashSet<Interactable>(); //interactable objects in the game
    //public static HashSet<SpawnPoint> spawnPoints = new HashSet<SpawnPoint>();  //TODO how to set this stuff up. see GameObjectType in GameObject.cs
    public static Dictionary<GameObject, Interactable> mapGameObjectToInteractable = new Dictionary<GameObject, Interactable>();

    public override void _Ready()
    {
        if (Instance == null){ //only preload the stuff once on level start (not scene start)
            init();
            Instance = this;
            _ = Instance;
        }
    }

    public void init(){
        EventManager.RegisterListener(this);
        //load the level scenes from somewhere
        levelScenesPacked = new Dictionary<string, PackedScene>(){ //this would also be the place to load from save file instead of default scene definition
            {"l0", ResourceLoader.Load<PackedScene>("res://levels/level0.tscn")},
            //{"l0-1", ResourceLoader.Load<PackedScene>("res://levels/level0-1.tscn")},
            //{"l0-2", ResourceLoader.Load<PackedScene>("res://levels/level0-2.tscn")},
            //{"l0-3", ResourceLoader.Load<PackedScene>("res://levels/level0-3.tscn")},
            {"l2", ResourceLoader.Load<PackedScene>("res://levels/level2.tscn")},
        }; //this is manually defined, which we probably wont use, but leaving it here for now for example
        activeLevelScenes = new Dictionary<string, Node>();
        activeLevelScenesSet = new HashSet<Node>();

        //Option 1: iterate through the scene files in the folder for the level
        //string[] scenefilepaths = System.IO.Directory.GetFiles(levelname);
        //foreach (string filename in scenefilepaths){
        //    if (filename.EndsWith(".tscn")){

        //    }
        //    levelScenesList.Add(ResourceLoader.Load<PackedScene>(filename));
        //}

        //Option 2: iterate through the lines of a config textfile to build the map of accessible levels
        //TODO

        //GameObject Stuff
        prefabs = new Dictionary<string, PackedScene>();
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://gameobjects/lamppost.tscn"));
        prefabs.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://gameobjects/floatingtext.tscn"));
        prefabs.Add("ERROR", ResourceLoader.Load<PackedScene>("res://gameobjects/error.tscn"));

        activeNodes = new HashSet<Node>();

        atmosphereManager = new AtmosphereManager();
    }

    
    public static void RegisterGameObject(Node node, GameObjectType type){
        RegisterGameObject(node, node.Name, type);
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

    public void ChangeLevel(string levelname){
        activeLevelScenesSet.Clear();
        if (levelScenesPacked.ContainsKey(levelname)){
            currentLevelScene.GetTree().ChangeSceneToPacked(levelScenesPacked[levelname]);
            activeNodes.Clear();
            currentLevelScene = GetTree().CurrentScene;
            SetActiveLevelScene(currentLevelScene);
            Global.sceneTree = GetTree();
        }
        /*
        Node nextScene;
        if (activeLevelScenes.ContainsKey(levelname)){
            nextScene = activeLevelScenes[levelname];
        } else if (levelScenesPacked.ContainsKey(levelname)){ 
            nextScene = levelScenesPacked[levelname].Instantiate();
        } else {
            GD.Print("something went wrong while loading level " + levelname);
            return;
        }
        currentLevelScene.RemoveChild(player);
        Node levelParent = currentLevelScene.GetParent();
        GetTree().Root.RemoveChild(currentLevelScene);
        //player.GetParent().RemoveChild(player);
        //levelParent.AddChild(player);
        GetTree().Root.AddChild(currentLevelScene);
        nextScene.AddChild(player);
        //Global.Cam = nextScene.GetNode<Camera3D>("Camera3D") as Camera;
        
        //GetTree().Root.AddChild(nextScene);
        //((Node3D)currentLevelScene).Visible = false;

        SetActiveLevelScene(nextScene);
        */    
    }

    /**
     * update the currently-being-played level scene Node
     */
    public void SetActiveLevelScene(Node scene){
        if(scene!=null){
            if (!activeLevelScenesSet.Contains(scene)){
                activeLevelScenesSet.Add(scene);
            }
            currentLevelScene = scene;
        }
    }

    public void SpawnObject(string obj, Vector3 position){
        if (prefabs.ContainsKey(obj) && prefabs[obj] != null){
            Node3D node = (Node3D) prefabs[obj].Instantiate();
            node.Position = position;
            node.Name = obj;
            Global.sceneTree.Root.AddChild(node);
            prefabs[obj].Instantiate();
        }
    }

    //traverse up the node tree to see if this is an interactable. TODO might need to make sure to stop at some point if the node tree goes all the way up to level
    public static Interactable GetInteractable(Node n){
        GameObject go = GetGameObject(n);
        if (go != null && mapGameObjectToInteractable.ContainsKey(go)){
            return mapGameObjectToInteractable[go];
        }
        return null;
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

    public void HandleEvent(Event e)
    {
        GD.Print($"SceneManager handling event {e.eventType.ToString()}");
    }

}