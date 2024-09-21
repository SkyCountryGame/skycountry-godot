using System;
using Godot;
using static ItemProperties;
public partial class InventoryItem : Resource, ICloneable {
    public int id;
    private static int nextId = 0; //keep count so that id is always unique
    public string name; 
    public PackedScene packedScene; //the scene that will be instantiated if this item is dropped 
    public ItemType itemType;
    public bool equippable;

    //TODO establish all the properties than inventory items can have
    private string packedScenePath; //the path to the scene that will be instantiated if this item is dropped
    [Export]
    public ItemProperties itemProperties;

    public bool inited = false;
    //mass? volume? other properties

    //from old unity code. part of the skill system
    //public Dictionary<EntityTypes.Skill, int> EffectivenessMap; //each skill's weight on an entities effectiveness with this item

    public InventoryItem() : base() { }

    //name is the same as the key in the GameObjectManager.gameObjectsPacked
    public InventoryItem(ItemProperties itemProperties, bool equippable = false) : base()
    {
        itemType = itemProperties.itemType;
        name = itemProperties.name;
        id = nextId++;
        this.equippable = equippable;
        this.itemProperties = itemProperties;
        //if the packedscene is already loaded, use that, otherwise keep the path to load it later if need be
        if (ResourceLoader.Exists($"res://gameobjects/{name}.tscn")){
            packedScenePath = $"res://gameobjects/{name}.tscn";
        } else {
            packedScene = SceneManager._.prefabs["ERROR"];
        }
    }

    public InventoryItem(ItemType t, string name){

        itemType = t;
        this.name = name;
        equippable = false;
        //if the packedscene is already loaded, use that, otherwise keep the path to load it later if need be
        if (SceneManager._.prefabs.ContainsKey(this.name) && SceneManager._.prefabs[this.name] != null){ //should we remove this? theres not a ton of reason to do this i think as of now.
            packedScene = SceneManager._.prefabs[this.name];
        } 
        if (ResourceLoader.Exists($"res://gameobjects/{this.name}.tscn")){
            packedScenePath = $"res://gameobjects/{this.name}.tscn";
        }
        if (packedScene == null && packedScenePath == null) {
            packedScene = SceneManager._.prefabs["ERROR"];
        }

    }

     public InventoryItem(ItemType t, string name, PackedScene packedScene) : base()
    {
        itemType = t;
        this.name = name;
        id = nextId++;
        equippable = (t & (ItemType.Weapon | ItemType.Apparel | ItemType.Shield)) != 0; 
        this.packedScene = packedScene;
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

    public PackedScene GetPackedScene(){
        if (packedScene == null){
            packedScene = ResourceLoader.Load<PackedScene>(packedScenePath);
            SceneManager._.prefabs[name] = packedScene; //for now these are indexed by invitem name but will probably be something else in future
        }
        return packedScene;
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
