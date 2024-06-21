using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : CharacterBody3D, Collideable, Interactor
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private HashSet<Interactable> availableInteractables = new HashSet<Interactable>();

	[Export]
	private HUDManager HUD;

	private NavigationAgent3D navAgent;

	public Vector3 navTargetPos = new Vector3(3, 0, 1); //where go
	
	private MovementType movementType = MovementType.WASD;

	private State activityState = State.DEFAULT;
	Dictionary<State, HashSet<State>> dS; //allowed state transitions, used when updating

	public enum MovementType
	{
		WASD,
		Mouse
	}
	private enum State //maybe activity state? 
	{
		DEFAULT, 
        CHARGING, //preparing to roll
        ROLLING, 
        PREPARING, //preparing to attack 
        ATTACKING,
        COOLDOWN,
        HEALING,
        RELOADING,
        AIMING, //this could be different instance flag
        INVENTORY, //in an inventory menu
        DIALOGUE
	}
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
		HUD = GetNode<HUDManager>("../HUD");
		//would register controller if integrated with my architecture

		navAgent.PathDesiredDistance = .3f;
		navAgent.TargetDesiredDistance = .3f;
		Callable.From(Setup).CallDeferred();

		dS = new Dictionary<State, HashSet<State>>();
        dS.Add(State.DEFAULT, new HashSet<State>() {State.CHARGING, State.HEALING, State.PREPARING, State.RELOADING, State.AIMING, State.INVENTORY, State.DIALOGUE});
        dS.Add(State.CHARGING, new HashSet<State>() { State.ROLLING, State.DEFAULT });
        dS.Add(State.ROLLING, new HashSet<State>() { State.DEFAULT });
        dS.Add(State.PREPARING, new HashSet<State>() { State.ATTACKING, State.DEFAULT });
        dS.Add(State.ATTACKING, new HashSet<State>() { State.COOLDOWN });
        dS.Add(State.RELOADING, new HashSet<State>() { State.DEFAULT, State.HEALING, State.AIMING });
        dS.Add(State.COOLDOWN, new HashSet<State>() { State.DEFAULT, State.HEALING, State.RELOADING, State.CHARGING, State.AIMING });
        dS.Add(State.AIMING, new HashSet<State>() { State.DEFAULT, State.ATTACKING, State.HEALING, State.COOLDOWN });
        dS.Add(State.INVENTORY, new HashSet<State>() { State.DEFAULT });
        dS.Add(State.DIALOGUE, new HashSet<State>() { State.DEFAULT });
	}

	private bool UpdateState(State ps){
		State prev = ps; //some states need to know previous
		if (dS[activityState].Contains(ps)){
			activityState = ps;
			switch (activityState){
				case State.DEFAULT:
					break;
				case State.CHARGING:
					break;
				case State.ROLLING:
					break;
				case State.PREPARING:
					break;
				case State.ATTACKING:
					break;
				case State.COOLDOWN:
					break;
				case State.HEALING:
					break;
				case State.RELOADING:
					break;
				case State.AIMING:
					break;
				case State.INVENTORY:
					break;
				case State.DIALOGUE:
					break;
			}
		} else {
			return false;
		}
		return true;
	}

	private async void Setup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		navTarget = navTargetPos;
	}
	public override void _PhysicsProcess(double delta)
	{
		GD.Print(movementType.ToString());
		if (movementType == MovementType.WASD)
		{
			var velocity = Vector3.Zero;
			velocity.X += Input.GetAxis("left", "right");
			velocity.Z += Input.GetAxis("forward", "backward");
			velocity = velocity.Normalized() * 500 * (float)delta;
			Velocity = velocity;
			GD.Print(velocity);
			MoveAndSlide();
		}
		else if (movementType == MovementType.Mouse)
		{
			base._PhysicsProcess(delta);
			if (navAgent.IsNavigationFinished()) return;
			float dt = (float)delta;

			Vector3 nextPathPos = navAgent.GetNextPathPosition();
			Vector3 curPos = GlobalTransform.Origin;
			Vector3 newVel = (nextPathPos - curPos).Normalized() * 10;
			GlobalPosition = GlobalPosition.MoveToward(nextPathPos, dt * 10);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent ev){
		if (Input.IsActionJustPressed("player_action2"))
		{ //set destination
			//InputEventMouseButton mEvent = ((InputEventMouseButton)@event);
			//mEvent.Position;
		} else if (Input.IsActionJustPressed("player_use")){
			switch (activityState){
				case State.DEFAULT:
					Interactable i = GetFirstInteractable();
					if (i != null)
					{
						//was for dbging HUD.LogEvent($"player use {((Node)i).Name}");
						dynamic payload = i.Interact();
						HandleInteract(i, (Node)i, payload);
					} else {
						HUD.LogEvent("there is nothing with which to interact");
					}
					break;
				case State.DIALOGUE:
					//TODO continue dialogue. get the next words from the current talker, whether it be from him or a request for response from player
					break;
			}
		} else if (Input.IsActionJustPressed("pause"))
		{
			
		} else if (Input.IsActionJustPressed("ui_back")){
			if (activityState == State.DIALOGUE){
				HUD.HideDialogue();
				UpdateState(State.DEFAULT);
			}
		}
	}

	public void SetTravelDestination(Vector3 pos)
	{
		navAgent.TargetPosition = pos;
	}

	public void HandleInteract(Interactable i, Node interactionObj, dynamic payload)
	{
		switch (i.interactionType)
		{
			case InteractionType.Dialogue:
				UpdateState(State.DIALOGUE);
				HUD.ShowDialogue($"{payload}"); //TODO name of talker
				break;
			case InteractionType.Inventory:
				break;
			case InteractionType.Pickup:
				break;
			case InteractionType.General:
				break;
			case InteractionType.Mineable:
				break;
			default:
				break;
		}
	}

	public Interactable GetFirstInteractable()
	{
		if (availableInteractables.Count > 0)
		{
			return availableInteractables.First();
		}
		return null;
	}

	public void HandleCollide(ColliderZone zone, Node other)
	{
		//TODO get the actual text to spawn
		ResourceManager.SpawnFloatingText("collision"+other.GetHashCode(), other.Name, this, new Vector3(0,3,0));

		switch (zone){
			case ColliderZone.Awareness0:
				Interactable i = ResourceManager.GetInteractable(other);
				if (i != null)
				{
					availableInteractables.Add(i);
				}
				break;
			case ColliderZone.Awareness1:
				break;
			case ColliderZone.Body:
				break;
		}
	}

	public void HandleDecollide(ColliderZone zone, Node other)
	{
		//TODO figure out a better way to handle collision zones of interactables instead of allows traversing up tree
		Interactable i = ResourceManager.GetInteractable(other);
		if (availableInteractables.Contains(i))
		{
			availableInteractables.Remove(i);
		}
	}
}
