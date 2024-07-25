public class InventoryItem : System.ICloneable {
    public int id;
    public string title;
    public enum ItemType {Weapon, Aid, Ammo, Apparel, Shield, Semantic, Quest, Junk, Mineral};
    public ItemType itemType;
    public bool inited = false;
    //mass? volume? other properties

    //from old unity code. part of the skill system
    //public Dictionary<EntityTypes.Skill, int> EffectivenessMap; //each skill's weight on an entities effectiveness with this item

    public InventoryItem() : base() { }

    public InventoryItem(ItemType t, string title) : base()
    {
        itemType = t;
        this.title = title;
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

    public int GetHashCode()
    {
        return id + title.GetHashCode();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
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