using System;
using Godot;

public class InventoryItem : System.ICloneable {
    public int id;
    private static int nextId = 0; //keep count so that id is always unique
    public string name; 
    public enum ItemType {Weapon, Aid, Ammo, Apparel, Shield, Semantic, Quest, Junk, Mineral};
    public ItemType itemType;

    //TODO establish all the properties than inventory items can have

    public PackedScene packedScene; //the scene that will be instantiated if this item is dropped 

    private string packedScenePath; //the path to the scene that will be instantiated if this item is dropped

    private bool equippable;

    public bool inited = false;
    //mass? volume? other properties

    //from old unity code. part of the skill system
    //public Dictionary<EntityTypes.Skill, int> EffectivenessMap; //each skill's weight on an entities effectiveness with this item

    public InventoryItem() : base() { }

    //name is the same as the key in the GameObjectManager.gameObjectsPacked
    public InventoryItem(ItemType t, string name, bool equippable = false) : base()
    {
        itemType = t;
        this.name = name;
        id = nextId++;
        
        //if the packedscene is already loaded, use that, otherwise keep the path to load it later if need be
        if (SceneManager._.prefabs.ContainsKey(this.name)){
            packedScene = SceneManager._.prefabs[this.name];
        } else if (ResourceLoader.Exists($"res://gameobjects/{this.name}.tscn")){
            packedScenePath = $"res://gameobjects/{this.name}.tscn";
        } else {
            packedScene = SceneManager._.prefabs["ERROR"];
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
        if (packedScene != null){
            return packedScene;
        } else {
            return ResourceLoader.Load<PackedScene>(packedScenePath);
        }
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