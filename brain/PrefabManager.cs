using Godot;
using System;
using System.Collections.Generic;

/**
 * holds associations between prefabs (PackedScenes and their filepaths) and any nodes instantiated from them
 * this includes levels and gameobject
 * TODO how are the resources that are used for some nodes handled
    */
public partial class PrefabManager : Node {
    
    public Dictionary<string, PackedScene> prefabs; //prefabs are just PackedScenes that are used to instantiate game objects
    public Dictionary<PackedScene, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it)
    
    public override void _Ready(){
        //Option 1: iterate through the scene files in the folder for the level
        //string[] scenefilepaths = System.IO.Directory.GetFiles(levelname);
        //foreach (string filename in scenefilepaths){
        //    if (filename.EndsWith(".tscn")){

        //    }
        //    levelScenesList.Add(ResourceLoader.Load<PackedScene>(filename));
        //}

        //Option 2: iterate through the lines of a config textfile to build the map of accessible levels
        //TODO

        prefabs = new Dictionary<string, PackedScene>();
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://gameobjects/lamppost.tscn"));
        prefabs.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://gameobjects/floatingtext.tscn"));
        prefabs.Add("ERROR", ResourceLoader.Load<PackedScene>("res://gameobjects/error.tscn"));
        prefabs.Add("Player", ResourceLoader.Load<PackedScene>("res://player/player.tscn"));
        prefabs.Add("PauseMenu", ResourceLoader.Load<PackedScene>("res://ui/pause_menu.tscn"));
    }

    public void SpawnObject(string obj, Vector3 position){
        if (prefabs.ContainsKey(obj) && prefabs[obj] != null){
            Node3D node = (Node3D) prefabs[obj].Instantiate();
            node.Position = position;
            node.Name = obj;
            prefabs[obj].Instantiate();
        }
    }

    public void HandleEvent(Event e)
    {
        GD.Print($"SceneManager handling event {e.eventType.ToString()}");
    }

    public Node Instantiate(string label){
        if (prefabs.ContainsKey(label) && prefabs[label] != null){
            Node node = prefabs[label].Instantiate();
            return node;
        }
        return null;
    }
}