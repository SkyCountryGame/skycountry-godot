using System;
using Godot;

/**
	this is an explicit implementation of an interactable lamppost
*/
public partial class LampPost : Node, Interactable {


	public InteractionMethod interactionMethod => InteractionMethod.Use;

  	//[Export(PropertyHint.Enum, "interactionType")]
	public InteractionType interactionType => InteractionType.General;
	[Export] private OmniLight3D lamplight;
	private int n = 0;

	public override void _Ready()
	{
		Global.RegisterGameObject(this, GameObjectType.Interactable);
		//lamplight = GetNode<OmniLight3D>("CollisionShape3D/StaticBody3D/OmniLight3D");
		
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public string Info()
	{
		return "Does one seek light?";
	}

	public dynamic Interact()
	{
		if (n > 4){ //it's a magic lamp
			Global.ChangeLevel("res://levels/level2.tscn");
		}
		//toggle lamp light
		lamplight.LightEnergy = lamplight.LightEnergy == 0 ? 1 : 0;
		n += 1;
		return true;
	}

	public bool IsInteractionValid(Interactor interactor)
	{
		throw new NotImplementedException();
	}

	public void Retain()
	{
		throw new NotImplementedException();
	}

}
