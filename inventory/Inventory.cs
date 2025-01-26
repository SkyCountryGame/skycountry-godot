using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;

/**
* an inventory of items for any entity that can hold items
*/
[GlobalClass]
public partial class Inventory : Resource, System.ICloneable
{
	//TODO "favorite" items. able to be equipped with some hotkey? 
	//public Dictionary<int, InventoryItem> stockIndexed; //for quick access //TODO will only need this if we decide to use a select-anything inv instead of cycle-through

	[Export] public int capacity = 2; //max # items can be held
	[Export] private Godot.Collections.Array<InventoryItem> stock;
	

	public Inventory()
	{
		if (stock == null) stock = new Godot.Collections.Array<InventoryItem>();
	}
	public Inventory(int capacity)
	{
		this.capacity = capacity;
		if (stock == null) stock = new Godot.Collections.Array<InventoryItem>();
	}
	/*
	public override void _SetupLocalToScene(){
		if (stock == null){
			stock = new Godot.Collections.Array<InventoryItem>();
		}
		stockList = stock.ToList();
	}*/

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
				res += i.name + "(" + i.itemType + ") \n";
		}
		return res;
	}

	public InventoryItem GetItemByID(int itemID) //NOTE to adam (dama33): this is why i wanted to use a hashmap. 
	{
		return stock.ToList().Find(item => item.id == itemID);
	}

	public Dictionary<string, int> GetDetailedItemList(){
		Dictionary<string, int> itemCounts = new Dictionary<string, int>();
		foreach(InventoryItem inventoryItem in stock){
			if(!itemCounts.ContainsKey(inventoryItem.name)){
				itemCounts.Add(inventoryItem.name, 1);
			}
			else {
				itemCounts[inventoryItem.name]=itemCounts[inventoryItem.name]+1;
			}
		}
		return itemCounts;
	}

	public InventoryItem GetItemByIndex(int idx){
		if (idx < 0 || idx >= stock.Count) return null;
		return stock[idx];
	}

	public Godot.Collections.Array<InventoryItem> GetItems()
	{
		return stock;
	}

	public bool RemoveItem(InventoryItem item)
	{
		return stock.Remove(item);
	}

	public bool RemoveItemByName(string name){
		return stock.Remove(stock.ToList().Find(item => item.name == name));
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
