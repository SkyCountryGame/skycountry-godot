using System.Collections.Generic;
using System.Linq;
using Godot;

/**
* an inventory of items for any entity that can hold items
*/
public class Inventory : System.ICloneable
{
    public List<InventoryItem> stock; //currently a linked list because we want to cycle through items. but may change to something else if more similar to zelda    
    public InventoryItem primary; //item first in line to be equipped (not the equipped item)
    //public Dictionary<int, InventoryItem> stockIndexed; //for quick access //TODO will only need this if we decide to use a select-anything inv instead of cycle-through

    public int capacity; //max # items can be wwww

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        stock = new List<InventoryItem>(4);
        primary = null;
    }

    public bool Add(InventoryItem item)
    {
        GD.Print($"inv add {item.title}. stock = {stock.Count}");
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
                res += i.title + "(" + i.itemType + ") \n";
        }
        return res;
    }

    public InventoryItem GetItemByID(int itemID)
    {
        return stock.Find(item => item.id == itemID);
    }

    public InventoryItem GetItemByIndex(int idx){
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