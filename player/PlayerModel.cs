using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class PlayerModel : Resource {
	public CharacterBody3D playerNode;
	[Export]
    public State activityState = State.DEFAULT;
	[Export]
    public int hp = 0;
	public Dictionary<State, HashSet<State>> dS; //allowed state transitions, used when updating
	
	[Flags]
	public enum State //maybe activity state? 
	{
		DEFAULT = 1 << 0,
		CHARGING = 1 << 1, 
		ROLLING = 1 << 2, 
		PREPARING = 1 << 3, 
		ATTACKING = 1 << 4,
		COOLDOWN = 1 << 5,
		HEALING = 1 << 6,
		RELOADING = 1 << 7,
		AIMING = 1 << 8, 
		INVENTORY = 1 << 9, 
		DIALOGUE = 1 << 10
	}

	[Export]
	public Inventory inv; //NOTE this might be moved into an Entity superclass 
	//[Export]
	public InventoryItem equipped; 
	
	public PlayerModel(){
		dS = new Dictionary<State, HashSet<State>>();
		dS.Add(State.DEFAULT, new HashSet<State>() { State.ATTACKING, State.CHARGING, State.HEALING, State.PREPARING, State.RELOADING, State.AIMING, State.INVENTORY, State.DIALOGUE });
		dS.Add(State.CHARGING, new HashSet<State>() { State.ROLLING, State.DEFAULT });
		dS.Add(State.ROLLING, new HashSet<State>() { State.DEFAULT });
		dS.Add(State.PREPARING, new HashSet<State>() { State.ATTACKING, State.DEFAULT });
		dS.Add(State.ATTACKING, new HashSet<State>() { State.COOLDOWN, State.DEFAULT });
		dS.Add(State.RELOADING, new HashSet<State>() { State.DEFAULT, State.HEALING, State.AIMING });
		dS.Add(State.COOLDOWN, new HashSet<State>() { State.DEFAULT, State.HEALING, State.RELOADING, State.CHARGING, State.AIMING });
		dS.Add(State.AIMING, new HashSet<State>() { State.DEFAULT, State.ATTACKING, State.HEALING, State.COOLDOWN });
		dS.Add(State.INVENTORY, new HashSet<State>() { State.DEFAULT });
		dS.Add(State.DIALOGUE, new HashSet<State>() { State.DEFAULT });
		inv = new Inventory(4);
	}
    public PlayerModel(CharacterBody3D playerNode) : base() {
		
		this.playerNode = playerNode;
		inv = new Inventory(4);
	}

	public State GetState() { return activityState;}

	//---INVENTORY---	
	public void AddToInventory(InventoryItem item)
    {
        inv.Add(item);
		Global.HUD.UpdateInventoryMenu(inv);
    }

    public void AddToInventory(Inventory inv)
    {
        foreach (InventoryItem ii in inv.GetItems())
        {
            AddToInventory(ii);
        }
    }

    public bool IsInvFull()
    {
        return inv.IsFull();
    }

	/*public List<Wearable> GetActiveArmor()
    {
        return armor;
    }*/

}