using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EffectsManager {
    public static Dictionary<Node3D, Dictionary<string, Label3D>> floatingTextNodes = new Dictionary<Node3D, Dictionary<string, Label3D>>();

    public static void SpawnFloatingText(string key, string text, Node3D obj, Vector3 offset, int duration = 2000){

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
            System.Threading.Thread.Sleep(duration);
            obj.CallDeferred("remove_child", floatingTextNodes[obj][key]);
            floatingTextNodes[obj][key].QueueFree();
            floatingTextNodes[obj].Remove(key);
        });
    }

    public static void MarkerPoint(string key, Vector3 pos, int duration = 2000){
        Node3D markerNode = (Node3D) Global.prefabs["MarkerPoint"].Instantiate();
        markerNode.Position = pos;
        Global.level.AddChild(markerNode);

        //set the timer to remove this floating text after spcified duration
        Task.Run(() => {
            System.Threading.Thread.Sleep(duration);
            Global.level.CallDeferred("remove_child", markerNode);
            markerNode.QueueFree();
        });
    }

    //highlight mouse-over

    //spawn particles

    //
}