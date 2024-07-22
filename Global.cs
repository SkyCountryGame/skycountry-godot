using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Godot;

/* EntityManager? ObjectManager? GameManager? 
    things that need to be accessible anywhere in the codebase.
    hashmaps to associate godot stuff with game stuff, and more. 
*/
public class Global {
    public static PackedScene indicator; 

    /**
    * the current player data, to persist between scenes
    */
    public static PlayerModel _P;

    //associate each godot node with the actual game object in the context of this game
    public static Dictionary<Node, GameObject> gameObjects = new Dictionary<Node, GameObject>(); 
    public static HashSet<Interactable> interactables = new HashSet<Interactable>(); //interactable objects in the game
    public static HashSet<SpawnPoint> spawnPoints = new HashSet<SpawnPoint>();
    public static Dictionary<GameObject, Interactable> mapGameObjectToInteractable = new Dictionary<GameObject, Interactable>();
    //NOTE: this might end up being a map, because we wont have interactables implemented by godot nodes, but by game objects

    //effects (such as floating text) that follow an in-game object
    //public static ConcurrentDictionary<string, (Node3D, Vector3)> followers = new ConcurrentDictionary<string, (Node3D, Vector3)>();

    //in-game objects that are following other objects. usually floating text above something. 
    //public static Dictionary<Node3D, List<Node3D>> followers = new Dictionary<Node3D, List<Node3D>>();
    
    //key = parent object
    //value = dict of floating text strings (key -> label3d)
    public static Dictionary<Node3D, Dictionary<string, Label3D>> floatingTextNodes = new Dictionary<Node3D, Dictionary<string, Label3D>>();
    //NOTE: possible naming standard for collections of nodes? 

    public static void SpawnFloatingText(string key, string text, Node3D obj, Vector3 offset, float duration = 2.0f){

        if (!floatingTextNodes.ContainsKey(obj)){
            floatingTextNodes.Add(obj, new Dictionary<string, Label3D>());
        } else {
            offset.Y += floatingTextNodes[obj].Count;
        }
        if (floatingTextNodes[obj].ContainsKey(key)){
            floatingTextNodes[obj][key].Text = text;
        } else {
            Label3D textObj = (Label3D) ResourceLoader.Load<PackedScene>("res://floatingtext.tscn").Instantiate();
            textObj.Text = text;
            textObj.Position = offset;
            obj.AddChild(textObj);
            floatingTextNodes[obj].Add(key, textObj);
        }

        //set the timer to remove this floating text after spcified duration
        Task removeText = new Task(() => {
            System.Threading.Thread.Sleep(2000);
            obj.RemoveChild(floatingTextNodes[obj][key]);
            floatingTextNodes[obj][key].QueueFree();
            floatingTextNodes[obj].Remove(key);
        });
        removeText.Start();
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
                spawnPoints.Add((SpawnPoint)node);
                break;
            default:
                break;
        }
    }
    public static void RegisterSpawnPoint(Node node){

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
}