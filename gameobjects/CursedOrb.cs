using Godot;
using System;

public partial class CursedOrb : Node3D
{
	private CharacterBody3D body;
	private Vector3 scaleVector = new Vector3(1,1,1);
	private NavigationAgent3D nav;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nav = GetNode<NavigationAgent3D>("NavAgent");		
		body = GetNode<CharacterBody3D>("CharacterBody");	
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		body.Scale = (float) Math.Cos(delta % (2*Math.PI)) * scaleVector;
		body.Rotate(Vector3.Up, MathF.PI/32.0f);
		Position = new Vector3(Position.X, 2*(1+(float) Math.Sin(delta % (2*Math.PI))), Position.Z);

		if (Global.playerNode != null){
			nav.TargetPosition = Global.playerNode.Position;
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		nav.Velocity = (nav.TargetPosition - Position).Normalized() * 2;
		body.Velocity = nav.Velocity;
		body.MoveAndSlide();
	}
}
