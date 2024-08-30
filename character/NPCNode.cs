using Godot;

public partial class NPCNode : Node3D {

    [Export] public NPCModel m;

    public override void _Ready(){

        m = new NPCModel("Bob", "A friendly NPC");
    }

    public override void _Process(double delta){
        switch (m.state.Item1){
            case NPCModel.State.IDLE:
                break;
            case NPCModel.State.TALKING:
                break;
            case NPCModel.State.ROAMING:
                break;
            case NPCModel.State.ATTACKING:
                break;
            case NPCModel.State.SLEEPING:
                break;
            case NPCModel.State.ACTION:
                break;
            case NPCModel.State.DEAD:
                break;
        }
    }
}