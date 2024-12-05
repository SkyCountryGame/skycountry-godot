using Godot;
using System;

public partial class Bat : Equipable, Collideable
{
	[Export] private MeleeItemProperties properties;

    public Bat()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        base._Ready();
        Global.RegisterGameObject(this, Name, GameObjectType.Equipable);
        if (properties == null){
            properties = ResourceLoader.Load<MeleeItemProperties>("res://gameobjects/resources/bat.tres");
        }
		itemProperties = properties;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string Info()
    {
        return "Bat";
    }
    
    //attempt to use the bat on the object. only works if thing is Destroyable
    public override void Use(dynamic obj = null){
        if (obj != null && obj is Destroyable){
            ((Destroyable)obj).ApplyDamage(properties.damage);
            CallDeferred("DisableHitbox");
            GD.Print("bat used");
        }
    }

    private void DisableHitbox(){
        hitbox.Disabled = true;
    }
    
    public void HandleCollide(ColliderZone zone, Node other)
    {
        switch(zone){
            case ColliderZone.Body:
                Use(other);
                break;
        }
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        return; //nothing to do for now
    }
}
