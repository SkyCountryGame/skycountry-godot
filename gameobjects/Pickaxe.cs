using Godot;
using System;

public partial class Pickaxe : Equipable, Interactable, Collideable {
    [Export] private InventoryItem pickaxeItem;
    

    public InteractionMethod interactionMethod => InteractionMethod.Use;
    public Pickaxe()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        MeleeItemProperties meleeItemProperties = ResourceLoader.Load<MeleeItemProperties>("res://gameobjects/resources/pickaxe.tres");
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
        pickaxeItem = new InventoryItem(meleeItemProperties, true);
        hitbox = GetNode<CollisionShape3D>("Area3D/Hitbox");
        //GetChild<MeshInstance3D>(0).SetSurfaceMaterial(0, new SpatialMaterial() { AlbedoColor = new Color(0.5f, 0.5f, 0.5f) });
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string Info()
    {
        return "Pickaxe";
    }

    //PAYLOAD 
    public dynamic Interact()
    {
        return pickaxeItem;
    }

    public void Retain()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        return true;
    }

    public override void Use(dynamic obj = null){
        if (obj != null && obj is Destroyable){
            ((Destroyable)obj).ApplyDamage(((MeleeItemProperties)pickaxeItem.GetItemProperties()).damage);
            hitbox.Disabled = true;
            GD.Print("pick used");
        }
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
