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
            Label3D textObj = (Label3D) PrefabManager.prefabs["FloatingText"].Instantiate();
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
        Node3D markerNode = (Node3D) PrefabManager.prefabs["MarkerPoint"].Instantiate();
        markerNode.Position = pos;
        Global.currentLevel.AddChild(markerNode);

        //set the timer to remove this floating text after spcified duration
        Task.Run(() => {
            System.Threading.Thread.Sleep(duration);
            Global.currentLevel.CallDeferred("remove_child", markerNode);
            markerNode.QueueFree();
        });
    }

    //apply a flash of color on some meshes
    public static void Flash(List<MeshInstance3D> meshes, Color color, float duration = 0.5f){
        foreach (MeshInstance3D mesh in meshes){
            Flash(mesh, color, duration);
        }
    }

    public static void Flash(MeshInstance3D mesh, Color color, float duration = 0.2f){
        Task.Run(()=>{
            StandardMaterial3D mat = (StandardMaterial3D) mesh.GetActiveMaterial(0); //assuming only one surface 
            Color ogColor = mat.AlbedoColor;
            float ogEmissionIntensity = mat.EmissionIntensity;
            Color ogEmission = mat.Emission;
            bool ogEmissionEn = mat.EmissionEnabled;
            if (mesh.IsInsideTree() && mesh.IsNodeReady()){
                mat.AlbedoColor = color; //TODO interpolate
                mat.EmissionEnabled = true;
                mat.Emission = color;
                mat.EmissionIntensity = 2;
            }
            System.Threading.Thread.Sleep((int) (duration * 1000));
            if (mesh.IsInsideTree() && mesh.IsNodeReady()){
                mat.AlbedoColor = ogColor;
                mat.Emission = ogEmission;
                mat.EmissionIntensity = ogEmissionIntensity;
                mat.EmissionEnabled = ogEmissionEn;
            }
            
        });
    }

    //highlight mouse-over

    //spawn particles

    //
}