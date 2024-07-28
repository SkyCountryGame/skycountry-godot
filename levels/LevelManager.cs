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
    public Dictionary<string, PackedScene> levelScenesMap;
    
    [Export]
    public string levelname; //this is the folder path in which to scan for level scenes
    public List<PackedScene> levelScenesList; //this is populated by scanning the scene files in the folder
    
    private static List<StaticBody3D> floor;

    public override void _Ready()
    {
        base._Ready();
        if (_ == null){
            init();
            _ = this;
        }
    }

    public void init(){

        //load the level scenes from somewhere
        levelScenesMap = new Dictionary<string, PackedScene>(){
            {"l0", ResourceLoader.Load<PackedScene>("res://levels/level0.tscn")},
            {"l0-1", ResourceLoader.Load<PackedScene>("res://levels/level0-1.tscn")},
            {"l0-2", ResourceLoader.Load<PackedScene>("res://levels/level0-2.tscn")},
            {"l0-3", ResourceLoader.Load<PackedScene>("res://levels/level0-3.tscn")},
            {"l0-4", ResourceLoader.Load<PackedScene>("res://levels/level0-4.tscn")},
        }; //this is manually defined, which we probably wont use, but leaving it here for now for example

        //Option 1: iterate through the scene files in the folder for the level
        string[] scenefilepaths = System.IO.Directory.GetFiles(levelname);
        foreach (string filename in scenefilepaths){
            if (filename.EndsWith(".tscn")){

            }
            levelScenesList.Add(ResourceLoader.Load<PackedScene>(filename));
        }

        //Option 2: iterate through the lines of a config textfile to build the map of accessible levels
        //TODO
    }
}