using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class StateManager : Node3D {
    [Export] public Array<string> states = new Array<string>(); 
    [Signal] public delegate void StateChangeEventHandler();
    public string currentState;

    public override void _Ready(){
        base._Ready();
        currentState = states[0];
    }

    public void SetState(string state){
        this.currentState = state;
        EmitSignal(SignalName.StateChange, state);
    }

    public void SetStateByIndex(int idx){
        this.currentState = states[idx];
        EmitSignal(SignalName.StateChange, states[idx]);
    }
}