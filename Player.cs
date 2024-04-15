using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Marker3D, Collideable
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private List<Interactable> availableInteractables = new List<Interactable>();
	
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

	public override void _UnhandledInput(InputEvent ev){
		if (Input.IsActionPressed("player_action2"))
		{ //set destination
			//InputEventMouseButton mEvent = ((InputEventMouseButton)@event);
			//mEvent.Position;
		} else if (Input.IsActionPressed("player_use")){
			GD.Print("player use");
			Interactable i = GetFirstInteractable();
			if (i != null)
			{
				dynamic payload = i.Interact();
				HandleInteract((Node)i, payload);
			}
		} else if (Input.IsActionPressed("pause"))
		{
			
		}
	}

	public void SetTravelDestination(Vector3 pos)
	{
		navAgent.TargetPosition = pos;
	}

	public void HandleInteract(Node interactionObj, dynamic payload)
	{

		throw new NotImplementedException();
	}

	public Interactable GetFirstInteractable()
	{
		return null;
	}

	public void HandleCollide(ColliderZone zone, Node other)
	{
		GD.Print($"player collide with {other.Name}, {zone}");
		ResourceManager.SpawnFloatingText("collision"+other.GetHashCode(), other.Name, this, new Vector3(0,3,0));
		//var indicator = ResourceLoader.Load<PackedScene>("res://assets/indicator.tscn").Instantiate();
		//AddChild(indicator);
	}

	public void HandleDecollide(ColliderZone zone, Node other)
	{
		throw new NotImplementedException();
	}
}
