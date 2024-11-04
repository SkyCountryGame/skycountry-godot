using Godot;
using System;

//a thing that the player can equip in his hand. a tool. this class is the ingame world object (Node3D), while the inventoryItem is the item's properties, similar to Player and PlayerModel
public abstract partial class Equipable : Node3D {
    private InventoryItem inventoryItem;
    public CollisionShape3D hitbox; //collisionzone that triggers tool use on object of tool
    //type? properties? or should all that be in the extending classes?

    public Equipable()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        MeleeItemProperties meleeItemProperties = ResourceLoader.Load<MeleeItemProperties>("res://gameobjects/resources/pickaxe.tres");
        Global.RegisterGameObject(this, Name, GameObjectType.Equipable);
        hitbox = GetNode<CollisionShape3D>("Area3D/Hitbox");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public abstract void Use(dynamic obj = null); //use the tool, on object obj if applicable

}