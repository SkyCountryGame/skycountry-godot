using Godot;
using System;
using System.ComponentModel;

public partial class Twig : RigidBody3D, Interactable {
    public InteractionType interactionType => InteractionType.Pickup;

    public InteractionMethod interactionMethod => InteractionMethod.Use;


    private InventoryItem invItem;

    public Twig()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PrefabManager.RegisterGameObject(this, GameObjectType.Interactable);
        invItem = new InventoryItem(InventoryItemProperties.ItemType.Quest, "Twig", true);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string Info()
    {
        return "Twig";
    }

    //PAYLOAD 
    public dynamic Interact()
    {
        return invItem;
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
