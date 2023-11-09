using Godot;
using System;

public partial class level0 : Node3D
{
	private Player player; //so that we can tell it to walk somewhere. TODO this should probably be done through some event handler

	private Camera3D cam;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player =  GetNode<Player>("../../../Player");
		cam = GetNode<Camera3D>("../../../TwistPivot/PitchPivot/Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	//perhaps all input should be handled here in this toplevel node, and then passed to the appropriate objects. at least this class should hold some type of INputManager
	public override void _Input(InputEvent @event){
		//TODO make InputManager class
		if (Input.IsActionPressed("player_action2"))
		{ //set destination
			//InputEventMouseButton mEvent = ((InputEventMouseButton)@event);
			//mEvent.Position;
		}

		if (@event is InputEventMouseButton ev && ev.ButtonIndex == MouseButton.Left && ev.IsReleased())
		{
			Vector2 screenPos = ev.Position;
			float rayL = 1000f;
			Vector3 rayStart = cam.ProjectRayOrigin(screenPos);
			Vector3 rayEnd = rayStart + cam.ProjectRayNormal(screenPos) * rayL;

			Vector3 p = NavigationServer3D.MapGetClosestPointToSegment(GetWorld3D().NavigationMap, rayStart, rayEnd);
			player.SetTravelDestination(p);
		}
		
	}
	
}
