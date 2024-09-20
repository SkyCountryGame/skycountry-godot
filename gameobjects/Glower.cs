using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

//a thing that glows when something gets close
public partial class Glower : RigidBody3D, Interactable, Collideable {
	public InteractionType interactionType { get => InteractionType.General; }
    public InteractionMethod interactionMethod { get => InteractionMethod.Contact; }

    private bool active = false;
    private Node3D target; //the thing that entered the collision zone
    private Material material;

    public override void _Ready(){
        Global.RegisterGameObject(this, GameObjectType.Interactable);
        //material = new StandardMaterial3D();
        //material.Set("albedo", new Color(1,0,0));
        material = GetNode<MeshInstance3D>("Awareness1/CollisionShape3D/MeshInstance3D").GetActiveMaterial(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (active){
            //material.Set("emission_energy_multiplier", GlobalPosition.DistanceTo(target.GlobalTransform.Origin));
            double val = 1.0 / MathF.Pow(GlobalPosition.DistanceTo(target.GlobalTransform.Origin)-.4f,2);
            material.Set("emission_energy_multiplier", val);
        }
    }

    //start dialogue when player interacts
    public dynamic Interact()
    {
        //TODO set intensity of glow
        return null;
    }

    public void Retain()
    {
        //wat do?
    }

    public string Info()
    {
       return ".";
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        return true;
    }

    public void HandleCollide(ColliderZone zone, Node other)
    {
        switch (zone){
            case ColliderZone.Awareness0:
                GD.Print("glower zone 0");
                target = (Node3D) other;
                active = true;
                break;
            case ColliderZone.Awareness1:
                GD.Print("glower zone 1");
                break;
            default:
                break;
        }
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        switch (zone){
            case ColliderZone.Awareness0:
                active = false;
                target = null;
                break;
            default:
                break;
        }
    }
}
