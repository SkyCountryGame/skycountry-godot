using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using System.Collections.Generic;
using System.Threading;

//this thing "patrols" between some set of nodes
[GlobalClass] //TODO not actually a component, but an NPC
public partial class Patroller : NPCNode {

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
		activityTimer.baseWaitTime = 3;
		activityTimer.Start();
		stationsLL = new LinkedList<Node3D>(stations);
		stationCurrent = stationsLL.First;
		//TODO set cycles states from editor 
		cycleStates = new LinkedList<State>(new State[2]{State.IDLE, State.ALERT});
		cycleStateCurrent = cycleStates.First;
	}

	public override void _Process(double delta){
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		switch (currentState){
			case State.IDLE:
				break;
			case State.ALERT:
				mot.pos_goal = nav.TargetPosition;
				GD.Print($"Patroller alert. nav target pos: {nav.TargetPosition}; vel: {physBody.Velocity}; cur pos: {physBody.Position}");
				break;
		}
		mot.Update(delta, physBody);
		if (mot.pos_goal != physBody.GlobalPosition){
			physBody.LookAt(mot.pos_goal);
		}
		/*if (physBody.Velocity.Length() > 0f){
			LookAt(physBody.Velocity, Vector3.Up);
		}*/
	}

	//timer timeout to switch from chilling at nest to flying to other nest
	private void SwitchActivity(){
		GD.Print("Switching activity");
		cycleStateCurrent = cycleStateCurrent.Next ?? cycleStates.First;
		SetState(cycleStateCurrent.Value);
	}

	public override void OnStateChange(State state, float duration = -1)
	{
		base.OnStateChange(state);
		switch (state){
			case State.IDLE:
				SetTargetPosition(physBody.GlobalPosition);
				physBody.Velocity = Vector3.Zero;
				break;
			case State.TALKING:
				SetTargetPosition(physBody.GlobalPosition);
				physBody.Velocity = Vector3.Zero;
				new Thread(() => {
					Thread.Sleep((int)(5 * 1000));
					SetState(State.IDLE);
				}).Start();
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
		return true;
	}

}
