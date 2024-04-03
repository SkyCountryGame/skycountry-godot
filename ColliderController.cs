using System.Collections.Generic;
using Godot;

/*
 * attach to a simple Collider that represents a zone around some parent object, to handle collision/vicinity
 */

public partial class ColliderController : ObjectController, Collideable
{
	public Node parent;
	private Collideable parentCollideable;
	public string tag;

	public ColliderController(Node o) : base(o)
	{
		//parentCollideable    find parent node
		// this.Connect("BodyEnteredEventHandler", )
		//BodyEntered += (body) => OnBodyEnter(body);
		//BodyExited += (body) => OnBodyExit(body);
	}

	public void HandleCollide(ColliderZone zone, Node other) { }
	public void HandleDecollide(ColliderZone zone, Node other) { }

	public override void update(float dt)
	{
		
	}

}
