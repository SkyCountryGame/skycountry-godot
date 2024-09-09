using Godot;

public partial class DeathWall : Node, Collideable
{
    public void HandleCollide(ColliderZone zone, Node other)
    {
        switch (zone){
            case ColliderZone.Awareness0:
                GD.Print("death wall warning"); //TODO
                break;
            case ColliderZone.Body:
                GD.Print("death wall damage");
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