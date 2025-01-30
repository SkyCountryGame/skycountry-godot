using Godot;
using System;

public partial class SpawnPoint : Node3D
{
	[Export] private GameObjectType typeToSpawn;
	[Export] private Resource resourceToSpawn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		switch (typeToSpawn){
			case GameObjectType.Player:
				if (Global.playerNode == null){
					Global.playerNode = (Player) PrefabManager.Get("Player").Instantiate();
					GetParent().CallDeferred("add_child", Global.playerNode);
				} else {
					Global.playerNode.CallDeferred("reparent", GetParent());
				}
				Global.playerNode.GlobalTransform = GlobalTransform;
				QueueFree();	
				break;
			case GameObjectType.Enemy:
				Enemy e = (Enemy) PrefabManager.Get("Enemy").Instantiate();
				GetParent().CallDeferred("add_child", e);
				e.GlobalTransform = GlobalTransform;
				break;
			case GameObjectType.WorldItem:
				
				break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
