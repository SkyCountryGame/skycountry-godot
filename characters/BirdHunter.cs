using Godot;
using System;

public partial class BirdHunter : NPCNode {
    public override void _Ready(){
        base._Ready();
        m.inv = ResourceFactory.MakeInventory();
    }

    public override void _Process(double delta){
        base._Process(delta);
        switch (m.state){
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

    public void OnStateTimeout(){
    }
}