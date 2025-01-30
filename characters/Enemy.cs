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

	private Godot.Timer timer;
	private Godot.Timer timerCooldown;
	private Damageable attackTarget;

	public override void _Ready(){
		base._Ready();
		timer = new Godot.Timer();
		AddChild(timer);
		timer.Timeout += () => UpdatePathGoal();
		timer.WaitTime = 5;
		timer.OneShot = false;
		timer.Start();
		timerCooldown = new Godot.Timer();
		AddChild(timerCooldown);
		timerCooldown.OneShot = true;
		timerCooldown.WaitTime = 1;
		timerCooldown.Timeout += () => {
			SetState(State.ALERT);
		};
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
				//if (navReady){ TODO remove if end up not using godot navigation
					float y = mot.pos_goal.Y;
					mot.pos_goal = nav.TargetPosition; //temp testing without nav system. GetNextPathPosition();
					if (mot.pos_goal.Y - y > 1) { mot.pos_goal.Y = y; } //some vertical threshold beyond which the enemy cant jump
                    //NOTE experiment with Vector3.MoveTowards() or Lerp()
				//}
				break;
			case State.ATTACKING:
				physBody.RotateY(MathF.PI/16.0f);
				//attack
				break;
		}
		mot.Update(delta, physBody);
		//physBody.LookAt(physBody.Velocity.Length() == 0 ? Vector3.Zero : physBody.Velocity, Vector3.Up);
	}

	//currently enemy only ever has player as a nav goal. this will probably change so will pass in a position 
	private void UpdatePathGoal(){
		if (Global.playerNode != null){
			nav.TargetPosition = Global.playerNode.GlobalPosition;
		}
	}

	public override void OnStateChange(State state, float duration = -1)
	{
		base.OnStateChange(state, duration);
		switch (state){
			case State.ALERT: //default state. go to player
				if (attackTarget != null){
					SetState(State.ATTACKING);
					return;
				}
				break;
			case State.ATTACKING: //player is close enough. attack
				attackTarget.ApplyDamage(1);
				GD.Print("Enemy is attacking you!");
				Global.HUD.LogEvent("Enemy is attacking you!");
				//do cooldown
				timerCooldown.Start();
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
					attackTarget = (Damageable) other;
					SetState(State.ATTACKING);
				}
				break;
			case ColliderZone.Awareness0:
				if (other is Player){
					timer.WaitTime = .05;
				}
				break;
		}
	}

	public override void HandleDecollide(ColliderZone zone, Node other)
	{
		switch (zone) {
			case ColliderZone.Awareness1:
				if (other is Damageable && other == attackTarget){
					attackTarget = null;
					SetState(State.ALERT);
				}
				break;
			case ColliderZone.Awareness0:
				if (other is Player){
					timer.WaitTime = 5;
				}
				break;
		}
	}

	public override bool CanChangeState(State state)
	{
		return true;
	}

}
