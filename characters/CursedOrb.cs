using Godot;
using System;

public partial class CursedOrb : NPCNode
{
	private Vector3 scaleVector = new Vector3(1,1,1);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		physBody.Scale = (float) Math.Cos(delta % (2*Math.PI)) * scaleVector;
		physBody.Rotate(Vector3.Up, MathF.PI/32.0f);
		Position = new Vector3(Position.X, 2*(1+(float) Math.Sin(delta % (2*Math.PI))), Position.Z);

		//TODO remove. this is just for prototyping/testing
		if (Global.playerNode != null && IsInstanceValid(Global.playerNode)){
			nav.TargetPosition = Global.playerNode.Position;
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		nav.Velocity = (nav.TargetPosition - Position).Normalized() * 2;
		physBody.Velocity = nav.Velocity;
		physBody.MoveAndSlide();
	}

    public override void HandleCollide(ColliderZone zone, Node other)
    {
        throw new NotImplementedException();
    }

    public override void HandleDecollide(ColliderZone zone, Node other)
    {
        throw new NotImplementedException();
    }

}
