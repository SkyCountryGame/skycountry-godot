using System;
using Godot;
using Prefab = PrefabManager;

//TODO there is still some sorting out to do with the Node vs Model situation
[GlobalClass]
public partial class InventoryItem : Resource, System.ICloneable {
    public enum ItemType {
        Weapon = 1<<0,
        Aid = 1<<1,
        Ammo = 1<<2, 
        Apparel = 1<<3,
        Shield = 1<<4, 
        Semantic = 1<<5,
        Tool = 1<<6, 
        Quest = 1<<7,
        Junk = 1<<8,
        Mineral = 1<<9
    }

    [Export] public string name;
    [Export] public ItemType itemType;
    [Export] public bool equipable;
    
    public int id;
    private static int nextId = 0; //keep count so that id is always unique

    public bool inited = false;

    //TODO maybe should keep the constructor so don't have to use godot resources

    //sets up the necessary data for this item to be added to an entity's inventory. e.g. this is usually called when an item is picked up
    public InventoryItem() : this(null, ItemType.Weapon, false, null, null) {}

    public InventoryItem(string name, ItemType itemType, bool equipable, PackedScene packedSceneWorldItem, PackedScene packedSceneEquipable){
        this.name = name;
        this.itemType = itemType;
        this.equipable = equipable;
    }

    public void Init()
    {
        if (inited) return;
        //do init stuff
        inited = true;
    }

    override public string ToString()
    {
        return itemType.ToString() + ": " + name;
    }

    public override int GetHashCode()
    {
        return id + name.GetHashCode();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public PackedScene GetPackedSceneWorldItem(){
        /*if (packedScene == null){
            packedScene = ResourceLoader.Load<PackedScene>(packedScenePath);
            Global.prefabs[name] = packedScene; //for now these are indexed by invitem name but will probably be something else in future
        }*/
        return Prefab.Get(name+"_pickup");
    }
    public PackedScene GetPackedSceneEquipable(){
        return Prefab.Get(name+"_equip");
    }
}
