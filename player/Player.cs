using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static PlayerModel;

public partial class Player : CharacterBody3D, Collideable, Interactor, Damageable
{

	//MOVEMENT 
	public double gravity = 3f;
	public float JumpVelocity = 30;
	private bool jump = false;
	private double jumpBuffer = .16; //what percentage of a second you want the buffer to be
	private double currentJumpTiming = 0;
	private Vector3 velocity = Vector3.Zero;
	[Export]
	private AnimationPlayer animationPlayer; 
	private Vector3 controlDir; //user-inputted vector of intended direction of player, adjusted for camera
	private Vector3 inputDir = new Vector3(); //user-inputted vector of intended direction of player
	private Node3D rightHand;
	public float accelScalar = 0; //made this public for the devtool. personally i'm ok with this being public, but understand if we want to keep it private. in that case just have devtool broadcast changeevents that objects can listen to 
	public float velMagnitudeMax = 24f; //approximate max velocity allowed
	public Vector3 camForward = Vector3.Forward; //forward vector of camer

	//INTERACTION STUFF
	private HashSet<Interactable> availableInteractables = new HashSet<Interactable>();
	//UI stuff
	
	//PLAYER STATE
	private PlayerModel playerModel; //this is the player data that should be persisted between scenes. '_M' because shorthand
	[Export]
	public AnimationTree animationTree;


	public override void _Ready()
	{
		base._Ready();
		if (Global.playerModel == null){
			playerModel = new PlayerModel(this); //TODO what parameters to give here
			Global.playerModel = playerModel;
		} else {
			playerModel = Global.playerModel;
		}
		Global.playerNode = this; //while the playerMODEL will remain the same between scenes, the playerNODE could change
		ApplyFloorSnap();
		if (animationTree == null){ //animtree might be set from editor (.tscn file)
			animationTree = GetNode<AnimationTree>("RollinDudeMk5/AnimationTree"); //NOTE in future might we have other player models? 
		}
		rightHand = GetNode<Node3D>("RollinDudeMk5/Armature/Skeleton3D/HandAttachment/HandContainer/ItemContainer");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(playerModel.activityState==State.DEFAULT){
			DoMotion(delta); 
		}
		animationTree.Set("parameters/Run/blend_position", Velocity.Length() / velMagnitudeMax);
	}
	public override void _Process(double delta)
	{
		if (SceneManager._ != null && SceneManager._.currentLevelScene != GetTree().CurrentScene){
			SceneManager._.SetActiveLevelScene(GetTree().CurrentScene); //tell the level manager what scene we are in
		}



		//RayCast Stuff
		if(playerModel.activityState==State.DEFAULT){
			Vector2 mousePosition = GetViewport().GetMousePosition();
			Camera3D camera =  Global.cam;
			Vector3 rayOrigin = camera.ProjectRayOrigin(mousePosition);
			Vector3 rayTarget = rayOrigin+camera.ProjectRayNormal(mousePosition)*10000;
			PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
			Godot.Collections.Dictionary intersection = spaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(rayOrigin, rayTarget,(uint)Math.Pow(2,14-1)));
			if(intersection.ContainsKey("position") && !intersection["position"].Equals(null)){
				Vector3 pos = (Vector3)intersection["position"];
				Vector3 viewAngle = new Vector3(pos.X, Position.Y, pos.Z);
				LookAt(viewAngle);
			}
		}	
		

		//HUD stuff
		if (!Global.hud.actionLabel.Visible && availableInteractables.Count > 0){
			Global.hud.ShowAction($"{GetFirstInteractable().Info()}");
		}

