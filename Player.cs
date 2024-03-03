using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	private NavigationAgent3D navAgent;

	public Vector3 navTargetPos = new Vector3(3, 0, 1); //where go

	public Vector3 navTarget
	{
		get { return navAgent.TargetPosition;  }
		set { navAgent.TargetPosition = value; }
	}
	
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
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (navAgent.IsNavigationFinished()) return;
		float dt = (float)delta;

		Vector3 nextPathPos = navAgent.GetNextPathPosition();
		Vector3 curPos = GlobalTransform.Origin;
		Vector3 newVel = (nextPathPos - curPos).Normalized() * 10;
		GlobalPosition = GlobalPosition.MoveToward(nextPathPos, dt * 10);
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
	public void SetTravelDestination(Vector3 pos)
	{
		//navTargetPos = pos;
		navAgent.TargetPosition = pos;
	}
}
