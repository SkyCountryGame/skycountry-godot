using Godot;
using System;

public partial class FishingPole : Equipable {

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        base._Ready();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string Info()
    {
        return "Fishing Pole";
    }
    
    //attempt to use the pickaxe on the object. only works if thing is Destroyable
    public override void Use(dynamic obj = null){
        GD.Print("Fishing Pole used");
    }

    private void DisableHitbox(){
        hitbox.Disabled = true;
    }
    
    public void HandleCollide(ColliderZone zone, Node other)
    {
        switch(zone){
            case ColliderZone.Body:
                Use(other);
                break;
        }
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        return; //nothing to do for now
    }

}
