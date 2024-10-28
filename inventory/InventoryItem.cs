using System;
using Godot;
using static InventoryItemProperties;


//TODO there is still some sorting out to do with the Node vs Model situation
[GlobalClass]
public partial class InventoryItem : Resource, System.ICloneable {
    public int id;
    private static int nextId = 0; //keep count so that id is always unique
    public string name;    
    public PackedScene packedScene; //the scene that will be instantiated if this item is dropped 

    private string packedScenePath; //the path to the scene that will be instantiated if this item is dropped

    public bool inited = false;
    [Export] private InventoryItemProperties properties = new InventoryItemProperties();

    public InventoryItem() : base() { }

    //name is the same as the key in the GameObjectManager.gameObjectsPacked
    public InventoryItem(ItemType t, string name, bool equippable = false) : base()
    {
        properties.itemType = t;
        this.name = name;
        id = nextId++;
        
        //if the packedscene is already loaded, use that, otherwise keep the path to load it later if need be
        if (Global.prefabs.ContainsKey(this.name) && Global.prefabs[this.name] != null){
            packedScene = Global.prefabs[this.name];
        }
        if (ResourceLoader.Exists($"res://gameobjects/{this.name}.tscn")){
            packedScenePath = $"res://gameobjects/{this.name}.tscn";
        }
        if (packedScene == null && packedScenePath == null) {
            packedScene = Global.prefabs["ERROR"];
        }
    }

    //sets up the necessary data for this item to be added to an entity's inventory. e.g. this is usually called when an item is picked up
    public void Init()
    {
        if (inited) return;
        //do init stuff
        inited = true;
    }

    override
    public string ToString()
    {
        return properties.itemType.ToString() + ": " + name;
    }

    public override int GetHashCode()
    {
        return id + name.GetHashCode();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public PackedScene GetPackedScene(){
        if (packedScene == null){
            packedScene = ResourceLoader.Load<PackedScene>(packedScenePath);
            Global.prefabs[name] = packedScene; //for now these are indexed by invitem name but will probably be something else in future
        }
        return packedScene;
    }

    public ItemType GetItemType(){
        return properties.itemType;
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
