using Godot;
using System;
using System.ComponentModel;

public partial class Rock : RigidBody3D {
    private InventoryItem metalBar;
    private int health = 3; 

    public Rock()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        BodyEntered += OnBodyEntered;
        SceneManager.RegisterGameObject(this, Name, GameObjectType.Mineable);
        metalBar = new InventoryItem(InventoryItem.ItemType.Mineral, "metalbar");
        //GetChild<MeshInstance3D>(0).SetSurfaceMaterial(0, new SpatialMaterial() { AlbedoColor = new Color(0.5f, 0.5f, 0.5f) });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    private void OnBodyEntered(Node body)
    {
        GD.Print("I AM IN THE METHOD");
        if(body is Area3D){
            Area3D area = (Area3D)body;
            if(area.GetCollisionMaskValue(3) && area.FindChild("Hitbox") != null && !((CollisionShape3D)area.FindChild("Hitbox")).Disabled){
                health -=1;
                GD.Print("Health is "+health);
            }
        }
        
    }
}
