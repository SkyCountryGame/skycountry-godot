using Godot;
using System.Collections.Generic;
public class GameObjectManager {

    public static Dictionary<string, PackedScene> gameObjectsPacked;
    
    
    private static List<StaticBody3D> floor;

    public static void init(){
        gameObjectsPacked = new Dictionary<string, PackedScene>();
        floor = new List<StaticBody3D>();
        gameObjectsPacked.Add("Rock", ResourceLoader.Load<PackedScene>("res://interact/rock.tscn"));
        //gameObjects.Add("LampPost", ResourceLoader.Load<PackedScene>("res://entity/lamppost.tscn"));
        gameObjectsPacked.Add("Enemy", ResourceLoader.Load<PackedScene>("res://entity/enemy.tscn"));
        gameObjectsPacked.Add("FloatingText", ResourceLoader.Load<PackedScene>("res://floatingtext.tscn"));
    }

    public static void SpawnObject(string obj, Vector3 position){
        if (gameObjectsPacked.ContainsKey(obj) && gameObjectsPacked[obj] != null){
            Node3D node = (Node3D) gameObjectsPacked[obj].Instantiate();
            node.Position = position;
            node.Name = obj;
            Global._SceneTree.Root.AddChild(node);
            gameObjectsPacked[obj].Instantiate();
        }
    }

    public static void SetFloor(List<StaticBody3D> floors){
        GameObjectManager.floor.Clear();
        foreach (var f in floors){
            GameObjectManager.floor.Add(f);
        }
    }

    public static void probe(){
        GD.Print("ok");
    }
}