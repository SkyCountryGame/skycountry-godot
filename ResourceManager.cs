using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Godot;

/* EntityManager? ObjectManager? GameManager? */
public class ResourceManager {
    public static PackedScene indicator; 
    public static Dictionary<Node3D, WorldObjectInfo> worldObjInfo = new Dictionary<Node3D, WorldObjectInfo>(); //associate each object in the game with info about it

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

}

public enum ObjectType {Entity, Prop, Structure, Item, Enemy, Friendly, Neutral};

public struct WorldObjectInfo{
    public ObjectType type;
}