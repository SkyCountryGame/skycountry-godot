using System;
using Godot;

public class InventoryItem : System.ICloneable {
    public int id;
    private static int nextId = 0; //keep count so that id is always unique
    public string title;
    public enum ItemType {Weapon, Aid, Ammo, Apparel, Shield, Semantic, Quest, Junk, Mineral};
    public ItemType itemType;
    public Node3D gameObject; //godot node for the world object. this was either picked up off ground or is instantiated elsewhere if this inv item wasn't ever in the world
    public bool inited = false;
    //mass? volume? other properties

    //from old unity code. part of the skill system
    //public Dictionary<EntityTypes.Skill, int> EffectivenessMap; //each skill's weight on an entities effectiveness with this item

    public InventoryItem() : base() { }

    public InventoryItem(ItemType t, string title) : base()
    {
        itemType = t;
        this.title = title;
        id = nextId++;
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
        return itemType.ToString() + ": " + title;
    }

    public override int GetHashCode()
    {
        return id + title.GetHashCode();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void SetGameObject(Node3D obj){
        gameObject = obj;
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