using Godot;
using System;

//teleportation mechanism. 
public partial class Door : Area3D
{
	[Export] private string destinationPath; //tscn file of where door leads to
	private string destinationLabel; //the key used to reference in prefabs hashmap

	public override void _Ready()
	{
		if (destinationPath == null){
			GD.PushWarning("no destination set for door");
		}
		destinationLabel = PrefabManager.LoadPrefab(destinationPath);
		BodyEntered += (body) => OnBodyEntered(body); 
	}

	private void OnBodyEntered(Node3D body)
	{
		if(body==Global.playerNode){
			Global.ChangeLevel(destinationLabel);
		}
	}
}
