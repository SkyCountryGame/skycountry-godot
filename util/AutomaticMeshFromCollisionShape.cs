using Godot;
using System;
using System.Drawing;

public partial class AutomaticMeshFromCollisionShape : CollisionShape3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        MeshInstance3D mi = new MeshInstance3D();
		mi.AddToGroup("mesh");
		if (Shape is BoxShape3D boxshp){
			BoxMesh m = new BoxMesh();
			m.Size = boxshp.Size;
			mi.Mesh = m;
		}
		if (Shape is CylinderShape3D cylshp){
			CylinderMesh m = new CylinderMesh();
			m.TopRadius = cylshp.Radius;
			m.BottomRadius = cylshp.Radius;
			m.Height = cylshp.Height;
			mi.Mesh = m;
		}
		if (Shape is CapsuleShape3D){}
		if (Shape is SphereShape3D){}

		mi.Rotation = Rotation;

        AddChild(mi);        
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
