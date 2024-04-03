using System;
using System.Collections.Generic;
using Godot;

/* EntityManager? ObjectManager? GameManager? */
public class ResourceManager {
    public static PackedScene indicator; 
    public static Dictionary<Node3D, WorldObjectInfo> worldObjInfo = new Dictionary<Node3D, WorldObjectInfo>(); //associate each object in the game with info about it
}

public enum ObjectType {Entity, Prop, Structure, Item, Enemy, Friendly, Neutral};

public struct WorldObjectInfo{
    public ObjectType type;
}