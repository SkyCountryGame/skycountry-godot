using Godot;
using System;
using Godot.Collections;
using State = NPCModel.State;

//bird sits at his nest, flies to another tree, chills for a bit, flies back, and repeats
public partial class Bird : Node3D, Collideable {

    [Export] private Array<Node3D> stations = new Array<Node3D>(); //places of interest to the bird
    private ActivityTimer activityTimer;
    [Export] private StateManager stateManager;

	public override void _Ready(){
		base._Ready();
        //activityTimer = GetNode<ActivityTimer>("ActivityTimer");
        //activityTimer.Start();
        if (stateManager == null){
            stateManager = GetNode<StateManager>("StateManager");
        }
    }

	public override void _Process(double delta){
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public void OnStateChange(){
        GD.Print("next station ");
    }

	public Vector3 NextNavPoint(){
		float th = (float)GD.RandRange(0, 2*Mathf.Pi);
		float d = (float)GD.RandRange(0, 10);
		return new Vector3(d*Mathf.Cos(th), 0, d*Mathf.Sin(th)) + GlobalPosition;
	}

    public void HandleCollide(ColliderZone zone, Node other)
    {
        throw new NotImplementedException();
    }

    public void HandleDecollide(ColliderZone zone, Node other)
    {
        throw new NotImplementedException();
    }
}
