using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using System.Collections.Generic;

//this thing "patrols" between some set of nodes
[GlobalClass]
public partial class Patroller : NPCNode, StateHolder {

	[Export] private Array<Node3D> stations = new Array<Node3D>();
	[Export] private float radius = 3f; //tavel to a random point within this radius

	private LinkedList<Node3D> stationsLL; //store as linkedlist for easy incrementing
	private LinkedListNode<Node3D> stationCurrent; //currently heading here
	private TimerRandomInterval activityTimer;
	
	public override void _Ready(){
		base._Ready();
		activityTimer = new TimerRandomInterval();
		AddChild(activityTimer);
		activityTimer.Timeout += SwitchActivity;
		activityTimer.baseWaitTime = 1;
		activityTimer.Start();
		stationsLL = new LinkedList<Node3D>(stations);
		stationCurrent = stationsLL.First;
		//TODO set cycles states from editor 
		cycleStates = new LinkedList<State>(new State[3]{State.IDLE, State.ALERT, State.TALKING});
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
				GD.Print($"Patroller alert. nav target pos: {nav.TargetPosition}; vel: {physBody.Velocity}; cur pos: {physBody.Position}");
				break;
		}
		mot.UpdateOld(delta, physBody);
		//Position = physBody.GlobalPosition;
		LookAt(physBody.Velocity, Vector3.Up);
	}

	//timer timeout to switch from chilling at nest to flying to other nest
	private void SwitchActivity(){
		GD.Print("Switching activity");
		cycleStateCurrent = cycleStateCurrent.Next ?? cycleStates.First;
		stateManager.SetState(cycleStateCurrent.Value);
	}

	public override void HandleStateChange(State state)
	{
		base.HandleStateChange(state);
		switch (state){
			case State.IDLE:
			case State.TALKING:
				SetTargetPosition(GlobalPosition);
				physBody.Velocity = Vector3.Zero;
				break;
			case State.ALERT:
				stationCurrent = stationCurrent.Next ?? stationsLL.First;
				SetTargetPosition(stationCurrent.Value.GlobalPosition);
				break;
			default:
				break;
		}
	}

	private void SetTargetPosition(Vector3 pos){
		GD.Print($"{Name} Setting target position: {pos}");
		nav.TargetPosition = pos;
		mot.pos_goal = pos;
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
		throw new NotImplementedException();
	}

}
