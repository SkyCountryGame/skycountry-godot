using System.Collections.Generic;
using Godot;

/*
 * attach to a simple Collider that represents a zone around some parent object, to handle collision/vicinity
 */

public partial class CollideableObject : Area3D
{
	public Node parent;
	private Collideable parentCollideable;
    private List<Area3D> collisionAreas;
	
	[Export(PropertyHint.Enum, "Awareness0,Awareness1,Body")]
	public ColliderZone zone {get; set;}
	
	[Export]
	public string devinfo {get; set;}


	
	public override void _Ready ()  
	{
		base._Ready();
		if (parent == null){
			parent = GetParent();
		}
		parentCollideable = parent as Collideable;
        BodyEntered += (body) => OnBodyEnter(body);
		BodyExited += (body) => OnBodyExit(body);
	}

	private void OnBodyEnter(Node3D other)
	{	
		parentCollideable.HandleCollide(zone, other);
	}

	private void OnBodyExit(Node3D other)
	{
		parentCollideable.HandleDecollide(zone, other);
	}
}
