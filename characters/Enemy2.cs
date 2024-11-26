using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using Microsoft.VisualBasic;
using System.Collections.Generic;

//enemy 1: sits idle, chases player, attacks player when close enough
public partial class Enemy2 : NPCNode, StateHolder {
	
    private Node3D target; 

	public override void _Ready(){
		base._Ready();
        stateManager.SetState(State.IDLE); //dont know if i like to have to call upon a statemgr no matter what just so that godot nodes can interface with from editor for some state-holding-entities
	}

	public override void _Process(double delta){
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        //currently this updates updates every frame, can do via timer
        if (stateManager.currentState == State.ALERT){
            nav.TargetPosition = target.GlobalPosition;
            physBody.Velocity = (nav.TargetPosition - physBody.GlobalPosition).Normalized() *15;
            physBody.MoveAndSlide();
        }
	}

	public void SetState(StateManager.State state)
	{
		switch (state){
			case State.IDLE:
				nav.TargetPosition = physBody.GlobalPosition;
                physBody.Velocity = Vector3.Zero;
				break;
			case State.ALERT: //player is detected. chase
				break;
            case State.ATTACKING: //player is close enough. attack
                break;
			default:
				break;
		}
	}

	public override void HandleCollide(ColliderZone zone, Node other)
	{
		switch (zone) {
            case ColliderZone.Awareness0:
                if (other is Player){
                    target = (Node3D) other;
                    stateManager.SetState(State.ALERT);
                }
                break;
            case ColliderZone.Awareness1:
                if (other is Player){
                    stateManager.SetState(State.ATTACKING);
                    GD.Print("Enemy is attacking you!");
                    Global.hud.ShowAction("Enemy is attacking you!");
                }
                break;
        }
	}

	public override void HandleDecollide(ColliderZone zone, Node other)
	{
		switch (zone) {
            case ColliderZone.Awareness0:
                if (other is Player){
                    //target = (Node3D) other;
                    stateManager.SetState(State.IDLE);
                }
                break;
            case ColliderZone.Awareness1:
                break;
        }
	}
}
