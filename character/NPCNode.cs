using Godot;

public partial class NPCNode : CharacterBody3D {

	[Export] public NPCModel m;

	private Vector3 goalPosition; //a world position where the npc is currently trying to go 
	private NavigationAgent3D nav;
	private bool navReady = false;
	public override void _Ready(){
		m = new NPCModel("Bob", "A friendly NPC");
		m.state = NPCModel.State.IDLE;
		Velocity = new Vector3(1, 0, -2);
		nav = GetNode<NavigationAgent3D>("NavAgent");
		

		NavigationServer3D.MapChanged += (arg) => { 
			nav.TargetPosition = Global.level.GetRandomNavPoint();
			navReady = true; 
		};
	}

	public override void _Process(double delta){
		switch (m.state){ //what is the npc doing in each different case of his state of being? 
			case NPCModel.State.IDLE:
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
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		switch (m.state) {
			case NPCModel.State.IDLE:
				//if at goal position, stay here for a bit, then update the position
				//else keep moving towards the goal position
				//make sure velocity is such that going to goal, accounting for obstacles etc. 
				if (navReady){
					if (!nav.IsNavigationFinished()){
						nav.Velocity = (nav.GetNextPathPosition() - Position) * .5f;
						Velocity = nav.Velocity;
						MoveAndSlide();
					} else {
						nav.TargetPosition = Global.level.GetRandomNavPoint();
					}
				}
				break;
		}
	}
}
