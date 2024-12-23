using Godot;
using Godot.Collections;
using System;
using System.Drawing;
using System.Linq;

//attach this script to a node with some number of MeshInstance3D children and it will add a child CollisionShape3D for those meshes 
public partial class AutomaticCollisionShapeFromMesh : Node3D
{
	//another idea: instead of attaching to each prop, just have the level script call this to automatically scan all things in some node group
	[Export] private Array<MeshInstance3D> meshes;
	//TODO parameters for "resolution"

	//[Export]  TODO specify type of collider to generate. cyl, box, general, 

	public override void _Ready()
	{
		if (meshes == null)
		{ //attempt to find some meshes
			meshes = new Array<MeshInstance3D>();
			foreach (Node3D n in GetChildren(true))
			{
				if (n is MeshInstance3D)
				{
					meshes.Add((MeshInstance3D)n);
				}
				else
				{
					GD.Print(n.Name);
				}
			}
		}
		if (meshes.Count == 0)
		{
			GD.Print($"{Name} has no meshes. {GetChildren().Count}");
			return;
		}
		GD.Print($"{Name}: {meshes.Count} meshes");
		foreach (MeshInstance3D mi in meshes)
		{
			GD.Print($"  {mi.GetAabb()}");
			Mesh m = mi.Mesh;
			if (m is ArrayMesh am)
			{
				//get vertices from each surface. and print some debug info
				var vertices = new Array<Vector3>();
				GD.Print($"    surface count: {am.GetSurfaceCount()}");
				for (int i = 0; i < am.GetSurfaceCount(); i++)
				{
					Godot.Collections.Array sa = am.SurfaceGetArrays(i);
					GD.Print($"    surface {i}: {am.SurfaceGetFormat(i)}; {am.SurfaceGetPrimitiveType(i)}");
					GD.Print($"        array len: {am.SurfaceGetArrayLen(i)}");
					GD.Print($"        arrays: {sa.Count}");
					//TODO each array data. faces? 
					if (sa.Count > 0)
					{
						vertices.AddRange((Array<Vector3>)sa[(int)Mesh.ArrayType.Vertex]); 
					}
				}

				if (vertices.Count > 0)
				{
					CollisionShape3D cs = new CollisionShape3D();
					var shape = new ConvexPolygonShape3D();
					shape.Points = vertices.ToArray();
					cs.Shape = shape;
					
					// Match the mesh's transform
					cs.Transform = mi.Transform;
					AddChild(cs);
				}
			}
			else
			{
				GD.Print($"    {mi.GetType()}");
			}
		}


	}

	public override void _Process(double delta)
	{
	}
}
