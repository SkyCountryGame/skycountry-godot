using System;
using Godot;


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
        Quest = 1<<6, 
        Junk = 1<<7,
        Mineral = 1<<8
    }

    [Export] public string name;
    [Export] public ItemType itemType;
    [Export] public bool equipable;
    [Export] public string worldItemPath; 
    [Export] public PackedScene packedSceneEquipable;
    public int id;
    private static int nextId = 0; //keep count so that id is always unique

    private string packedScenePath; //the path to the scene that will be instantiated if this item is dropped

    public bool inited = false;

    //TODO maybe should keep the constructor so don't have to use godot resources

    //sets up the necessary data for this item to be added to an entity's inventory. e.g. this is usually called when an item is picked up
    public InventoryItem() : this(null, ItemType.Weapon, false, null, null) {}

    public InventoryItem(string name, ItemType itemType, bool equipable, string worldItemPath, PackedScene packedSceneEquipable){
        this.name = name;
        this.itemType = itemType;
        this.equipable = equipable;
        this.worldItemPath = worldItemPath;
        this.packedSceneEquipable = packedSceneEquipable;
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
        return ResourceLoader.Load<PackedScene>(worldItemPath);
    }
    public PackedScene GetPackedSceneEquipable(){
        return packedSceneEquipable;
    }

    /**
     * how effective the given entity is with this item
     */
     /*
    public int GetEffectiveness(Entity entity)
    {
        int res = 0;
        foreach (KeyValuePair<EntityTypes.Skill, int> m in EffectivenessMap)
        {
            res += entity.GetSkill(m.Key) * m.Value;
        }
        return res;
    }*/
}
