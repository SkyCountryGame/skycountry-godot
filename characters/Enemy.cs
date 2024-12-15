using Godot;
using System;
using Godot.Collections;
using State = StateManager.State;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

//enemy 0: every 5 seconds, update goal position to player position
public partial class Enemy : NPCNode {

    Godot.Timer timer;

	public override void _Ready(){
		base._Ready();
        timer = new Godot.Timer();
        AddChild(timer);
        timer.Timeout += () => UpdatePathGoal();
        timer.WaitTime = 5;
        timer.OneShot = false;
        timer.Start();
        SetState(defaultState);
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
                if (navReady){
                    mot.pos_goal = nav.GetNextPathPosition();
                }
                break;
            case State.ATTACKING:
                //attack
                break;
        }
        mot.Update(delta, physBody);
        //physBody.LookAt(physBody.Velocity.Length() == 0 ? Vector3.Zero : physBody.Velocity, Vector3.Up);
	}

    private void UpdatePathGoal(){
        if (Global.playerNode != null){
            nav.TargetPosition = Global.playerNode.GlobalPosition;
        }
        
    }

	public override void OnStateChange(State state, float duration = -1)
	{
        base.OnStateChange(state, duration);
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
                //do cooldown
                Task.Run(() => {
                    Thread.Sleep(1000);
                    SetState(State.ALERT);
                });
                break;
			default:
				break;
		}
	}

	public override void HandleCollide(ColliderZone zone, Node other)
	{
		switch (zone) {
            case ColliderZone.Awareness1:
                if (other is Player){
                    SetState(State.ATTACKING);
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
                    SetState(State.IDLE);
                }
                break;
            case ColliderZone.Awareness1:
                if (other is Player){
                    target = (Node3D) other;
                    SetState(State.ALERT);
                }
                break;
        }
	}

    public override bool CanChangeState(State state)
    {
        return true;
    }

}
