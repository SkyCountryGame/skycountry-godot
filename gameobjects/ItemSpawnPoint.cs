using Godot;
public partial class ItemSpawnPoint : Node3D
{
	[Export]
	PackedScene itemToSpawn = null;
	[Export]
	FlagResource flagToCheck;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.RegisterGameObject(this, GameObjectType.SpawnPoint);
		if(itemToSpawn!=null && flagToCheck.compareFlags()){
			AddChild(itemToSpawn.Instantiate());
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
