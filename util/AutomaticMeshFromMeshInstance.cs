using Godot;
using System;
using System.Drawing;

public partial class AutomaticMeshFromMeshInstance : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//get the shape of the parent collidable
		CollisionShape3D c = GetParent<CollisionShape3D>();
		Shape3D s = c.Shape;
		if (s is BoxShape3D boxshp){
			BoxMesh m = new BoxMesh();
			m.Size = boxshp.Size;
			this.Mesh = m;
		}
		if (s is CylinderShape3D cylshp){
			CylinderMesh m = new CylinderMesh();
			m.TopRadius = cylshp.Radius;
			m.BottomRadius = cylshp.Radius;
			m.Height = cylshp.Height;
			this.Mesh = m;
		}
		if (s is CapsuleShape3D){}
		if (s is SphereShape3D){}

		Rotation = c.Rotation;
		GlobalRotation = c.GlobalRotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
