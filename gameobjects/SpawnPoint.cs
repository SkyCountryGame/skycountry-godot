using Godot;
using System;


public partial class SpawnPoint : Node3D
{

	[Export]
	public int radius = 1; //radius around point within which the thing can spawn

	public PackedScene thing; //thing to spawn

	public override void _Ready()
	{
		SceneManager.RegisterGameObject(this, GameObjectType.SpawnPoint);
	}

	public override void _Process(double delta)
	{
	}
}
