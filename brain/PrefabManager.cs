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
    
    public static Dictionary<string, PackedScene> prefabs = new Dictionary<string, PackedScene>(); //prefabs are just PackedScenes that are used to instantiate game objects
    public Dictionary<string, List<Node>> mapPackedSceneToNodes; //assoc packed scenes with all of its instantiated nodes (or nodes that have been instantiated from it), for objects of which there can be multiple
    public Dictionary<string, Node> mapPackedSceneToSingleNode; //assoc packed scenes with the single node that was instantiated from it, for objects of which there can only be one, like a pausemenu or a boss
    //TODO make these 3 variables into a Prefab class for better encapsulation. e.g. rather than 3 HashMaps we have one HashMap<string, Prefab> 

    //NOTE there is also all the separate nodes in a scenetree that comprise a gameobject. 
    
    //public PrefabManager(){
    public static void Init(){
        prefabs = new Dictionary<string, PackedScene>();
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://gameobjects/lamppost.tscn"));
        //prefabs.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://gameobjects/tscn/floatingtext.tscn"));
        //prefabs.Add("ERROR", ResourceLoader.Load<PackedScene>("res://gameobjects/tscn/error.tscn"));
        prefabs.Add("Player", ResourceLoader.Load<PackedScene>("res://player/player.tscn"));
        prefabs.Add("PauseMenu", ResourceLoader.Load<PackedScene>("res://ui/pause_menu.tscn"));
       // prefabs.Add("MarkerPoint", ResourceLoader.Load<PackedScene>("res://gameobjects/tscn/markerpoint.tscn"));
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

    //get a prefab. account for cases where prefab name doesn't have suffix of "_[pickup,equip]"
    public static PackedScene Get(string label){
        if (prefabs.ContainsKey(label)){
            return prefabs[label];
        }
        //this packedscene has not yet been loaded in runtime mem. so look for it on disk
        PackedScene p = null;
        string[] filepaths = System.IO.Directory.GetFiles("tscn", $"{label}.tscn", System.IO.SearchOption.AllDirectories);
        if (filepaths.Length == 0){ //handle the case where doesn't have suffix of pickup or equip
            filepaths = System.IO.Directory.GetFiles("tscn", $"{label.Split("_pickup")[0]}*.tscn");
        }
        if (filepaths.Length == 0){
            filepaths = System.IO.Directory.GetFiles("tscn", $"{label.Split("_equip")[0]}*.tscn");
        }
        if (filepaths.Length > 0) {
            foreach (string fp in filepaths){
                GD.Print($"found {fp}");    
                try {
                    p = ResourceLoader.Load<PackedScene>($"res://{fp}");
                    prefabs[label] = p;
                    return p;
                } catch (InvalidCastException e){
                    GD.PushError(e);
                    GD.PushError($"{label} tscn file invalid: {fp}");
                    continue;
                }
            }
        }
        /*
        if (ResourceLoader.Exists($"res://prefabs/{label}.tscn")){
            p = ResourceLoader.Load<PackedScene>($"res://prefabs/{label}.tscn");
            prefabs[label] = p;
        } else if (ResourceLoader.Exists($"res://prefabs/{label.Split("_pickup")[0]}/{label}.tscn")) {
            p = ResourceLoader.Load<PackedScene>($"res://prefabs/{label.Split("_pickup")[0]}/{label}.tscn");
            prefabs[label] = p;
        } else if (ResourceLoader.Exists($"res://prefabs/{label.Split("_equip")[0]}/{label}.tscn")) {
            p = ResourceLoader.Load<PackedScene>($"res://prefabs/{label.Split("_equip")[0]}/{label}.tscn");
            prefabs[label] = p;
        } */ 
        return p;
    }
}