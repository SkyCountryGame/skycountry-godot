using Godot;
using System;
using System.ComponentModel;

public partial class Rock : RigidBody3D, Interactable {
    public InteractionType interactionType => InteractionType.Pickup;

    public InteractionMethod interactionMethod => InteractionMethod.Use;

    private InventoryItem rockItem;

    public Rock()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PrefabManager.RegisterGameObject(this, Name, GameObjectType.Interactable);
        rockItem = new InventoryItem(InventoryItemProperties.ItemType.Mineral, "rock", false);
        //GetChild<MeshInstance3D>(0).SetSurfaceMaterial(0, new SpatialMaterial() { AlbedoColor = new Color(0.5f, 0.5f, 0.5f) });
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string Info()
    {
        return "Rock";
    }

    //PAYLOAD 
    public dynamic Interact()
    {
        return rockItem;
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
