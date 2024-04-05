using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

/* EntityManager? ObjectManager? GameManager? */
public class ResourceManager {
    public static PackedScene indicator; 
    public static Dictionary<Node3D, WorldObjectInfo> worldObjInfo = new Dictionary<Node3D, WorldObjectInfo>(); //associate each object in the game with info about it

    //effects (such as floating text) that follow an in-game object
    //public static ConcurrentDictionary<string, (Node3D, Vector3)> followers = new ConcurrentDictionary<string, (Node3D, Vector3)>();

    //the strings that are currently floating above and following objects
    public static Dictionary<Node3D, List<string>> currentFollowingText = new Dictionary<Node3D, List<string>>();
    
    public static void ShowText(string text, Node3D obj, Vector3 offset){
        if (!currentFollowingText.ContainsKey(obj)){
            currentFollowingText.Add(obj, new List<string>());
        }
        currentFollowingText[obj].Add(text);
        //TODO is it better to have multiple Label3Ds each with one of the texts, or one Label3D with all the texts? 
        //if (followers.ContainsKey(text)){
            //update the text
            //TODO
        //} else {
            Label3D textObj = (Label3D) ResourceLoader.Load<PackedScene>("res://floatingtext.tscn").Instantiate();
		    textObj.Text = text;
            textObj.Position = offset;
            obj.AddChild(textObj);
            //followers.TryAdd(text, (textObj, offset));
        //}

        Task removeText = new Task(() => {
            System.Threading.Thread.Sleep(2000);
            currentFollowingText[obj].Remove(text);
            //followers.TryRemove(text, out (Node3D, Vector3) value);
            //value.Item1.QueueFree();
        });
        removeText.Start();
    }

}

public enum ObjectType {Entity, Prop, Structure, Item, Enemy, Friendly, Neutral};

public struct WorldObjectInfo{
    public ObjectType type;
}