using Godot;
using System.Collections.Generic;

public interface Interactor
{

    //pass the payload from interactable to the implementer
    void HandleInteract(Node interactionObj, dynamic payload);

    //returns the highest priority interactable, which is almost always the closest one
    Interactable GetFirstInteractable();
}
