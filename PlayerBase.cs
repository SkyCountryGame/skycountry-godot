using Godot;
using System;

public partial class PlayerBase : Marker3D
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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		//would register controller if integrated with my architecture

		navAgent.PathDesiredDistance = .3f;
		navAgent.TargetDesiredDistance = .3f;
		Callable.From(Setup).CallDeferred();
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void SetTravelDestination(Vector3 pos)
	{
		navAgent.TargetPosition = pos;
	}
}
