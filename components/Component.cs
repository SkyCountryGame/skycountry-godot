using Godot; 

public partial class Component : Node3D {
    [Export] protected Node3D subject; //the thing of which this is a component

    public override void _Ready(){
        base._Ready();
        if (subject == null){
            subject = GetParent<Node3D>();
        }
    }
}