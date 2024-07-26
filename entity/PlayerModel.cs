using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

public class PlayerModel {
	private CharacterBody3D playerNode;
    public State activityState = State.DEFAULT;
    public int hp = 0;
	Dictionary<State, HashSet<State>> dS; //allowed state transitions, used when updating
	
	[System.Flags]
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

	public Inventory inv; //NOTE this might be moved into an Entity superclass 
	public InventoryItem equipped; 

    public PlayerModel(CharacterBody3D playerNode){
		this.playerNode = playerNode;
        dS = new Dictionary<State, HashSet<State>>();
		dS.Add(State.DEFAULT, new HashSet<State>() {State.CHARGING, State.HEALING, State.PREPARING, State.RELOADING, State.AIMING, State.INVENTORY, State.DIALOGUE});
		dS.Add(State.CHARGING, new HashSet<State>() { State.ROLLING, State.DEFAULT });
		dS.Add(State.ROLLING, new HashSet<State>() { State.DEFAULT });
		dS.Add(State.PREPARING, new HashSet<State>() { State.ATTACKING, State.DEFAULT });
		dS.Add(State.ATTACKING, new HashSet<State>() { State.COOLDOWN });
		dS.Add(State.RELOADING, new HashSet<State>() { State.DEFAULT, State.HEALING, State.AIMING });
		dS.Add(State.COOLDOWN, new HashSet<State>() { State.DEFAULT, State.HEALING, State.RELOADING, State.CHARGING, State.AIMING });
		dS.Add(State.AIMING, new HashSet<State>() { State.DEFAULT, State.ATTACKING, State.HEALING, State.COOLDOWN });
		dS.Add(State.INVENTORY, new HashSet<State>() { State.DEFAULT });
		dS.Add(State.DIALOGUE, new HashSet<State>() { State.DEFAULT });
		inv = new Inventory(4);
	}

	/**
	  * logic to perform when switching states
	  */
    public bool UpdateState(State ps){
		State prev = activityState; //some states need to know previous
		if (dS[activityState].Contains(ps)){
			activityState = ps;
			switch (activityState){
				case State.DEFAULT:
					break;
				case State.CHARGING:
					break;
				case State.ROLLING:
					break;
				case State.PREPARING:
					break;
				case State.ATTACKING:
					break;
				case State.COOLDOWN:
					break;
				case State.HEALING:
					break;
				case State.RELOADING:
					break;
				case State.AIMING:
					break;
				case State.INVENTORY:
					//Global.HUD.ShowInventory();
					GD.Print("show inventory");
					break;
				case State.DIALOGUE:
					break;
			}
		} else {
			return false;
		}
		return true;
	}

	//---INVENTORY---	
	public void AddToInventory(InventoryItem item)
    {
        inv.Add(item);
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

	/** by default equip the primary item, or give an item to equip */
	public bool EquipItem(InventoryItem item = null){
		if (inv.IsEmpty()) return false;
		if (item == null){ //or maybe dequip?
			equipped = inv.GetItemByIndex(0);
			Global.HUD.ShowEquipped(equipped.title);
		} else {
			if (inv.Contains(item)){
				equipped = item;
			}
		}
		return equipped != null;
	}

	/** drop the equipped item, or a specific item */
	public bool DropItem(InventoryItem item = null){
		if (inv.IsEmpty()) return false;
		if (item == null){
			item = equipped;
		}
		if (inv.RemoveItem(item)){
			item.gameObject.Position = playerNode.GlobalPosition + new Vector3(0,1,1); //TODO pos placement
			playerNode.GetParent().AddChild(item.gameObject);
			if (item == equipped){
				equipped = null;
			}
			Global.HUD.ShowEquipped();
			return true;
		}
		return false;
	}

	/*public List<Wearable> GetActiveArmor()
    {
        return armor;
    }*/

}