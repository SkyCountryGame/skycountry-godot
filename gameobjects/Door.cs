using Godot;
using System;

//teleportation mechanism. 
public partial class Door : Area3D
{
	[Export] private string destination; //where door leads to

	public override void _Ready()
	{
		if (destination == null){
			GD.PushWarning("no destination set for door");
		} 
		BodyEntered += (body) => OnBodyEntered(body); 
	}

	private void OnBodyEntered(Node3D body)
	{
		if(body==Global.playerNode){
			Global.ChangeLevel(destination);
		}
	}
}
