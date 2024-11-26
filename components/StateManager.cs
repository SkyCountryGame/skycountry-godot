using Godot;
using System;
using Godot.Collections;

/*
* a state machine for any Node3D. define what states the thing has, then connect the signal StateChange to whatever other nodes
*/

[GlobalClass]
public partial class StateManager : Node3D {

	//the states that this thing is allowed to be
	[Export] public Array<State> states; //this has to be set in editor 
	[Signal] public delegate void StateChangeEventHandler();
	public State currentState;
	[Export] public State defaultState;
	[Export] private Node3D subject; //the thing for which this statemachine manages state

	//all of the states that something could be
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
		this.currentState = state;
		((StateHolder)subject).SetState(state);
	}

	public void SetStateByIndex(int idx){
		this.currentState = states[idx];
		((StateHolder)subject).SetState(states[idx]);
	}
}
