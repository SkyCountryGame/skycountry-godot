using System;

public class Talker : Interactable {
	public InteractionType interactionType { get => InteractionType.Dialogue; }
    public InteractionMethod interactionMethod { get => interactionMethod.Use; }

    [Export("dialogue")]
    public Dialogue dialogue;

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
}
