using System;
using Godot;

/**
    this is an explicit implementation of an interactable lamppost
*/
public partial class LampPost : Node, Interactable {


    public InteractionMethod interactionMethod => throw new NotImplementedException();

  	//[Export(PropertyHint.Enum, "interactionType")]
    public InteractionType interactionType => InteractionType.General;
    private OmniLight3D lamplight;

    public override void _Ready()
	{
        //ResourceManager.interactables.Add(this);
        ResourceManager.RegisterGameObject(this, GameObjectType.Interactable);
        lamplight = GetNode<OmniLight3D>("CollisionShape3D/StaticBody3D/OmniLight3D");
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public string Info()
    {
        throw new NotImplementedException();
    }

    public dynamic Interact()
    {
        //toggle lamp light
        lamplight.LightEnergy = lamplight.LightEnergy == 0 ? 1 : 0;
        return true;
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

    public void Retain()
    {
        throw new NotImplementedException();
    }

}
