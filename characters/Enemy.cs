using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using Microsoft.VisualBasic;
using System.Collections.Generic;

//enemy 1: sits idle, chases player, attacks player when close enough
public partial class Enemy : NPCNode, StateHolder {
	
    private Node3D target; 

    private MotionModule mot;

	public override void _Ready(){
		base._Ready();
        stateManager.SetState(stateManager.defaultState); //dont know if i like to have to call upon a statemgr no matter what just so that godot nodes can interface with from editor for some state-holding-entities
        mot = new MotionModule(physBody);
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
            case State.ROAMING:
                if (navReady && physBody.Position.DistanceTo(nav.TargetPosition) < .15){
                    nav.TargetPosition = Global.currentLevel.GetRandomPoint();
                }
                GD.Print($"Roaming to {nav.TargetPosition}");
                mot.pos_goal = nav.TargetPosition;
                //physBody.Velocity = (nav.TargetPosition - physBody.GlobalPosition).Normalized() *5;
                break;
            case State.ALERT:
                nav.TargetPosition = target.GlobalPosition; //could put this in a timer
                mot.pos_goal = nav.TargetPosition;
                //physBody.Velocity = (nav.TargetPosition - physBody.GlobalPosition).Normalized() *15;
                break;
            case State.ATTACKING:
                //attack
                break;
        }
        mot.Update(delta);
        //physBody.LookAt(physBody.Velocity.Length() == 0 ? Vector3.Zero : physBody.Velocity, Vector3.Up);
	}

	public void HandleStateChange(StateManager.State state)
	{
		switch (state){
			case State.IDLE:
				nav.TargetPosition = physBody.GlobalPosition;
                physBody.Velocity = Vector3.Zero;
				break;
            case State.ROAMING:
                nav.TargetPosition = new Vector3(1, 1, 3); // TODO fix. Global.currentLevel not yet updated here   //(Vector3)CallDeferred("GetRandomNavPoint", Global.currentLevel);
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
                    ((Damageable)other).ApplyDamage(1);
                    GD.Print("Enemy is attacking you!");
                    Global.HUD.LogEvent("Enemy is attacking you!");
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
                if (other is Player){
                    target = (Node3D) other;
                    stateManager.SetState(State.ALERT);
                }
                break;
        }
	}

    public bool CanChangeState(State state)
    {
        throw new NotImplementedException();
    }

}
