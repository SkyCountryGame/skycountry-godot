using Godot;
using System;

public partial class Rock : RigidBody3D, Interactable, Destroyable {
	int Destroyable.health { get => health; set => health = value; }

	public InteractionType interactionType => InteractionType.Pickup;

	public InteractionMethod interactionMethod => InteractionMethod.Destroy;

	private int health = 3;

	[Export] public InventoryItem invItem;

	public Rock()
	{
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public string Info()
	{
		return "Rock";
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
		EventManager.Invoke(EventType.WorldItemDestroyed, (Global.GetGameObject(this), GlobalPosition, invItem));
		GD.Print("destroy rock!");
	}

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
		QueueFree();
	}

	public bool IsInteractionValid(Interactor interactor)
	{
		throw new NotImplementedException();
	}

}
