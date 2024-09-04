

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

    //public void update(float dt);

}

/**
*   connect a scene node to a GameObject in the editor 
*/
public abstract partial class GameObjectConnector : Node 
{

    public override void _Ready(){
        PrefabManager.RegisterGameObject((Node)this, Name, type);
    }

    [Export(PropertyHint.Enum, "What is the nature of this object? To help things in the game respond to it.")]
    public GameObjectType type {get; set;}

    [Export(PropertyHint.ArrayType, "What quests are this object related to? This could affect gameplay and story.")]
    public int[] questIDs {get; set;}

    [Export(PropertyHint.None, "note for object")]
	public string devinfo {get; set;}
}

public enum GameObjectType {SpawnPoint, Entity, Prop, Structure, Item, Enemy, Friendly, Neutral, Interactable, Light};
//NOTE: currently experimenting with different ways to represent this stuff
public struct WorldObjectInfo{
    public GameObjectType type;
}