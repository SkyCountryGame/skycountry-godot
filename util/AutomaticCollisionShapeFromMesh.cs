using Godot;
using Godot.Collections;
using System;
using System.Drawing;

//attach this script to a node with some number of MeshInstance3D children and it will add a child CollisionShape3D for those meshes 
public partial class AutomaticCollisionShapeFromMesh : Node3D
{
	//another idea: instead of attaching to each prop, just have the level script call this to automatically scan all things in some node group
	[Export] private Array<MeshInstance3D> meshes;
	//TODO parameters for "resolution"
	
	//[Export]  TODO specify type of collider to generate. cyl, box, general, 
	
	public override void _Ready()
	{
		if (meshes == null) { //attempt to find some meshes
			meshes = new Array<MeshInstance3D>();
			foreach (Node3D n in GetChildren(true)){
				if (n is MeshInstance3D){
					meshes.Add((MeshInstance3D)n);
				} else {
					GD.Print(n.Name);
				}
			}
		}
		if (meshes.Count == 0){
			GD.Print($"{Name} has no meshes. {GetChildren().Count}");
			return;
		}
		GD.Print($"{Name}: {meshes.Count} meshes");
		CollisionShape3D cs = new CollisionShape3D();
		foreach (MeshInstance3D mi in meshes){
			GD.Print($"  {mi.GetAabb()}");
			Mesh m = mi.Mesh;
			GD.Print($"  {m.GetAabb()}");
			if (m is ArrayMesh am){
				GD.Print($"    surface count: {am.GetSurfaceCount()}");
				for (int i = 0; i < am.GetSurfaceCount(); i++){
					Godot.Collections.Array sa = am.SurfaceGetArrays(i);
					GD.Print($"    surface {i}: {am.SurfaceGetFormat(i)}; {am.SurfaceGetPrimitiveType(i)}");
					GD.Print($"        array len: {am.SurfaceGetArrayLen(i)}");
					GD.Print($"        arrays: {sa.Count}");
					//TODO each array data. faces? 
				}
				
			} else {
				GD.Print($"    {mi.GetType()}");
			}
		}
	}

	public override void _Process(double delta)
	{
	}
}
