using Godot;
using System.Collections.Generic;

/**
 * a singleton to manage:
    - loading levels
    - switching between scenes of levels and levels.
    - loading gameobjects
    - keeping track of relevant scenes (nodes) 
 * holds a collection of levels and a collection of game objects, loaded from given folder path. 
 * you can also use a config file to map what levels point to which other levels. 
    */
public partial class SceneManager : Node {

    public static SceneManager _; //the instance
    
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
    public Dictionary<string, PackedScene> gameObjectsPacked; //can this be static? 
    public HashSet<Node>activeGameObjects; //game objects that have been instantiated during this session.

    
    private static List<StaticBody3D> floor;

    public override void _Ready()
    {
        if (_ == null){
            init();
            _ = this;
        }
    }

    public void init(){
        //TODO, maybe this should be called from level script which passes in the level names. 
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
        gameObjectsPacked = new Dictionary<string, PackedScene>();
        floor = new List<StaticBody3D>();
        gameObjectsPacked.Add("Rock", ResourceLoader.Load<PackedScene>("res://interact/rock.tscn"));
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://entity/lamppost.tscn"));
        gameObjectsPacked.Add("Enemy", ResourceLoader.Load<PackedScene>("res://entity/enemy.tscn"));
        gameObjectsPacked.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://floatingtext.tscn"));
    }

    public void ChangeLevel(string levelname){
        /*Node nextScene;
        if (activeLevelScenes.ContainsKey(levelname)){
            nextScene = activeLevelScenes[levelname];
        } else if (levelScenesPacked.ContainsKey(levelname)){ 
            nextScene = levelScenesPacked[levelname].Instantiate();
        } else {
            GD.Print("something went wrong while loading level " + levelname);
            return;
        }
        GetTree().Root.AddChild(nextScene);
        GetTree().Root.RemoveChild(currentLevelScene);
        ((Node3D)currentLevelScene).Visible = false;
        SetActiveLevelScene(nextScene);
        */



        //this is just a sanity check. 
        if (levelScenesPacked.ContainsKey(levelname)){
            Node testLeveLoad = levelScenesPacked[levelname].Instantiate();
            GetTree().ChangeSceneToPacked(levelScenesPacked[levelname]);
        }
    }

    /**
     * update the currently-being-played level scene Node
     */
    public void SetActiveLevelScene(Node scene){
        if (!activeLevelScenesSet.Contains(scene)){
            activeLevelScenesSet.Add(scene);
        }
        currentLevelScene = scene;
    }

    public void SpawnObject(string obj, Vector3 position){
        if (gameObjectsPacked.ContainsKey(obj) && gameObjectsPacked[obj] != null){
            Node3D node = (Node3D) gameObjectsPacked[obj].Instantiate();
            node.Position = position;
            node.Name = obj;
            Global._SceneTree.Root.AddChild(node);
            gameObjectsPacked[obj].Instantiate();
        }
    }

    public static void SetFloor(List<StaticBody3D> floors){
        floor.Clear();
        foreach (var f in floors){
            floor.Add(f);
        }
    }
}