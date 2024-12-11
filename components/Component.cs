using Godot; 

public partial class Component : Node3D {
    [Export] protected /*StateManager*/ Node3D subject; //the thing of which this is a component. it's always a StateManager

    public override void _Ready(){
        base._Ready();
        if (subject == null){
            subject = GetParent<Node3D>();
        }
    }
}