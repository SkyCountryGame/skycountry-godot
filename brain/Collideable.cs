using Godot;

/**
	this is so that things can behave differently based on what collider zone is triggered.
	a Node should implement this interface if it wants to handle collisions for different zones. and it needs to have one or more Area3D children, each with CollisionZone script attached so that it can specify its zone
*/

public enum ColliderZone
{
	Awareness0, //e.g. did i hear something? 
	Awareness1, //e.g. hey! i see you! 
	Body //e.g. watch where you're walkin!
}

public interface Collideable
{
	//these will be called whenever something collides with one of the zones, and you can just check which zone
	void HandleCollide(ColliderZone zone, Node other);  
	void HandleDecollide(ColliderZone zone, Node other);
}
