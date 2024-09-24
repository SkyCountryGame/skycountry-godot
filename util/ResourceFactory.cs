using Godot;
using System;

//generate generic and random resources such as inventories, items, etc for NPCs
public class ResourceFactory {
    public static Inventory MakeInventory(){
        Inventory inv = new Inventory(4);
        inv.Add(new InventoryItem()); //todo some random items
        
        return inv;
    }
}