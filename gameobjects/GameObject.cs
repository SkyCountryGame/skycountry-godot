

using Godot;

/**
    * the idea behind this is that to have a representation of the game itself, separate from godot engine. 
    */
public class GameObject {
    protected Node gdNode; //the godot scene node
    public string desc_short;
    public string desc_long;
    
    public GameObject(Node node){
        gdNode = node;
    }
}

public enum GameObjectType {
    SpawnPoint, 
    WorldItem, 
    Equipable, 
    Entity, 
    Structure, 
    Enemy, Friendly, Neutral, 
    Interactable, 
    Light
};

//GameObjectPropertiesResource? or is that dealt with for specific types?