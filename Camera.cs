using Godot;
using System;

public partial class Camera : Camera3D
{
	private bool isRotating = false;
	private Node3D twistPivot, pitchPivot;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		twistPivot = (Node3D) GetNode("../../../TwistPivot");
		pitchPivot = (Node3D) GetNode("../../PitchPivot");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float dt = (float)delta;
		if (isRotating)
		{
			Vector2 mouseVel = Input.GetLastMouseVelocity() * .01f;
			twistPivot.RotateY(mouseVel.X * -.03f);
			pitchPivot.RotateX(mouseVel.Y * -.03f);
			GD.Print($"pitch: {pitchPivot.Rotation.X/MathF.PI}pi");
			if (pitchPivot.Rotation.X > 0) pitchPivot.Rotation = new Vector3(0, 0, 0);
			if (pitchPivot.Rotation.X < -.5f*MathF.PI) pitchPivot.Rotation = new Vector3(-.5f*MathF.PI, 0, 0);
		}
	}
	
	public override void _Input(InputEvent @event){
		if (Input.IsActionPressed("cam_zoomin"))
		{
			Position = new Vector3(0, 0, Position.Z - .2f);
		} else if (Input.IsActionPressed("cam_zoomout"))
		{
			Position = new Vector3(0, 0, Position.Z + .2f);
		}
		else
		{
			isRotating = Input.IsActionPressed("cam_rotate");
		}
	}
}
