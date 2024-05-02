using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Godot;

/* EntityManager? ObjectManager? GameManager? 
    important idea here is to associate each game object with its godot node, to keep our code not dependent on game engine
*/
public class ResourceManager {
    public static PackedScene indicator; 


    //associate each godot node with the actual game object in the context of this game
    public static Dictionary<Node, WorldObjectInfo> worldObjInfo = new Dictionary<Node, WorldObjectInfo>(); 

    public static HashSet<Node> interactables = new HashSet<Node>(); //interactable objects in the game
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

    public static WorldObjectInfo GetWorldObject(Node obj){
        //TODO might not be necessary
        return worldObjInfo[obj];
    }

    public static void RegisterGameObject(GameObjectConnector goc){

    }

    //traverse up the node tree to see if this is an interactable. TODO might need to make sure to stop at some point if the node tree goes all the way up to level
    public static Interactable GetInteractable(Node obj){
        Node n = obj.GetParent();
		if (interactables.Contains(n)){
            return (Interactable) n;
        }
        while (n.GetParent() != null){
			n = n.GetParent();
            if (interactables.Contains(n)){
                return (Interactable) n;
            }
		}
        return null;
    }

}

public enum ObjectType {Entity, Prop, Structure, Item, Enemy, Friendly, Neutral};


//NOTE: currently experimenting with different ways to represent this stuff
public struct WorldObjectInfo{
    public ObjectType type;
}