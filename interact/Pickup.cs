//an item that sits somewhere in the game world that can be picked up and added to inventory or consumed in some way
//PAYLOAD: Tuple<PickupType, dynamic> where the 2nd element is the pickup object    
public class Pickup : Interactable
{
    //the pickup type dictates what variable type the thing is, and what will happen with player, e.g. add to inv, gib health, ammo, etc.
    public enum PickupType { 
        Item, //InventoryItem 
        HP, Ammo, //int
        PlayerEffect, //function that takes player as arg and does something to it
          /* XP, Money, Energy, ??? */ };
    private dynamic thing; //the pickup item, whether it be an inventory item or something the player has to carry or just some hp
    private PickupType type;

    public InteractionType interactionType => InteractionType.Pickup;
    public InteractionMethod interactionMethod => InteractionMethod.Contact;

    public Pickup(PickupType type, dynamic thing = null)
    {
        this.type = type;
        this.thing = thing;
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public string Info()
    {
        throw new System.NotImplementedException();
    }

    //PAYLOAD: Tuple<PickupType, dynamic> where the 2nd element is the pickup object    
    public dynamic Interact()
    {
        return (type, thing);
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        return true;
    }

    public void Retain()
    {
        throw new System.NotImplementedException();
    }
}