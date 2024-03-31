using System.Collections.Generic;
using Godot;

/*
 * attach to a simple Collider that represents a zone around some parent object, to handle collision/vicinity
 */

public partial class ColliderController : Area3D, Collideable
{
    public Node parent;
    private Collideable parentCollideable;
    public string tag;
    
    public override void _Ready ()  
    {
        base._Ready();
        if (parent == null){
            parent = GetParent();
        }

        parentCollideable = parent as Collideable;
                //parentCollideable    find parent node
        BodyEnteredEventHandler += (body) => OnBodyEntered(body);
    }

    private void OnBodyEntered(Node3D body)
    {
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

    private void OnTriggerExit(Collider other)
    {
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

    public void HandleCollide(ColliderZone zone, Collider other) { }
    public void HandleDecollide(ColliderZone zone, Collider other) { }

    public void HandleCollide(ColliderZone zone, Node other)
    {
        throw new System.NotImplementedException();
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        throw new System.NotImplementedException();
    }
}
