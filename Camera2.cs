using Godot;
using System;
//using System.Numerics;
//using System.Numerics;

public partial class Camera2 : Camera3D
{

	[Export]
	public Node3D target; //thing to follow and look at

	private Player plyr;
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

	[Export]
	private HUDManager HUD;


	public override void _Ready()
	{
		if (target == null){ //target wasn't set in editor
			target = (Node3D)GetNode("../Player");
		}
		if (target is Player){
			plyr = (Player)target;
		}

		Global.Cam = this;	
	}

	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if (isRotating && Input.GetLastMouseVelocity().Length() > 0){
			float theta = Input.GetLastMouseVelocity().X > 0 ? -camRotateIncrement : camRotateIncrement;
			offset = offset.Rotated(Vector3.Up, theta);
			if (plyr != null){
				//plyr.SetForward(offset - Vector3.Up * offset.Dot(Vector3.Up)); //update the player's orientation to the offset without its y component (projected onto xz plane)
				plyr.SetForward(-new Vector3(offset.X, 0, offset.Z));
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

    public override void _Input(InputEvent @event){
		if (Input.IsActionPressed("cam_zoomin"))
		{
			//offset = new Vector3(0, offset.Y - .2f, offset.Z - .2f);
			offset *= .9f;
		} else if (Input.IsActionPressed("cam_zoomout"))
		{
			//offset = new Vector3(0, offset.Y + .2f, offset.Z + .2f);
			offset *= 1.1f;
		}
		else
		{
			//if (!isRotating && Input.IsActionPressed("cam_rotate"))
			isRotating = Input.IsActionPressed("cam_rotate");
		}
	}
}
