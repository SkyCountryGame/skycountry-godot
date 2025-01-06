using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/**
 * holds associations between prefabs (PackedScenes and their filepaths) and any nodes instantiated from them
 * this includes levels and gameobjects (thus maybe should be renamed to NodeManager or SceneManager)
 * TODO how are the resources that are used for some nodes handled
 * NOTE: this could go in Global, but i'm foreseeing that we'll want to do some specific management of packedsenses and their nodes etc.
    */
public partial class PrefabManager {
    
    public static Dictionary<string, PackedScene> prefabs = new Dictionary<string, PackedScene>(); //prefabs are just PackedScenes that are used to instantiate game objects
    public static Dictionary<string, string> tscnPaths = new Dictionary<string, string>(); //map nodes (by name) to filepath of tscn file that represents them. this is for quick access to filepath for loading the PackedScene if needed
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
       LoadAllPrefabs();
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

    //cache all prefabs into memory on another thread. in future might only load relevant prefabs
    public static void LoadAllPrefabs(){
        GD.Print("loading all prefabs");
        Task.Run(() => {
            try {
                string[] filepaths = System.IO.Directory.GetFiles("./tscn", "*.tscn", System.IO.SearchOption.AllDirectories);
                GD.Print($"found {filepaths.Length} prefabs");
                foreach (string fp in filepaths){
                    string label = Path.GetFileNameWithoutExtension(fp);
                    GD.Print($"loading {fp}; {label}");
                    prefabs[label] = ResourceLoader.Load<PackedScene>($"res://{fp}");
                }
                filepaths = System.IO.Directory.GetFiles("./levels", "*.tscn", System.IO.SearchOption.AllDirectories);
                foreach (string fp in filepaths){
                    string label = Path.GetFileNameWithoutExtension(fp);
                    GD.Print($"loading {fp}; {label}");
                    prefabs[label] = ResourceLoader.Load<PackedScene>($"res://{fp}");
                }
            } catch (Exception e){
                GD.Print(e);
            }
        });
    }

    //load some prefabs from a list of filepaths
    public static void LoadPrefabs(string[] tscnFilepaths){
        foreach (string path in tscnFilepaths){
            GD.Print("loading "+path);
            if (path.StartsWith("res://")){
                prefabs[Path.GetFileNameWithoutExtension(path)] = ResourceLoader.Load<PackedScene>(path);	
            } else {
                prefabs[Path.GetFileNameWithoutExtension(path)] = ResourceLoader.Load<PackedScene>("res://"+path);
            }
        }
    }

    //will scan the filesystem to find the tscn file for this node. 
    //only pass nodes that have a tscn file associated with them
    public static void AddFromNode(Node node, string label = null){
        if (label == null){
            label = node.Name;
        }
        if (tscnPaths.ContainsKey(label)){
            //prefabs[label] = ResourceLoader.Load<PackedScene>(tscnPaths[node]);
        }
    }

    ////scans the given directory recursively to find a tscn file named with given name, optional root directory, defaults to project root
    public static string FindSceneFile(string name, string dir="./"){
        if (dir.StartsWith("res://")){
            dir = dir.Substring(6);
        }
        if (!name.EndsWith(".tscn")){
            name = $"{name}.tscn";
        }
        string[] filepaths = System.IO.Directory.GetFiles(dir, $"{name}*", System.IO.SearchOption.AllDirectories);
        if (filepaths.Length > 1){
            GD.PushWarning($"Found multiple tscn files with name {name}. That's probably not good.\n {filepaths}");
        }
        if (filepaths.Length > 0) {
            foreach (string fp in filepaths){
                if (ResourceLoader.Exists($"res://{fp}")){
                    tscnPaths[name] = $"res://{fp}";
                    return fp;
                }
            }
        }
        return null;
    }
}