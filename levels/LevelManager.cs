using Godot;
using System.Collections.Generic;

/**
 * a singleton to manage switching between scenes of levels and levels.
 * holds a collection of levels, loaded from a given folder path. 
 * you can also use a config file to map what levels point to which other levels. 
    */
    //TODO this might get renamed to SceneManager. see notes
public partial class LevelManager : Node {

    public static LevelManager _; //the instance
    

    //experimenting with how to deal with this. 
    public Dictionary<string, PackedScene> levelPackedScenesMap;
    public Dictionary<string, Node> activeLevelScenes; //scenes that have been instantiated during this session. 
    
    [Export]
    public string levelname; //this is the folder path in which to scan for level scenes
    public List<PackedScene> levelScenesList; //this is populated by scanning the scene files in the folder

    public Node currentLevelScene; //level that the player is in
    public HashSet<Node> activeLevelScenesSet; 
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
        levelPackedScenesMap = new Dictionary<string, PackedScene>(){ //this would also be the place to load from save file instead of default scene definition
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
    }

    public void ChangeLevel(string levelname){
        Node nextScene;
        if (activeLevelScenes.ContainsKey(levelname)){
            nextScene = activeLevelScenes[levelname];
        } else if (levelPackedScenesMap.ContainsKey(levelname)){ 
            nextScene = levelPackedScenesMap[levelname].Instantiate();
        } else {
            GD.Print("something went wrong while loading level " + levelname);
            return;
        }
        GetTree().Root.AddChild(nextScene);
        //GetTree().Root.RemoveChild(currentLevelScene);
        ((Node3D)currentLevelScene).Visible = false;
        SetActiveLevelScene(nextScene);
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
}