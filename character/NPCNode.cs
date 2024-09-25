using System;
using System.Collections.Generic;
using System.Timers;
using Godot;
using State = NPCModel.State;

//functionality common to all NPCs. specific NPCs will extend this abstract class
public abstract partial class NPCNode : CharacterBody3D, Collideable {

	[Export] public NPCModel m;

	protected Vector3 goalPosition; //a world position where the npc is currently trying to go 
	protected AnimationTree animTree; //npcs gotta have animations

	[Export] protected NavigationAgent3D nav;
	protected Stack<Vector3> navPoints = new Stack<Vector3>(); //some places where this NPC can go
	protected bool navReady = false;

	protected Node3D target; //a node of interest to the npc. 

	//timers: periodic timer could be useful, https://learn.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-8.0 
	protected System.Timers.Timer stateTimer; //periodic timer used to switch activities. 
	//but Godot.Timer is countdown which works better here for remaining in a state for some time, then doing something, then resetting the timer with a new duration
	//protected SceneTreeTimer stateTimer; //used to count down to state change (unless interupted by event)
	protected List<State> cycleStates; //states that will be automatically cycled through via timer
	protected int cycleStateIdx = 0; //curent index of cycle state
	protected System.Timers.Timer checkTimer; //timer for checking position to see if got stuck

	public override void _Ready(){
		if (m == null){
			m = new NPCModel(); //TODO placeholder 
		}
		stateTimer = new System.Timers.Timer((m.stateTransitionInterval + GD.RandRange(-1, 1))*1000);//GetTree().CreateTimer(m.stateTransitionInterval);
		stateTimer.AutoReset = true; //Timeout += OnStateTimeout;
		stateTimer.Elapsed += OnStateTimeout;
		stateTimer.Start();
	}

	public override void _Process(double delta){
		switch (m.state){ //what is the npc doing in each different case of his state of being? 
			case NPCModel.State.IDLE: //
				break;
			case NPCModel.State.TALKING:
				break;
			case NPCModel.State.ALERT:
				LookAt(target.Position);
				break;
			case NPCModel.State.ROAMING:
				if (nav != null){
					LookAt(nav.TargetPosition);
				}
				break;
			case NPCModel.State.ATTACKING:
				break;
			case NPCModel.State.SLEEPING:
				break;
			case NPCModel.State.ACTION:
				break;
			case NPCModel.State.DEAD:
				break;
		}

		
	}

	/*
		npc pathfinding:
		- get a navpoint based on some parameters and the current desire of the npc
		- what conditions cause travel to be interrupted? and then how is the dest navpoint updated?
		- how to detect when the npc gets stuck? 
		- how to prevent getting stuck?
		- how to be able to go around obstacles to reach destination?
		- when to decide to stop and remain idle?
	*/

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		switch (m.state) {
			case NPCModel.State.IDLE:
				if (navReady){
					if (!nav.IsNavigationFinished()){
						nav.Velocity = (nav.GetNextPathPosition() - Position) * .5f;
						Velocity = nav.Velocity;
						MoveAndSlide();
					} else {
						nav.TargetPosition = NextNavPoint();
						EffectsManager.MarkerPoint("navpoint", nav.TargetPosition);
					}
				}
				break;
		}
	}

	//has default functionality but obviously can be overriden
	public Vector3 NextNavPoint(){
		return Global.level.GetRandomNavPoint(); //navPoints.Pop();
	}

	public abstract void HandleCollide(ColliderZone zone, Node other);

	public abstract void HandleDecollide(ColliderZone zone, Node other);

	public abstract bool UpdateState(State s, dynamic payload = null); 
	/*{  //abstract for now but might end up having general funcationality
		if (m.UpdateState(s)){
			//animation change and anything else that needs to be handled in node
			return true;
		} else {
			return false;
		}
	}*/

	//called by the state transition timer. attempt to change state according to some logic. reset the timer 
	private void OnStateTimeout(object sender, ElapsedEventArgs e){
		cycleStateIdx += 1;
		if (cycleStateIdx >= cycleStates.Count){
			cycleStateIdx = 0;
		}
		GD.Print("state timer");
		UpdateState(cycleStates[cycleStateIdx]);
		stateTimer.Interval = ((m.stateTransitionInterval + GD.RandRange(-3, 3))*1000);
		
	}
}
