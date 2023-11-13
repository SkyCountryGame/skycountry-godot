using Godot;
using System;
using System.Numerics;
using SkyCountry;
using Vector3 = Godot.Vector3;

//currently this is the only script attached to the player, so it is acting as the playercontroller. 
//the reason it's a RigidBody3D is because that's what the root scenenode of player is
public partial class Player : RigidBody3D//, InputActionListener
{
	private NavigationAgent3D navAgent;

	public Vector3 navTargetPos = new Vector3(3, 0, 1); //where go

	public Vector3 navTarget
	{
		get { return navAgent.TargetPosition;  }
		set { navAgent.TargetPosition = value; }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		//would register controller if integrated with my architecture

		navAgent.PathDesiredDistance = .3f;
		navAgent.TargetDesiredDistance = .3f;
		Callable.From(Setup).CallDeferred();
		/*navAgent.VelocityComputed += (Vector3 v) =>
		{
			navAgent.TargetPosition = navTarget;
		};*/
		
		//CallDeferred("NavigationSetup");
	}
	
	private async void Setup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		navTarget = navTargetPos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (navAgent.IsNavigationFinished()) return;
		float dt = (float)delta;

		Vector3 nextPathPos = navAgent.GetNextPathPosition();
		Vector3 curPos = GlobalTransform.Origin;
		Vector3 newVel = (nextPathPos - curPos).Normalized() * 10;
		LinearVelocity = newVel;
		GlobalPosition.MoveToward(nextPathPos, dt * 10);
		GD.Print($"newvel={newVel}");
		//ApplyCentralForce();
		/*
		if (navAgent.AvoidanceEnabled)
		{
			navAgent.SetVelocityForced(newVel);
		}
		else
		{
				//https://docs.godotengine.org/en/stable/tutorials/navigation/navigation_using_navigationagents.html
		}*/
	}
	
	public override void _Input(InputEvent @event){
		//TODO make InputManager class
		if (Input.IsActionPressed("player_action2"))
		{ //set destination
			navAgent.TargetPosition = Position + new Vector3(2, 0, 2);
			//InputEventMouseButton mEvent = ((InputEventMouseButton)@event);
			//mEvent.Position;
		}
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		GD.Print($"{position}, {normal}, {shape_idx}");
	}

	public void SetTravelDestination(Vector3 pos)
	{
		navTargetPos = pos;
		//navAgent.TargetPosition = pos;
	}
	
	/*
	public void HandleActionEnable(InputEventAction type, bool en)
	{
		//in the future i might actually want to use my own InputAction enum for further decoupling from godot or any engine
		string t = type.AsText();
		switch (t)
		{
			case "left":
				break;
		}
	}

	public void HandleContinuousAction(InputEventAction a)
	{
		throw new NotImplementedException();
	}*/
}
