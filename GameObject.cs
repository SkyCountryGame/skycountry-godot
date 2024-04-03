

using Godot;

/**
    * to associate data with generic game objects, and have the option to give it a controller for specific functionalities
    */
public abstract partial class GameObject : Node3D {
    protected Node _o; //the in-game object
    //controller here

    [Export(PropertyHint.Enum, "What is the nature of this object? To help things in the game respond to it.")]
    public ObjectType type {get; set;}

    [Export(PropertyHint.ArrayType, "What quests are this object related to? This could affect gameplay and story.")]
    public int[] questIDs {get; set;}

    [Export(PropertyHint.None, "note for object")]
	public string devinfo {get; set;}


    public GameObject(Node o){
        _o = o;
    }

    public abstract void update(float dt);

}