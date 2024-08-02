using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static PlayerModel;

public partial class Player : CharacterBody3D, Collideable, Interactor
{

	//MOVEMENT 
	public double gravity = -.3f;
	public const float JumpVelocity = 200;
	private bool jump = false;
	private Vector3 velocity = Vector3.Zero;
	private AnimationPlayer rollcurve; //function that defines vel during roll
	private Vector3 controlDir; //user-inputted vector of intended direction of player, adjusted for camera
	private Vector3 inputDir = new Vector3(); //user-inputted vector of intended direction of player
	private const float accelScalar = 90f;
	private const float velMagnitudeMax = 24f; //approximate max velocity allowed
	public Vector3 camForward = Vector3.Forward; //forward vector of camera

	//INTERACTION STUFF
	private HashSet<Interactable> availableInteractables = new HashSet<Interactable>();
	//UI stuff
	
	//PLAYER STATE
	private PlayerModel pm; //this is the player data that should be persisted between scenes. '_M' because shorthand

	public override void _Ready()
	{
		base._Ready();
		if (Global._PlayerModel == null){
			pm = new PlayerModel(this); //TODO what parameters to give here
			Global._PlayerModel = pm;
		} else {
			pm = Global._PlayerModel;
		}
		Global._PlayerNode = this; //while the playerMODEL will remain the same between scenes, the playerNODE could change
		ApplyFloorSnap();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		float angleToCam = camForward.SignedAngleTo(Vector3.Forward, Vector3.Up); //angle between control forward and camera forward
		//desired direction of player movement is based on user input and the current orientation of the camera
		/*controlDir = new Vector3(Input.GetAxis("left", "right"), 0, Input.GetAxis("forward", "backward")).Normalized()
							.Rotated(Vector3.Down, angleToCam);*/
		controlDir = inputDir.Normalized().Rotated(Vector3.Down, angleToCam);
		
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
			jump = false;
		} else if (IsOnFloor()) {
			velocity.Y = 0; 
		} else {
			velocity.Y += (float) (gravity * delta) * 300 - 20;
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	public override void _Process(double delta)
	{
		if (SceneManager._ != null && SceneManager._.currentLevelScene != GetTree().CurrentScene){
			SceneManager._.SetActiveLevelScene(GetTree().CurrentScene); //tell the level manager what scene we are in
		}

		//RayCast Stuff
		Vector2 mousePosition = GetViewport().GetMousePosition();
		Camera3D camera =  Global._Cam;
		Vector3 rayOrigin = camera.ProjectRayOrigin(mousePosition);
		Vector3 rayTarget = rayOrigin+camera.ProjectRayNormal(mousePosition)*100;
		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
		Godot.Collections.Dictionary intersection = spaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(rayOrigin, rayTarget,1));
		if(intersection.ContainsKey("position") && !intersection["position"].Equals(null)){
			Vector3 pos = (Vector3)intersection["position"];
			Vector3 viewAngle = new Vector3(pos.X, Position.Y, pos.Z);
			LookAt(viewAngle);
		}

		//HUD stuff
		if (!Global.HUD.actionLabel.Visible && availableInteractables.Count > 0){
			Global.HUD.ShowAction($"{GetFirstInteractable().Info()}");
		}
	}

	public override void _Input(InputEvent ev){
		
		//do appropriate thing whether we are in inventory or not
		if (pm.activityState == (State.DIALOGUE | State.INVENTORY)){
			if (Input.IsActionJustPressed("ui_back")){
				pm.UpdateState(State.DEFAULT);
			} else if (Input.IsActionJustPressed("left")){
				//TODO inv left
			} else if (Input.IsActionJustPressed("right")){
				//TODO inv right
			} else if (Input.IsActionJustPressed("up")){
				//TODO inv up
			} else if (Input.IsActionJustPressed("down")){
				//TODO inv down
			}
		} else {
			inputDir.X = Input.GetAxis("left", "right");
			inputDir.Z = Input.GetAxis("forward", "backward");
			if (Input.IsActionJustPressed("player_action2")){

			} else if (Input.IsActionJustReleased("player_jump")) //TODO implement charge-up later
			{
				jump = true;
			} else if (Input.IsActionJustPressed("player_use")){
				switch (pm.activityState){
					case State.DEFAULT:
						Interactable i = GetFirstInteractable();
						if (i != null)
						{
							if (i.interactionMethod == InteractionMethod.Use){
								HandleInteract(i, (Node)i);
							}
						} else {
							Global.HUD.LogEvent("there is nothing with which to interact");
						}
						break;
					case State.DIALOGUE:
						//TODO continue dialogue. get the next words from the current talker, whether it be from him or a request for response from player
						break;
				}
			} else if (Input.IsActionJustPressed("pause"))
			{
				//TODO pause
			} else if (Input.IsActionJustPressed("player_inv")){
				//_.UpdateState(State.INVENTORY); //TODO deal with how we want to control later. was thinking could use wasd to navigate items in addition to dragdrop. paused while inv?
				//GD.Print(_.inv);
				Global.HUD.ToggleInventory(pm.inv);
			} else if (Input.IsActionJustPressed("player_equip")){
				pm.EquipItem();
			}
		}
	}

	public void HandleInteract(Interactable i, Node interactionObj)
	{
		dynamic payload = i.Interact();
		switch (i.interactionType)
		{
			case InteractionType.Dialogue:
				pm.UpdateState(State.DIALOGUE);
				Global.HUD.ShowDialogue($"{payload}"); //TODO name of talker
				break;
			case InteractionType.Inventory: //opening an external inventory, such as chest
				break;
			case InteractionType.Pickup: 
				InventoryItem item = payload;
				pm.AddToInventory(item);
				item.SetGameObject((Node3D)interactionObj);
				interactionObj.GetParent().CallDeferred("remove_child", interactionObj);
				//interactionObj.QueueFree();
				Global.HUD.UpdateInventoryMenu(pm.inv);
				//TODO update inv view if visible. actually, this should automatically be done. so fix the system by which inventory updates its listview
				Global.HUD.LogEvent($" + {item.name}");
				break;
			case InteractionType.General:
				break;
			case InteractionType.Mineable:
				break;
			case InteractionType.Function:
				Global.HUD.LogEvent($"{i.Info()}");
				interactionObj.GetParent().CallDeferred("remove_child", interactionObj);
				payload(this); //TODO what return? 
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
		switch (zone){
			case ColliderZone.Awareness0:
				Interactable i = SceneManager.GetInteractable(other);
				if (i != null)
				{
					if (i.interactionMethod == InteractionMethod.Use){
						availableInteractables.Add(i);
						Global.HUD.ShowAction($"{GetFirstInteractable().Info()}");
					} else if (i.interactionMethod == InteractionMethod.Contact){
						HandleInteract(i, other);
					}
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
		Interactable i = SceneManager.GetInteractable(other);
		if (availableInteractables.Contains(i))
		{
			availableInteractables.Remove(i);
			if (availableInteractables.Count > 0){
				Global.HUD.ShowAction($"{GetFirstInteractable().Info()}");
			} else {
				Global.HUD.HideAction();
			}
		}
	}

	//set the forward vector to adjust movement control direction
	public void SetForward(Vector3 f){
		camForward = f.Normalized();	
	}
}
