using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using Microsoft.VisualBasic;
using System.Collections.Generic;

//bird sits at his nest, flies to another tree, chills for a bit, flies back, and repeats
public partial class Bird : NPCNode, StateHolder {

	[Export] private Array<Node3D> stations = new Array<Node3D>(); //places of interest to the bird (birdnests)
	private LinkedList<Node3D> stationsLL; //store as linkedlist for easy incrementing
	private LinkedListNode<Node3D> stationCurrent; //the curent station of interest. the bird is either here or headed here
	private ActivityTimer activityTimer;

	private MotionModule mot;
	
	public override void _Ready(){
		base._Ready();
		//activityTimer = GetNode<ActivityTimer>("ActivityTimer");
		//activityTimer.Start();
		stationsLL = new LinkedList<Node3D>(stations);
		stationCurrent = stationsLL.First;
		mot = new MotionModule(physBody);
	}

	public override void _Process(double delta){
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		mot.Update(delta);
	}

	//timer timeout to switch from chilling at nest to flying to other nest
	private void SwitchActivity(){
		if (stateManager.currentState == State.IDLE){
			stateManager.SetState(State.ALERT);
		} else if (physBody.Position.DistanceTo(nav.TargetPosition) < .3){ //this is just a quick hack so i dont have to mess with timer for now
			GD.Print("bird idle");
			stateManager.SetState(State.IDLE);
		}
	}

	public void SetState(StateManager.State state)
	{
		switch (state){
			case State.IDLE:
				SetTargetPosition(physBody.Position);
				break;
			case State.ALERT:
				stationCurrent = stationCurrent.Next ?? stationsLL.First;
				GD.Print($"Switching to {stationCurrent.Value.Name}");
				SetTargetPosition(stationCurrent.Value.Position);
				GD.Print($"Flying to {nav.TargetPosition}");
				break;
			default:
				break;
		}
	}

	private void SetTargetPosition(Vector3 pos){
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
}
