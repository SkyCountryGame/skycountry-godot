using Godot;

[GlobalClass]
public partial class InventoryItemProperties : Resource {
    public enum ItemType {
        Weapon = 1<<0,
        Aid = 1<<1,
        Ammo = 1<<2, 
        Apparel = 1<<3,
        Shield = 1<<4, 
        Semantic = 1<<5, 
        Quest = 1<<6, 
        Junk = 1<<7,
        Mineral = 1<<8
    }

    [Export] public string name;
    [Export] public ItemType itemType;
    [Export] public bool equippable;
    //[Export] public string scenePathWorldItem;
    [Export] public PackedScene packedSceneWorldItem;
    //[Export] public string scenePathEquippedItem;
    [Export] public PackedScene packedSceneEquipable;
}