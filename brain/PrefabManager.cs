using Godot;
using System;
using System.Collections.Generic;

/**
 * holds associations between prefabs (PackedScenes and their filepaths) and any nodes instantiated from them
 * this includes levels and gameobject
 * TODO how are the resources that are used for some nodes handled
 * NOTE: this could go in Global, but i'm foreseeing that we'll want to do some specific management of packedsenses and their nodes etc.
    */
public partial class PrefabManager {
    
    public Dictionary<string, PackedScene> prefabs; //prefabs are just PackedScenes that are used to instantiate game objects
    public Dictionary<string, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it), for objects of which there can be multiple
    public Dictionary<string, Node> mapPackedSceneToSingleNode; //assoc packed scenes with the single node that was instantiated from it, for objects of which there can only be one, like a pausemenu or a boss
    //TODO make these 3 variables into a Prefab class for better encapsulation. e.g. rather than 3 HashMaps we have one HashMap<string, Prefab> 
    
    public PrefabManager(){
        //level loading possibilities
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
        prefabs.Add("MarkerPoint", ResourceLoader.Load<PackedScene>("res://gameobjects/markerpoint.tscn"));
    }

    public void SpawnObject(string obj, Vector3 position){
        if (prefabs.ContainsKey(obj) && prefabs[obj] != null){
            Node3D node = (Node3D) prefabs[obj].Instantiate();
            node.Position = position;
            node.Name = obj;
            prefabs[obj].Instantiate();
        }
    }

    //get an already instantiated node if it exists, otherwise instantiate one and return the new node
    public Node GetNode(string label, bool forceNew = false){
        if (prefabs.ContainsKey(label) && prefabs[label] != null){
            if (mapPackedSceneToSingleNode.ContainsKey(label)){
                return mapPackedSceneToSingleNode[label];
            } else if (forceNew) {
                Node node = prefabs[label].Instantiate();
                mapPackedSceneToSingleNode.Add(label, node);
                return node;
            }
        }
        return null;
    }

    //get all nodes instantiated from a packed scene for objects that can have multiple instances. optionally create a new one if none exist
    public List<Node> GetNodes(string label, bool forceNew = false){
        if (prefabs.ContainsKey(label)){
            if (mapPackedSceneToNodes.ContainsKey(label)){
                return mapPackedSceneToNodes[label];
            } else if (forceNew) {
                Node node = prefabs[label].Instantiate();
                List<Node> nodes = new List<Node>(){node};
                return nodes;
            }
        }
        return null;
    }

    public Node Instantiate(string label){
        if (prefabs.ContainsKey(label) && prefabs[label] != null){
            Node node = prefabs[label].Instantiate();
            return node;
        }
        return null;
    }
}