using Godot;
using System;

public partial class Rock : Pickup, /*RigidBody3D*/ Interactable, Destroyable {
    int Destroyable.health { get => health; set => health = value; }
    private int health = 3;


    public Rock()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
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