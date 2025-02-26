using Godot;
using System;

public partial class PickupBase : Node3D, Interactable
{
	[Export]
	public InventoryItem inventoryItem;
	[Export] MeshInstance3D meshInstance;
	[Export] CollisionShape3D collisionShape;

    public InteractionType interactionType => InteractionType.Pickup;

    public InteractionMethod interactionMethod => InteractionMethod.Use;

    public override void _Ready()
	{
		setupPickupBase();
	}

	public PickupBase(){
		
	}
	public PickupBase(InventoryItem item): this(){
		inventoryItem = item;
		setupPickupBase();
	}

	public void setupPickupBase(){
		if(collisionShape == null){
			collisionShape = new CollisionShape3D();
		}
		switch (inventoryItem.collisionShapeType){
			case InventoryItem.CollisionShapeType.CapsuleShape3D:
				CapsuleShape3D capsuleShape3D = new CapsuleShape3D();
				capsuleShape3D.Height = inventoryItem.height;
				capsuleShape3D.Radius = inventoryItem.radius;
				collisionShape.Shape = capsuleShape3D;
				break;
			default:
				break;
		}

		if(meshInstance == null){
			meshInstance = new MeshInstance3D();
		}
		meshInstance.Mesh = inventoryItem.mesh;
		if(inventoryItem.scale != Vector3.Zero){
			Scale = inventoryItem.scale;
		}

		//if gpuparticle, set gpuparticle
		if(inventoryItem.gpuParticles != null){
			//gpuParticles = (GpuParticles3D) inventoryItem.gpuParticles.Instantiate().Duplicate();
			 GpuParticles3D gpuParticlesTemp = (GpuParticles3D) inventoryItem.gpuParticles.Instantiate();
			 AddChild(gpuParticlesTemp);
		}
		Global.RegisterGameObject(this, inventoryItem.name, GameObjectType.Interactable);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public dynamic Interact()
    {
        QueueFree();
        return inventoryItem;
    }

    public void Retain()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        return;
    }

    public string Info()
    {
		return inventoryItem.name;
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

}
