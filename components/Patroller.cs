using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using System.Collections.Generic;
using System.Threading;

//this thing "patrols" between some set of nodes
[GlobalClass] //TODO not actually a component, but an NPC
public partial class Patroller : NPCNode, EventListener {

	[Export] private Array<Node3D> stations = new Array<Node3D>();
	[Export] private float radius = 3f; //tavel to a random point within this radius

	private LinkedList<Node3D> stationsLL; //store as linkedlist for easy incrementing
	private LinkedListNode<Node3D> stationCurrent; //currently heading here
	private TimerRandomInterval activityTimer;

	public HashSet<EventType> eventTypes => new HashSet<EventType>(){EventType.DialogueStart, EventType.DialogueEnd}; 

	public override void _Ready(){
		base._Ready();
		EventManager.RegisterListener(this);
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
				if  (navReady){
					mot.pos_goal = nav.GetNextPathPosition();
				}
				//GD.Print($"Patroller alert. nav target pos: {nav.TargetPosition}; vel: {physBody.Velocity}; cur pos: {physBody.Position}");
				break;
		}
		mot.Update(delta, physBody);
		if (mot.pos_goal.X != physBody.GlobalPosition.X && mot.pos_goal.Z != physBody.GlobalPosition.Z){
			physBody.LookAt(new Vector3(mot.pos_goal.X, physBody.GlobalPosition.Y, mot.pos_goal.Z), Vector3.Up);
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
				activityTimer.Start();
				SetTargetPosition(physBody.GlobalPosition);
				physBody.Velocity = Vector3.Zero;
				break;
			case State.TALKING:
				activityTimer.Stop();
				SetTargetPosition(physBody.GlobalPosition);
				physBody.Velocity = Vector3.Zero;
				break;
			case State.ALERT:
				activityTimer.Start();
				stationCurrent = stationCurrent.Next ?? stationsLL.First;
				SetTargetPosition(stationCurrent.Value.GlobalPosition);
				break;
			default:
				break;
		}
	}

	private void SetTargetPosition(Vector3 pos){
		Vector3 navPos = Global.currentLevel.GetNavPoint(pos);
		nav.TargetPosition = navPos;
		mot.pos_goal = navPos;
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

	public void HandleEvent(Event e)
	{
		if (eventTypes.Contains(e.eventType)){
			switch (e.eventType){
				case EventType.DialogueStart:
					if (e.payload == Global.GetGameObject(this)){
						SetState(State.TALKING);
					}
					break;
				case EventType.DialogueEnd:
					if (currentState == State.TALKING){
						SetState(State.IDLE);
					}
					break;
			}
		}
	}
}
