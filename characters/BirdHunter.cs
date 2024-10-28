using Godot;
using System;
using System.Collections.Generic;
using State = NPCModel.State;

public partial class BirdHunter : NPCNode {
	
	public override void _Ready(){
		base._Ready();
		m.inv = ResourceFactory.MakeInventory();
		cycleStates = new List<State>(){ State.ROAMING, State.IDLE };
        if (nav != null){
            NavigationServer3D.MapChanged += (arg) => { 
                nav.TargetPosition = Global.level.GetRandomNavPoint(); //TODO how does birdhunter get his next navigation point?
                navReady = true; 
            };
        }
	}

	public override void _Process(double delta){
		base._Process(delta);
		switch (m.state){
			case State.IDLE:
				break;
			case State.ALERT:
				LookAt(target.GlobalPosition);
				break;
			case State.TALKING:
				break;
			case State.ROAMING:
				if (nav == null) {
					try {
						nav = GetNode<NavigationAgent3D>("NavAgent");
					} catch {
						GD.Print($"NPCNode: No NavigationAgent3D found for {this}");
					}
				} else if (!navReady){
                    nav.TargetPosition = Global.level.GetRandomNavPoint(); //TODO how does birdhunter get his next navigation point?
                    navReady = true; 
                }
				break;
			case State.ATTACKING:
				break;
			case State.SLEEPING:
				break;
			case State.ACTION:
				break;
			case State.DEAD:
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        switch (m.state) {
			case State.ROAMING:
				if (navReady){
					if (!nav.IsNavigationFinished()){
						Vector3 nextPos = nav.GetNextPathPosition();
						nav.Velocity = (nextPos - Position);
						Velocity = nav.Velocity;
					} else {
						nav.TargetPosition = NextNavPoint();
						EffectsManager.MarkerPoint("navpoint", nav.TargetPosition);
					}
				}
				break;
            case State.IDLE:
                Velocity = Vector3.Zero;
                break;
		}
		MoveAndSlide();
	}

	public void OnStateTimeout(){
	}

	public override void HandleCollide(ColliderZone zone, Node other)
	{
        //need to verify that not colliding with self (another node that is attached to this one)
		switch (zone){
			case ColliderZone.Awareness0:
				UpdateState(State.ALERT, other);
				break;
			case ColliderZone.Body:
				Velocity = (((Node3D)other).Position - Position).Normalized() * 4;
				break;
			default:
				break;
		}
	}

	public override void HandleDecollide(ColliderZone zone, Node other)
	{
		switch (zone){
			case ColliderZone.Awareness0:
				UpdateState(State.IDLE);
				break;
			case ColliderZone.Body:
				Velocity = Vector3.Zero;
				break;
			default:
				break;
		}
	}

	//expirimenting a bit here, having this logic handled in the node class rather than the model class. idk which is better at this point
	public override bool UpdateState(State s, dynamic payload = null)
	{
		State prev = m.state;
		switch (s){
			case State.IDLE:
				target = null;
				break;
			case State.ALERT:
				target = payload;
				break;
			case State.TALKING:
				break;
			case State.ROAMING:
                nav.TargetPosition = (Vector3) CallDeferred("NextNavPoint");
				break;
			case State.ATTACKING:
				break;
			case State.SLEEPING:
				break;
			case State.ACTION:
				break;
			case State.DEAD:
				break;
		}
		m.state = s;
		return true;
	}
}
