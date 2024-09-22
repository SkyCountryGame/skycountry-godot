using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EffectsManager {
    public static Dictionary<Node3D, Dictionary<string, Label3D>> floatingTextNodes = new Dictionary<Node3D, Dictionary<string, Label3D>>();

    public static void SpawnFloatingText(string key, string text, Node3D obj, Vector3 offset, float duration = 2.0f){

        if (!floatingTextNodes.ContainsKey(obj)){
            floatingTextNodes.Add(obj, new Dictionary<string, Label3D>());
        } else {
            offset.Y += floatingTextNodes[obj].Count;
        }
        if (floatingTextNodes[obj].ContainsKey(key)){
            floatingTextNodes[obj][key].Text = text;
        } else {
            Label3D textObj = (Label3D) Global.prefabs["FloatingText"].Instantiate();
            textObj.Text = text;
            textObj.Position = offset;
            obj.AddChild(textObj);
            floatingTextNodes[obj].Add(key, textObj);
        }

        //set the timer to remove this floating text after spcified duration
        Task.Run(() => {
            System.Threading.Thread.Sleep(2000);
            obj.CallDeferred("remove_child", floatingTextNodes[obj][key]);
            floatingTextNodes[obj][key].QueueFree();
            floatingTextNodes[obj].Remove(key);
        });
    }

    //highlight mouse-over

    //spawn particles

    //
}