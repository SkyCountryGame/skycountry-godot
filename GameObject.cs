

using Godot;

/**
    * 
    */
public abstract class GameObject {
    protected Node gdNode; //the godot scene node

    public GameObject(Node node){
        gdNode = node;
    }

    public abstract void update(float dt);

}

/**
*   connect a scene node to a GameObject in the editor 
*/
public abstract partial class GameObjectConnector : Node 
{

    public override void _Ready(){
        //ResourceManager.RegisterGameObject((Node)this);
    }

    [Export(PropertyHint.Enum, "What is the nature of this object? To help things in the game respond to it.")]
    public ObjectType type {get; set;}

    [Export(PropertyHint.ArrayType, "What quests are this object related to? This could affect gameplay and story.")]
    public int[] questIDs {get; set;}

    [Export(PropertyHint.None, "note for object")]
	public string devinfo {get; set;}
}