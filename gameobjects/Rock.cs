using Godot;
using System;

public partial class Rock : RigidBody3D, Interactable, Destroyable {
    int Destroyable.health { get => health; set => health = value; }

    public InteractionType interactionType => InteractionType.Pickup; //in addition to being able to be mined, you can also pick up rocks. they might be heavy though

    public InteractionMethod interactionMethod => InteractionMethod.Use;


    private int health = 3;

    private InventoryItem rockItem;


    public Rock()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
        rockItem = new InventoryItem(InventoryItemProperties.ItemType.Mineral, "rock", false); //NOTE: this doesn't actually need to be constructed until the player mines/picks-up the rock
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

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0){
            Destroy();
        }
    }

    public void Destroy()
    {
        //trigger an event to notify that a rock has been destroyed, and pass along the associated GameObject
        EventManager.Invoke(EventType.WorldItemDestroyed, Global.GetGameObject(this)); 
        GD.Print("destroy rock!");
        QueueFree();
    }

}