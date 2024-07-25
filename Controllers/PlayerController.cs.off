using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerController : ObjectController, Interactor
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private List<Interactable> availableInteractables = new List<Interactable>();
	
	private NavigationAgent3D navAgent;

	public Vector3 navTargetPos = new Vector3(3, 0, 1); //where go

	public Vector3 navTarget
	{
		get { return navAgent.TargetPosition;  }
		set { navAgent.TargetPosition = value; }
	}

	public PlayerController(Player p): base(p)
	{
	
	}

    public override void update(float dt)
    {
    }


    public void HandleInteract(Node interactionObj, dynamic payload)
	{

		throw new NotImplementedException();
	}

	public Interactable GetFirstInteractable()
	{
		return null;
	}
}
