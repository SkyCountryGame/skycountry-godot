using System.Collections.Generic;
using System.Linq;
using Godot;

/**
* an inventory of items for any entity that can hold items
*/
public class Inventory : System.ICloneable
{
    private LinkedList<InventoryItem> stock; //currently a linked list because we want to cycle through items. but may change to something else if more similar to zelda    
    public LinkedListNode<InventoryItem> primary; //item first in line to be equipped (not the equipped item)
    //public Dictionary<int, InventoryItem> stockIndexed; //for quick access //TODO will only need this if we decide to use a select-anything inv instead of cycle-through

    public int capacity; //max # items can be wwww

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        stock = new LinkedList<InventoryItem>();
        primary = stock.First; //yes it's currently null
    }

    public bool Add(InventoryItem item)
    {
        GD.Print($"inv add {item.title}. stock = {stock.Count}");
        if (stock.Count < capacity)
        {
            stock.AddLast(new LinkedListNode<InventoryItem>(item));
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

    /**
     * grab the currently most primary item
     */
    public InventoryItem GetFirstItem()
    {
        if (primary.Value == null) return null;
        InventoryItem res = primary.Value;
        primary = primary.Next ?? stock.First; //if next is null (end of list) cycle back to beginning
        return res;
    }

    public List<InventoryItem> GetFirstNItems(int n)
    {
        List<InventoryItem> res = new List<InventoryItem>();
        for (int i = 0; i < n; i++)
        {
            if (stock.Count > 0)
            {
                res.Add(GetFirstItem());    
            }
            else
            {
                break;
            }    
        }

        return res;
    }

    public InventoryItem GetItem(int itemID)
    {
        InventoryItem res = primary.Value;
        while (res != null){
            if (res.id == itemID){
                return res;
            }
            res = primary.Next?.Value;
        }
        return res;
    }

    public List<InventoryItem> GetItems()
    {
        return stock.ToList();
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
        return this.MemberwiseClone();
    }
}