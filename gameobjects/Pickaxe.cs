using Godot;
using System;
using System.ComponentModel;

public partial class Pickaxe : StaticBody3D, Interactable {
    public InteractionType interactionType => InteractionType.Pickup;

    public InteractionMethod interactionMethod => InteractionMethod.Use;

    private InventoryItem pickaxeItem;

    public Pickaxe()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        SceneManager.RegisterGameObject(this, Name, GameObjectType.Interactable);
        pickaxeItem = new InventoryItem(InventoryItem.ItemType.Weapon, "pickaxe", "TestPickaxe", true);
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
        throw new NotImplementedException();
    }
}
