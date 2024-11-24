using System.Collections.Generic;
using Godot;

/*
 * a collision zone -- to handle collisions with different "parts" or "zones" of some parent game object, like an arm vs a leg vs a line of sight. 
 * this is attached to an Area3D which has a CollisionShape that represents its area of concern, and (importantly) is a child of a node that implements Collideable interface that is the parent object that wants to handle its different collisions of concern (did something enter my awareness zone? my line of sight? my leg? my axe?)
 */ 

public partial class CollisionZone : Area3D
{
	public Node parent;
	private Collideable parentCollideable;
	
	[Export]
	public ColliderZone zone {get; set;} //this is a label for zone, so that the parent Collideable knows what zone was collided with when the function is triggered. defined in Collideable.cs
	
	public override void _Ready ()  
	{
		base._Ready();
		if (parent == null){
			parent = GetParent();
		}
		//traverse up the tree until we find the node that implements Collideable, because maybe this zone is deep down in some hierarchy
		while (parent is not Collideable && parent != GetTree().Root){
			parent = parent.GetParent();
		}
		parentCollideable = parent as Collideable;
		BodyEntered += (body) => OnBodyEnter(body);
		BodyExited += (body) => OnBodyExit(body);
		//only handling bodies, not other Area3Ds. will implement that later if we need it
	}

	private void OnBodyEnter(Node other)
	{	
		parentCollideable.HandleCollide(zone, other); //tell the parent that other collided with zone zone
	}

	private void OnBodyExit(Node other)
	{
		parentCollideable.HandleDecollide(zone, other); //tell the parent that something collided with zone zone
	}
}
