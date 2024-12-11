using Godot;

//an item that sits somewhere in the game world that can be picked up and added to inventory or consumed in some way
//PAYLOAD: InventoryItem
[GlobalClass]
public partial class Pickup : Component, Interactable
{
    [Export] public InventoryItem invItem {get; set;} //the pickup item to be added to inventory
    [Export] public string info; //short description to show when player can pick it up

    public InteractionType interactionType => InteractionType.Pickup;
    public InteractionMethod interactionMethod => InteractionMethod.Use;
    //TODO hold reference to ingame item to destory when interact

    public override void _Ready()
    {
        base._Ready();
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
        GD.Print($"{GetPath()} pickup");
    }

    public void Clear()
    {
        QueueFree();
    }

    //PAYLOAD: the inventory item for this pickupable world item
    public dynamic Interact()
    {
        return invItem;
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        return true;
    }

    public void Retain()
    {
        throw new System.NotImplementedException();
    }

    public string Info(){
        return info;
    }

}
