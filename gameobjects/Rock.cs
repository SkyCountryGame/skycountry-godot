using Godot;
using System;

public partial class Rock : RigidBody3D, Collideable {
    private InventoryItem metalBar; //idk bro i know its a rock just roll with it
    private int health = 30;

    public Rock()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
        metalBar = new InventoryItem(InventoryItemProperties.ItemType.Mineral, "metalbar");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void HandleCollide(ColliderZone zone, Node other)
    {
        switch (zone){
			case ColliderZone.ToolZone:
                SceneTree tree = other.GetTree();
				other.GetNode<CollisionShape3D>("").Disabled = true;
                break;
		}
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        throw new NotImplementedException();
    }
}
