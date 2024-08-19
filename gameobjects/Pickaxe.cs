using Godot;
using System;

public partial class Pickaxe : StaticBody3D, Interactable, IBaseMelee {
    private InventoryItem pickaxeItem;


    public string name => "pickaxe";


    public string equipPath => "TestPickaxe";

    public int durability => 1;
    public int damage => 1;
    public int arcLength => arcLength; 
    public int range=> 1; 
    public int reswingSpeed  => reswingSpeed; 
    public string swingAnimation => "Mining02"; 

    public InteractionType interactionType => InteractionType.Pickup;

    public InteractionMethod interactionMethod => InteractionMethod.Use;
    public Pickaxe()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        SceneManager.RegisterGameObject(this, Name, GameObjectType.Interactable);
        pickaxeItem = new InventoryItem(InventoryItem.ItemType.Weapon, this, true);
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

    public Animation Swing(AnimationTree animationTree)
    {
        throw new NotImplementedException();
    }

}
