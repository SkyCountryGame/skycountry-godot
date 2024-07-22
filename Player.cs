using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class Player : CharacterBody3D, Collideable, Interactor
{

	//MOVEMENT 
	public double gravity = -.3f;
	public const float JumpVelocity = 200;
	private bool jump = false;
	private Vector3 velocity = Vector3.Zero;
	private AnimationPlayer rollcurve; //function that defines vel during roll
	private Vector3 controlDir; //user-inputted vector of intended direction of player
	private const float accelScalar = 40f;
	private const float velMagnitudeMax = 16f; //approximate max velocity allowed
	public Vector3 camForward = Vector3.Forward; //forward vector of camera
	//INTERACTION STUFF
	private HashSet<Interactable> availableInteractables = new HashSet<Interactable>();

	//UI stuff
	private HUDManager HUD;

	//PLAYER STATE
	private PlayerModel p; //this is the player data that should be persisted between scenes
	private State activityState = State.DEFAULT;
	Dictionary<State, HashSet<State>> dS; //allowed state transitions, used when updating
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

	public override void _Ready()
	{
		base._Ready();
		if (Global._P == null){
			p = new PlayerModel(); //TODO what parameters to give here
			Global._P = p;
		} else {
			p = Global._P;
		}
		
		HUD = GetNode<HUDManager>("../HUD"); 
		ApplyFloorSnap();

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
		State prev = activityState; //some states need to know previous
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

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		//desired direction of player movement is based on user input and the current orientation of the camera
		controlDir = new Vector3(Input.GetAxis("left", "right"), 0, Input.GetAxis("forward", "backward")).Normalized()
						.Rotated(Vector3.Down, -Mathf.Acos(camForward.Dot(Vector3.Forward)));
		
		Vector3 gv = controlDir * velMagnitudeMax; //goal velocity based on user input
		Vector3 accel = (gv - Velocity).Normalized() * accelScalar; //accelerate towards desired velocity
		if (controlDir.Length() == 0 && Velocity.Length() < 0.01f){
			velocity = Vector3.Zero;
			accel = Vector3.Zero;
		} else if (Velocity.Length() > gv.Length()) {
			velocity = gv;
		}
		if (Velocity.Length() < velMagnitudeMax){
			velocity += accel * (float)delta;
		}
		if (jump){
			velocity.Y += JumpVelocity;
			//Velocity += new Vector3(0, JumpVelocity, 0);
			jump = false;
		} else if (IsOnFloor()) {
			velocity.Y = 0; 
		} else {
			velocity.Y += (float) (gravity * delta) * 300 - 20;
		}
		
		/*velocity = Vector3.Zero;  //old implementation without acceleration
		velocity.X += (float)(Input.GetAxis("left", "right")*500*delta);
		velocity.Z += (float)(Input.GetAxis("forward", "backward")*500*delta);
		*/

		//velocity = velocity.Normalized() * 500 * (float)delta;
		Velocity = velocity;
		MoveAndSlide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent ev){
		if (Input.IsActionJustPressed("player_action2")){

		} else if (Input.IsActionJustReleased("player_jump")) //TODO implement charge-up later
		{
			jump = true;
			p.hp += 1; //TODO remove. this is just to show that state is persisted
		} else if (Input.IsActionJustPressed("player_use")){
			switch (activityState){
				case State.DEFAULT:
					Interactable i = GetFirstInteractable();
					if (i != null)
					{
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
		} else if (Input.IsKeyPressed(Key.R)){
			Position += new Vector3(0, .2f, 0);
		} else if (Input.IsKeyPressed(Key.F)){
			Position += new Vector3(0, -.2f, 0);
		}
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
		Global.SpawnFloatingText("collision"+other.GetHashCode(), other.Name, this, new Vector3(0,3,0));

		switch (zone){
			case ColliderZone.Awareness0:
				Interactable i = Global.GetInteractable(other);
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
		Interactable i = Global.GetInteractable(other);
		if (availableInteractables.Contains(i))
		{
			availableInteractables.Remove(i);
		}
	}

	//set the player's forward vector
	public void SetForward(Vector3 f){
	
		// Normalize the input vector
		camForward = f.Normalized();
		
		// Calculate the angle between the current forward vector and the new forward vector
		//float angle = Mathf.Acos(GlobalTransform.Basis.Z.Dot(f));
		// Rotate the player's transform around the Y-axis by the calculated angle
		//GlobalTransform = GlobalTransform.Rotated(Vector3.Up, angle);
	
	}
}
