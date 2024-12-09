using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using Microsoft.VisualBasic;
using System.Collections.Generic;

//bird sits at his nest, flies to another tree, chills for a bit, flies back, and repeats
public partial class Bird : NPCNode {

	[Export] private Array<Node3D> stations = new Array<Node3D>(); //places of interest to the bird (birdnests)
	private LinkedList<Node3D> stationsLL; //store as linkedlist for easy incrementing
	private LinkedListNode<Node3D> stationCurrent; //the curent station of interest. the bird is either here or headed here
	private TimerRandomInterval activityTimer;
	
	public override void _Ready(){
		base._Ready();
		activityTimer = GetNode<TimerRandomInterval>("Timer");
		//activityTimer = GetNode<ActivityTimer>("ActivityTimer");
		//activityTimer.Start();
		stationsLL = new LinkedList<Node3D>(stations);
		stationCurrent = stationsLL.First;
		mot = new MotionModule();
		cycleStates = new LinkedList<State>(new State[2]{State.IDLE, State.ALERT});
		cycleStateCurrent = cycleStates.First;
	}

	public override void _Process(double delta){
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		switch (stateManager.currentState){
			case State.IDLE:
				break;
			case State.ALERT:
				mot.pos_goal = nav.TargetPosition;
				break;
		}
		mot.Update(delta, physBody);
	}

	//timer timeout to switch from chilling at nest to flying to other nest
	private void SwitchActivity(){		
		if (stateManager.currentState == State.IDLE){
			stateManager.SetState(State.ALERT);
			activityTimer.Reset(State.ALERT);
		} else {
			stateManager.SetState(State.IDLE);
		}
	}

	public override void OnStateChange(StateManager.State state)
	{
		base.OnStateChange(state);
		switch (state){
			case State.IDLE:
				SetTargetPosition(physBody.GlobalPosition);
				break;
			case State.ALERT:
				stationCurrent = stationCurrent.Next ?? stationsLL.First;
				SetTargetPosition(stationCurrent.Value.Position);
				break;
			default:
				break;
		}
	}

	private void SetTargetPosition(Vector3 pos){
		nav.TargetPosition = pos;
	}

	public override void HandleCollide(ColliderZone zone, Node other)
	{
		throw new NotImplementedException();
	}

	public override void HandleDecollide(ColliderZone zone, Node other)
	{
		throw new NotImplementedException();
	}

	public override bool CanChangeState(State state)
	{
		return true;
	}

}
