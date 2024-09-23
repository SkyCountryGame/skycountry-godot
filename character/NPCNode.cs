using System.Collections.Generic;
using Godot;

public partial class NPCNode : CharacterBody3D, Collideable {

	[Export] public NPCModel m;

	private Vector3 goalPosition; //a world position where the npc is currently trying to go 
	private AnimationTree animTree; //npcs gotta have animations

	[Export] private NavigationAgent3D nav;
	private Stack<Vector3> navPoints = new Stack<Vector3>(); //some places where this NPC can go
	private bool navReady = false;
	public override void _Ready(){
        if (m == null){
            m = new NPCModel("Bob", "A friendly NPC"); //TODO placeholder 
		    m.state = NPCModel.State.IDLE;
        }
		Velocity = new Vector3(1, 0, -2);

		if (nav == null) {
			try {
				nav = GetNode<NavigationAgent3D>("NavAgent");
				NavigationServer3D.MapChanged += (arg) => { 
					nav.TargetPosition = Global.level.GetRandomNavPoint();
					navReady = true; 
				};
			} catch {
				GD.Print($"NPCNode: No NavigationAgent3D found for {this}");
			}
		}
	}

	public override void _Process(double delta){
		switch (m.state){ //what is the npc doing in each different case of his state of being? 
			case NPCModel.State.IDLE: //

				break;
			case NPCModel.State.TALKING:
				break;
			case NPCModel.State.ROAMING:
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

		LookAt(nav.TargetPosition);
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

	public Vector3 NextNavPoint(){
		return Global.level.GetRandomNavPoint(); //navPoints.Pop();
	}

	public void HandleCollide(ColliderZone zone, Node other)
	{
		throw new System.NotImplementedException();
	}

	public void HandleDecollide(ColliderZone zone, Node other)
	{
		throw new System.NotImplementedException();
	}
}
