using Godot;
using System;

public partial class Talker : Node, Interactable {
	public InteractionType interactionType { get => InteractionType.Dialogue; }
    public InteractionMethod interactionMethod { get => InteractionMethod.Use; }

    [Export(PropertyHint.None, "dialogue")]
    public Dialogue dialogue;

    public override void _Ready(){
        ResourceManager.RegisterGameObject(this, GameObjectType.Interactable);
    }

    //start dialogue when player interacts
    public dynamic Interact()
    {

        return dialogue.Next();
    }

    public void Retain()
    {
        //wat do?
    }

    public string Info()
    {
       return "A talking thing. ";
    }

    public bool IsInteractionValid(GameObject interactor)
    {
        return true;
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

}
