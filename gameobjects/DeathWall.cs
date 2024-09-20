using Godot;

public partial class DeathWall : Node, Collideable
{
	public void HandleCollide(ColliderZone zone, Node other)
	{
		switch (zone){
			case ColliderZone.Awareness0:
				//TODO do a cool effect to show the player that the wall knows he is near
				break;
			case ColliderZone.Body:
				if (other is Damageable){
					((Damageable)other).ApplyDamage(10); //placeholder number
					//Event.Invoke(DeathWall) TODO
				}
				break;
		}
	}

	public void HandleDecollide(ColliderZone zone, Node other)
	{
		switch (zone){
			case ColliderZone.Awareness0:
			   break;
		}
	}
}
