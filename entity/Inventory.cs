using System.Collections.Generic;
using System.Linq;
using Godot;

public class Inventory : System.ICloneable
{
    private LinkedList<InventoryItem> stock; //currently a linked list because we want to cycle through items. but may change to something else if more similar to zelda
    private LinkedListNode<InventoryItem> primary;

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
            stock.AddLast(new LinkedListNode<InventoryItem>((InventoryItem)item.Clone()));
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

    public List<InventoryItem> GetItems()
    {
        return stock.ToList();
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