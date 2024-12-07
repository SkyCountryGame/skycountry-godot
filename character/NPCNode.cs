using System;
using System.Collections.Generic;
using System.Timers;
using Godot;
using State = StateManager.State;

//functionality common to all NPCs. specific NPCs will extend this abstract class
public abstract partial class NPCNode : Node3D, Collideable, StateHolder {

	[Export] public NPCModel m;

	protected Vector3 goalPosition; //a world position where the npc is currently trying to go 
	protected AnimationTree animTree; //npcs gotta have animations

	[Export] protected NavigationAgent3D nav;
	protected Stack<Vector3> navPoints = new Stack<Vector3>(); //some places where this NPC can go
	protected bool navReady = false;
	private Vector3 navTarget; //where the thing is trying to go currently

	protected Node3D target; //a node of interest to the npc. 
	protected LinkedList<State> cycleStates; //states that will be automatically cycled through via timer
	protected LinkedListNode<State> cycleStateCurrent; //the current state in the cycle
	protected int cycleStateIdx = 0; //curent index of cycle state

	[Export] protected CharacterBody3D physBody; //used for handling motion
	[Export] protected StateManager stateManager;
	[Export] protected AnimationController animController;
	[Export] protected MotionModule mot;

	public override void _Ready(){
		if (m == null){
			m = new NPCModel(); //TODO placeholder 
		}
		
		//setup some things that inheriters may or may not have
		if (physBody == null && HasNode("CharacterBody3D")) {
			physBody = GetNode<CharacterBody3D>("CharacterBody3D");
		}
		if (stateManager == null && HasNode("StateManager")){
			stateManager = GetNode<StateManager>("StateManager");
		}
		if (nav == null && HasNode("NavigationAgent3D")){
			nav = GetNode<NavigationAgent3D>("NavigationAgent3D");
		}
		if (nav != null){
			NavigationServer3D.MapChanged += (arg) => { 
				navReady = true;
			};
		}
		if (mot == null && physBody != null){
			mot = new MotionModule(physBody);
		}
	}

	public override void _Process(double delta){
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public abstract void HandleCollide(ColliderZone zone, Node other);

	public abstract void HandleDecollide(ColliderZone zone, Node other);

    public virtual void HandleStateChange(State state)
    {
        if (animController != null){
			animController.HandleStateChange(state);
		}
		if (mot != null){
			mot.HandleStateChange(state);
		}
    }

    public abstract bool CanChangeState(State state);

}
