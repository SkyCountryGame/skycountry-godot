using System;
using Godot;

/**
    this is an explicit implementation of an interactable lamppost
*/
public partial class LampPost : Node, Interactable {
	
	[Export(PropertyHint.Enum, "interactionType")]
	InteractionType interactionType = InteractionType.General;

    public InteractionMethod interactionMethod => throw new NotImplementedException();

    InteractionType Interactable.interactionType => throw new NotImplementedException();

    public override void _Ready()
	{
        //ResourceManager.interactables.Add(this);
        ResourceManager.RegisterGameObject(this, GameObjectType.Interactable);
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
