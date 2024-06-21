using Godot;
using System;
//using System.Numerics;

public partial class Camera2 : Camera3D
{
	//NOTE i'm currently playing around with different possibilites, so some of the vars will be removed etc
	private bool isRotating = false;
	private Node3D target; //thing to follow and look at
	private Vector3 posDest; //current destination of camera
	private Vector3 vel; //current velocity of camera
	private float accelCoeff = 2f; //acceleration magnitude
	private float ct = .1f; //time for camera to 
	private Vector3 offset = new Vector3(0, 8, 10);
	private float offsetDist = 10; 
	private float offsetTheta = 0; //about y
	private float offsetPhi = 45; //about x (target's x)
	private float camRotate = (float) (Math.PI / 128.0d);

	[Export]
	private HUDManager HUD;


	public override void _Ready()
	{
		target = (Node3D)GetNode("../Player"); //FUTUREDESIGN: general Node
		vel = new Vector3(0, 0, 0);
	}

	public override void _Process(double delta)
	{
	
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		posDest = target.GlobalPosition + offset;
		//NOTE maybe use interpolation instead? 
		if (posDest != Position && !isRotating){
			Vector3 dir = (posDest - Position).Normalized(); 
			float d = (posDest - Position).Length(); //displacement
			//vel =  dir * accelCoeff * (float)delta + (posDest - Position) * .1f;
			vel = (posDest-Position)/ct; // we want to reach the destination in ct seconds
			Position += vel * (float)delta + .5f * accelCoeff * dir * (float)(delta*delta);
			LookAt(target.GlobalPosition);
		}

		if (isRotating && Input.GetLastMouseVelocity().Length() > 0){
			//rotate about target TODO this is obviously wrong but leaving it there
			RotateY(Input.GetLastMouseVelocity().X > 0 ? -camRotate : camRotate);
		}

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
			if (!isRotating && Input.IsActionPressed("cam_rotate")) HUD.LogEvent("yes i know camera rotation is broken. ");
			isRotating = Input.IsActionPressed("cam_rotate");
			
		}
	}
}
