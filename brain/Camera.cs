using Godot;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Numerics;
//using System.Numerics;

public partial class Camera : Camera3D
{

	[Export]
	public Node3D target; //thing to follow and look at. this can be changed. 
	private Vector3 posDest; //current destination of camera
	private Vector3 vel = new Vector3(0, 0, 0); //current velocity of camera
	private float accelCoeff = 2f; //acceleration magnitude
	private float ct = .1f; //time for camera to 
	private Vector3 offset = new Vector3(0, 8, 10);
	private float offsetDist = 10; 
	private float offsetTheta = 0; //about y
	private float offsetPhi = 45; //about x (target's x)
	private float camRotateIncrement = (float) (Math.PI / 72.0d);
	private bool isRotating = false;
	private CameraState state = CameraState.DEFAULT;

	[Export]
	private HUDManager HUD;

	private enum CameraState {
		DEFAULT, //follow player
		ALT, //follow some other Node3D target
		SEQUENCE, //move between some set of Node3D, remaining at each for some duration

	} //TODO camera can save a snapshot of its present config (orientation, distance from target, angle of view, projection parameters)
		//and these configurations can be associated with each camera target


	public override void _Ready()
	{
		Global.cam = this;
		if (target == null){ //target wasn't set in editor
			target = Global.playerNode;
		}
	}

	//currently doesn't need to be as complicated as player statemachine, but might need to be later
	private bool UpdateState(CameraState s){
		//check if can switch to the state
		//for now we can always. but will change later
		state = s;
		return true;
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (target != null){
			if (isRotating && Input.GetLastMouseVelocity().Length() > 0){
				float theta = Input.GetLastMouseVelocity().X > 0 ? -camRotateIncrement : camRotateIncrement;
				offset = offset.Rotated(Vector3.Up, theta);
				if (target is Player){
					Global.playerNode.SetForward(-new Vector3(offset.X, 0, offset.Z));
				}
			}	
			posDest = target.GlobalPosition + offset;
			//NOTE maybe use interpolation instead? 
			if (posDest != Position){// && !isRotating){
				Vector3 dir = (posDest - Position).Normalized(); 
				float d = (posDest - Position).Length(); //displacement
				
				//vel =  dir * accelCoeff * (float)delta + (posDest - Position) * .1f;
				vel = (posDest-Position)/ct; // we want to reach the destination in ct seconds
				Position += vel * (float)delta + .5f * accelCoeff * dir * (float)(delta*delta);
				
			}
			LookAt(target.GlobalPosition);

			//goal: camera never ends up lagging behind and looking at an angle. so will have to go faster as the difference is greater from the player accelerating
		}
	}


	public override void _Input(InputEvent @event){
		if (Input.IsActionPressed("cam_zoomin"))
		{
			//offset = new Vector3(0, offset.Y - .2f, offset.Z - .2f);
			offset *= .9f;
		} else if (Input.IsActionPressed("cam_zoomout"))
		{
			//offset = new Vector3(0, offset.Y + .2f, offset.Z + .2f);
			offset *= 1.1f;
		} else if (@event is InputEventMouseButton mouseEvent){
			//TODO pass raycast to appropriate object. this is to handle overlapping zones
			if (mouseEvent.ButtonIndex == MouseButton.Right && !mouseEvent.Pressed && state == CameraState.ALT) {
				LockOff();
			}
		}
		else
		{
			//if (!isRotating && Input.IsActionPressed("cam_rotate"))
			isRotating = Input.IsActionPressed("cam_rotate");
		}
	}

	//move camera to focus on some other object
	public void LockOn(Node3D target){
		//TODO move to look at some point-of-interest. 
		//LookAt(target.Origin);
		if (UpdateState(CameraState.ALT)){
			this.target = target;
		}
	}
	public void LockOff(){
		if (state == CameraState.ALT && UpdateState(CameraState.DEFAULT)){
			this.target = Global.playerNode;
		}
	}
}
