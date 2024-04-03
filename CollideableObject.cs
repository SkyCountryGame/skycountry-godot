using System.Collections.Generic;
using Godot;

/*
 * attach to a simple Collider that represents a zone around some parent object, to handle collision/vicinity
 */

public partial class CollideableObject : Area3D, Collideable
{
	public Node parent;
	private Collideable parentCollideable;
	private ColliderController ctlr;
	
	public string tag; //TODO use enum
	
	public override void _Ready ()  
	{
		base._Ready();
		if (parent == null){
			parent = GetParent();
		}

		parentCollideable = parent as Collideable;
				//parentCollideable    find parent node
	   // this.Connect("BodyEnteredEventHandler", )
		BodyEntered += (body) => OnBodyEnter(body);
		BodyExited += (body) => OnBodyExit(body);
	}

	private void OnBodyEnter(Node3D other)
	{	
		parentCollideable.HandleCollide(ColliderZone.Body, other);
		switch (tag)
		{
			case "Body":
				//HandleCollide(ColliderZone.Body, other);
				parentCollideable.HandleCollide(ColliderZone.Body, other); 
				break;
			case "Awareness0":
				parentCollideable.HandleCollide(ColliderZone.Awareness0, other);
				break;
			case "Awareness1":
				parentCollideable.HandleCollide(ColliderZone.Awareness1, other);
				break;
			default:
				break;
		}
	}

	private void OnBodyExit(Node3D other)
	{
		parentCollideable.HandleDecollide(ColliderZone.Body, other);
		GD.Print("body exited");
		switch (tag)
		{
			case "Body":
				//HandleCollide(ColliderZone.Body, other);
				parentCollideable.HandleDecollide(ColliderZone.Body, other);
				break;
			case "Awareness0":
				parentCollideable.HandleDecollide(ColliderZone.Awareness0, other);
				break;
			case "Awareness1":
				parentCollideable.HandleDecollide(ColliderZone.Awareness1, other);
				break;
			default:
				break;
		}
	}

	public void HandleCollide(ColliderZone zone, Node other) { }
	public void HandleDecollide(ColliderZone zone, Node other) { }
}
