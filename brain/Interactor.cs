using Godot;
using System.Collections.Generic;

public interface Interactor
{

    //this method will be called somewhere and is where the Interactor will get the payload by calling i.Interact()
    void HandleInteract(Interactable i, Node interactionObj);

    //returns the highest priority interactable, which is almost always the closest one
    Interactable GetFirstInteractable();
}
