using System;
using Godot;


/**
    the idea here is to be able to attach this class to something in the godot editor and assign the exported properties from within the editor,
    then in the code, there is a mapping between the names of the interactables and the functions that get executed upon interaction. 
*/
public partial class GenericInteractable : Node, Interactable {
	
	[Export(PropertyHint.Enum, "interactionType")]
	InteractionType interactionType;

    public InteractionMethod interactionMethod => throw new NotImplementedException();

    InteractionType Interactable.interactionType => throw new NotImplementedException();

    public Func<dynamic> interactionFunction;


    public override void _Ready()
	{
        
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
