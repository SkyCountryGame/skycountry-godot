

using System;
using System.Collections.Generic;
using Godot;

/**
    * the idea behind this is that to have a representation of the game itself, separate from godot engine. 
    */
public class GameObject {
    protected Node gdNode; //the godot scene node. TODO remove
    public Guid id;
    public string desc_short;
    public string desc_long;
    public GameObjectType type;
    
    public GameObject(GameObjectType type){
        id = Guid.NewGuid();
        this.type = type;
    }
    //TODO i think that GameObject doesn't need to hold its nodes. 
    public GameObject(Node node){
        gdNode = node;
    }
}

//TODO what types and what are their responsibilities? what flow-of-logic does each type signal to the game (the elements of the game)?
public enum GameObjectType {
    SpawnPoint = 1 << 0, //entities can spawn here 
    WorldItem = 1 << 1,  //physical object. rock, sword, pumpkin, tree
    Equipable = 1 << 2, //a tool to use or something to wear
    Entity = 1 << 3, //an NPC, player, enemy, projectile, 
    Structure = 1 << 4, //
    Enemy = 1 << 5, Friendly = 1 << 6, Neutral = 1 << 7, 
    Interactable = 1 << 8, //player needs to know when near one of these 
    Light = 1 << 9,
    Player = 1 << 10,
    Level = 1 << 11,
};

public interface GameObjectNodeProvider {
    List<Node3D> GetNodes(); //returns all the nodes that comprise this game object
}

//GameObjectPropertiesResource? or is that dealt with for specific types?