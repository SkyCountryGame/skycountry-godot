using Godot;

//an item that sits somewhere in the game world that can be picked up and added to inventory or consumed in some way
//PAYLOAD: InventoryItem
[GlobalClass]
public partial class Pickup : Node3D, Interactable
{
    [Export] public InventoryItem invItem {get; set;} //the pickup item to be added to inventory
    //private Node3D worldItem; //the actually thing in the world after it is instantiated on scene load
    [Export] public string info; //short description to show when player can pick it up

    public InteractionType interactionType => InteractionType.Pickup;
    public InteractionMethod interactionMethod => InteractionMethod.Use;

    public override void _Ready()
    {
        base._Ready();
        if (invItem == null){
            GD.PrintErr("Pickup item not set");
        }
        //TODO remove. this is from 
        //worldItem = (Node3D) invItem.GetPackedSceneWorldItem().Instantiate();
        //worldItem.GlobalPosition = GlobalPosition;
        //AddChild(worldItem);
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
    }

    public void Clear()
    {
        return;
    }

    //PAYLOAD: the inventory item for this pickupable world item
    public dynamic Interact()
    {
        QueueFree();
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
