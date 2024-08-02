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

    /** ======= GLOBAL STATIC REFERENCES ========
    * current player data, to persist between scenes
    * NOTE i want to use a shorter name other than "global", like "G" or "__" 
    */
    public static PlayerModel PlayerModel;
    public static Player PlayerNode;
    public static Camera2 Cam;
    public static SceneTree SceneTree;
    public static HUDManager HUD; //TODO maybe these should be set via functions so that memory can be freed if prev existed 


    
    
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
            Label3D textObj = (Label3D) SceneManager._.prefabs["FloatingText"].Instantiate();
            textObj.Text = text;
            textObj.Position = offset;
            obj.AddChild(textObj);
            floatingTextNodes[obj].Add(key, textObj);
        }

        //set the timer to remove this floating text after spcified duration
        Task removeText = new Task(() => {
            System.Threading.Thread.Sleep(2000);
            obj.CallDeferred("remove_child", floatingTextNodes[obj][key]);
            floatingTextNodes[obj][key].QueueFree();
            floatingTextNodes[obj].Remove(key);
        });
        removeText.Start();
    }
}