		if(currentJumpTiming>jumpBuffer){
			jump = false;
		}
		else {
			currentJumpTiming+=delta;
		}
	}

	public override void _Input(InputEvent ev){
		
		//do appropriate thing whether we are in inventory or not
		if ((playerModel.activityState & (State.DIALOGUE | State.INVENTORY)) != 0){ 
			if (Input.IsActionJustPressed("ui_back")){
				playerModel.UpdateState(State.DEFAULT);
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
			if(Input.IsActionJustPressed("player_action1") && (playerModel.activityState & (State.DEFAULT | State.AIMING)) != 0){
				if(playerModel.equipped != null ){ //hmmmmmmmmmmmmmmm has to be a better way
					playerModel.activityState = State.ATTACKING;
					playerModel.rightHandEquipped.GetNode<CollisionShape3D>("RigidBody3D/Hitbox").Disabled=false;
					((AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback")).Travel(((MeleeItemProperties) playerModel.equipped.itemProperties).swingAnimation);
					animationTree.AnimationFinished += (value) => 
					{
						playerModel.rightHandEquipped.GetNode<CollisionShape3D>("RigidBody3D/Hitbox").Disabled=true;
						playerModel.activityState = State.DEFAULT;
					};	

					//playerModel.equipped;(MeleeItemProperties)playerModel.equipped.itemProperties).equipPath+"/Area3D/Hitbox";
				}		
			}
			else if (Input.IsActionJustPressed("player_action2")){

			} else if (Input.IsActionJustPressed("player_jump")) //TODO implement charge-up later
			{
				currentJumpTiming=0;
				jump = true;
			} else if (Input.IsActionJustPressed("player_use")){
				switch (playerModel.activityState){
					case State.DEFAULT: //attempt to interact with something in the world
						Interactable i = GetFirstInteractable();
						if (i != null)
						{
							if (i.interactionMethod == InteractionMethod.Use){
								HandleInteract(i, (Node)i);
							}
						} else {
							Global.hud.LogEvent("there is nothing with which to interact");
						}
						break;
					case State.DIALOGUE:
						Global.hud.ContinueDialogue(); //NOTE this does nothing currently. 
						break;
				}
			} else if (Input.IsActionJustPressed("pause"))
			{
				//TODO pause
			} else if (Input.IsActionJustPressed("player_inv")){
				//_.UpdateState(State.INVENTORY); //TODO deal with how we want to control later. was thinking could use wasd to navigate items in addition to dragdrop. paused while inv?
				//GD.Print(_.inv);
				Global.hud.ToggleInventory(playerModel.inv);
			} else if (Input.IsActionJustPressed("player_equip")){
				EquipItem();
			}
		}
	}

	//process player movement based on user input and other factors. this is WIP
	private void DoMotion(double delta){
		float angleToCam = camForward.SignedAngleTo(Vector3.Forward, Vector3.Up); //angle between control forward and camera forward
		controlDir = inputDir.Normalized().Rotated(Vector3.Down, angleToCam);
		
		//TODO if controlDir is close to 180d from current vel, then set accel to some multiple of maxmagnitude until vel reverses

		Vector3 gv = controlDir * velMagnitudeMax; //goal velocity based on user input
		if (accelScalar == 0){ //acceleration activation toggle
			if (controlDir.Length() == 0 && Velocity.Length() < 0.01f){
				velocity = Vector3.Zero;
			} else {
				velocity = gv;
			}
			velocity.Y = Velocity.Y; //thank you adam
		} else {
			Vector3 accel = (gv - Velocity).Normalized() * accelScalar; //accelerate towards desired velocity
			if (controlDir.Length() == 0 && Velocity.Length() < 0.01f){
				velocity = Vector3.Zero;
				accel = Vector3.Zero;
			} else if (Velocity.Length() > gv.Length()) {
				velocity = gv;
			}
			velocity.Y = Velocity.Y;
			if (Velocity.Length() < velMagnitudeMax){
				velocity += accel * (float)delta;
			}
		}
		if (jump && IsOnFloor()){
			velocity.Y += JumpVelocity;
			jump = false;
		} else if (IsOnFloor()) {
			velocity.Y = 0; 
		} else {
			velocity.Y -= (float) (gravity * delta) * 64; //- 20;
		}
		Velocity = velocity;
		MoveAndSlide();
	}

	public void HandleInteract(Interactable i, Node interactionObj)
	{
		dynamic payload = i.Interact();
		switch (i.interactionType)
		{
			case InteractionType.Dialogue:
				playerModel.UpdateState(State.DIALOGUE);
				Global.hud.ShowDialogue(payload); //TODO name of talker
				break;
			case InteractionType.Inventory: //opening an external inventory, such as chest
				break;
			case InteractionType.Pickup: 
				InventoryItem item = payload;
				playerModel.AddToInventory(item);
				interactionObj.GetParent().CallDeferred("remove_child", interactionObj);
				//interactionObj.QueueFree();
				Global.hud.UpdateInventoryMenu(playerModel.inv);
				//TODO update inv view if visible. actually, this should automatically be done. so fix the system by which inventory updates its listview
				Global.hud.LogEvent($" + {item.name}");
				break;
			case InteractionType.General:
				break;
			case InteractionType.Function:
				Global.hud.LogEvent($"{i.Info()}");
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
				Interactable interactable = SceneManager.GetInteractable(other);
				if (interactable != null)
				{
					if (interactable.interactionMethod == InteractionMethod.Use){
						availableInteractables.Add(interactable);
						Global.hud.ShowAction($"{GetFirstInteractable().Info()}");
					} else if (interactable.interactionMethod == InteractionMethod.Contact){
						HandleInteract(interactable, other);
					}
				}
				break;
			case ColliderZone.Awareness1:
				break;
			case ColliderZone.Body:
					interactable = SceneManager.GetInteractable(other);
					if (interactable.interactionMethod == InteractionMethod.Contact){
						HandleInteract(interactable, other);
					}
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
				Global.hud.ShowAction($"{GetFirstInteractable().Info()}");
			} else {
				Global.hud.HideAction();
			}
		}
	}

	//set the forward vector to adjust movement control direction
	public void SetForward(Vector3 f){
		camForward = f.Normalized();	
	}

	public void EquipRightHand(InventoryItem item){
		string equipPath = "";
		switch(item.itemProperties){
			case MeleeItemProperties i:
				equipPath = i.equipPath;
				break;
			case RangedItemProperties i:
				equipPath = i.equipPath;
				break;
			default:

				break;
		}
		playerModel.rightHandEquipped = (Node3D)item.GetPackedScene().Instantiate().GetNode(equipPath).Duplicate();
		rightHand.AddChild(playerModel.rightHandEquipped);
	}

	public void UnequipRightHand(){
		if(rightHand.GetChildCount()>0){
			rightHand.RemoveChild(rightHand.GetChild(0));
		}
	}

	/** by default equip the primary item, or give an item to equip */
	public bool EquipItem(InventoryItem item = null){
		if (playerModel.inv.IsEmpty() || !item.equippable || item == null) return false;
		if (playerModel.inv.Contains(item)){
			if(playerModel.equipped != null) {
				UnequipRightHand();
			}
			playerModel.equipped = item;
			EquipRightHand(item);
		}	
		return playerModel.equipped != null;
	}

	/** drop the equipped item, or a specific item */
	public bool DropItem(InventoryItem item = null){
		if (playerModel.inv.IsEmpty() || item == null) return false;
		// if (item == null){
		// 	item = equipped;
		// } commenting out because im not sure why this would be the case
		if (playerModel.inv.RemoveItem(item)){
			Node gameObject = item.GetPackedScene().Instantiate();
			SceneManager._.currentLevelScene.AddChild(gameObject);
			((Node3D) gameObject).Position = Position + new Vector3(0,1,1);

			if (item == playerModel.equipped){
				UnequipRightHand();
				playerModel.equipped = null;
			}
			Global.hud.ShowEquipped(); //TODO should not have to call this. fix
			return true;
		}
		return false;
	}
	public void ApplyDamage(int d)
	{
		playerModel.hp -= d; //TODO take into account armor, skills, etc.
		if (playerModel.hp < 0){
			//EventManager.Invoke(EventType.GameOver); //TODO this depends on changes from another branch 
		}
	}

	    public void SetPlayerModel(PlayerModel pm)
    {
        this.playerModel = pm;
    }
}

