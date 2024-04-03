

using Godot;

/**
    * controller for game objects (the things that extend godot classes and are attached to the scene nodes)
    */
public abstract class ObjectController {
    protected Node _o; //the in-game object

    public ObjectController(Node o){
        _o = o;
    }

    public abstract void update(float dt);

}