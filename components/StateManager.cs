using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

/*
* base class for a state machine for any Node3D. it has listeners which are notified when the state changes
*/

[GlobalClass]
public abstract partial class StateManager : Node3D {

	//the states that this thing is allowed to be
	[Export] public Array<State> states; //this has to be set in editor 
	public State currentState;
	[Export] public State defaultState;
	[Export] private Array<Node> listenerNodes; //the thing for which this statemachine manages state
	private HashSet<StateChangeListener> listeners;
	public Node3D stateHolder; //the thing that cares about this state, and knows what state transitions are allowed

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
		listeners = new HashSet<StateChangeListener>();
		if (listenerNodes != null){
			foreach (Node3D ln in listenerNodes){
				listeners.Add((StateChangeListener)ln);
			}
		}
	}

	public void SetState(State state, float duration = -1){
		if (CanChangeState(state)){
			State prevState = currentState;
			currentState = state;
			foreach (StateChangeListener l in listeners){
				l.OnStateChange(state, duration);
			}
		}
	}

	public void SetStateByIndex(int idx){
		currentState = states[idx];	
	}

	public void AddStateListener(StateChangeListener l){
		if (!listeners.Contains(l)){
			listeners.Add(l);
		}
	}

	public abstract bool CanChangeState(State state); //to tell the manager if can switch to given state

}
public interface StateChangeListener {
	void OnStateChange(StateManager.State state, float duration = -1);
}

//an idea
struct StateStruct {
	string label;
	float duration;
	//List<State> nextStates;
}
