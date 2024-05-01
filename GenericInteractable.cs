using System;
using Godot;

public partial class GenericInteractable : Node, Interactable {
	
	[Export(PropertyHint.Enum, "interactionType")]
	InteractionType interactionType;

    public InteractionMethod interactionMethod => throw new NotImplementedException();

    InteractionType Interactable.interactionType => throw new NotImplementedException();

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
        throw new NotImplementedException();
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
