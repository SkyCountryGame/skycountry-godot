using System.Collections.Generic;
using System.Linq;
using Godot;

/**
* an inventory of items for any entity that can hold items
*/
[GlobalClass]
public partial class Inventory : Resource, System.ICloneable
{
	public List<InventoryItem> stock; //currently a linked list because we want to cycle through items. but may change to something else if more similar to zelda    
	//TODO "favorite" items. able to be equipped with some hotkey? 
	//public Dictionary<int, InventoryItem> stockIndexed; //for quick access //TODO will only need this if we decide to use a select-anything inv instead of cycle-through

	[Export]
	public int capacity = 4; //max # items can be wwww

	public Inventory()
	{
		stock = new List<InventoryItem>(capacity);
	}

	public Inventory(int capacity = 1)
	{
		this.capacity = capacity;
		stock = new List<InventoryItem>(capacity);
	}

	public bool Add(InventoryItem item)
	{
		GD.Print($"inv add {item.name}. stock = {stock.Count}");
		if (stock.Count < capacity)
		{
			stock.Add(item);
			return true;
		}
		return false;
	}
	public bool IsFull()
	{
		return stock.Count == capacity;
	}
	public bool IsEmpty()
	{
		return stock.Count == 0;
	}

	override
	public string ToString()
	{
		string res = stock.Count + "/" + capacity + "\n";
		foreach (InventoryItem i in stock)
		{
				res += i.name + "(" + i.GetItemType() + ") \n";
		}
		return res;
	}

	public InventoryItem GetItemByID(int itemID) //NOTE to adam (dama33): this is why i wanted to use a hashmap. 
	{
		return stock.Find(item => item.id == itemID);
	}

	public InventoryItem GetItemByIndex(int idx){
		if (idx < 0 || idx >= stock.Count) return null;
		return stock[idx];
	}

	public List<InventoryItem> GetItems()
	{
		return stock;
	}

	public bool RemoveItem(InventoryItem item)
	{
		return stock.Remove(item);
	}

	public bool Contains(InventoryItem item)
	{
		return stock.Contains(item);
	}

	public int Count()
	{
		return stock.Count;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
