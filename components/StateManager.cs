using Godot;
using System;
using Godot.Collections;

/*
* base class for a state machine for any Node3D. it has listeners which are notified when the state changes
*/

[GlobalClass]
public partial class StateManager : Node3D /*StateHolder*/ {

	//the states that this thing is allowed to be
	[Export] public Array<State> states; //this has to be set in editor 
	public State currentState;
	[Export] public State defaultState;
	[Export] private Node3D subject; //the thing for which this statemachine manages state

	//all of the states that something could be. could be defined by another class, to keep StateManager Node generic
	public enum State { 
		IDLE, //standing still
		ALERT, //something nearby happened, like somebody entered awareness zone, maybe watching or pursuing
		TALKING, 
		ROAMING, //same as idle except moving around 
		ATTACKING, //attacking target
		SLEEPING, 
		ACTION, //performing an action which may also be triggering an event
		DEAD 
	}

	public override void _Ready(){
		base._Ready();
		currentState = defaultState;
		
	}

	public void SetState(State state){
		currentState = state;
		((StateHolder)subject).HandleStateChange(state);
	}

	public void SetStateByIndex(int idx){
		currentState = states[idx];
		((StateHolder)subject).HandleStateChange(states[idx]);
	}

	/* TODO
    public void HandleStateChange(State state)
    {
        throw new NotImplementedException();
    }

    public bool CanChangeState(State state)
    {
        throw new NotImplementedException();
    }*/

}
