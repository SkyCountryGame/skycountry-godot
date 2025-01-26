using Godot;
using System;

//trigger an event when player enter zone
public partial class EventTriggerZone : Area3D
{
    [Export] private EventType eventType;
    
	public override void _Ready()
	{
		BodyEntered += (body) => OnBodyEntered(body); 
	}

	private void OnBodyEntered(Node3D body)
	{
		if(body==Global.playerNode){
			EventManager.Invoke(new Event());
		}
	}
}