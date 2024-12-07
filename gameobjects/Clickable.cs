using Godot;
using System;

//a node that can clicked on.
public partial class Clickable : Node, Interactable {
	public InteractionType interactionType { get => InteractionType.Function; }
	public InteractionMethod interactionMethod { get => InteractionMethod.Click; }

	public override void _Ready(){
		Global.RegisterGameObject(this, GameObjectType.Interactable);
	}

	private void OnInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left) 
		{
		}
	}

	//
	public dynamic Interact()
	{	

		return null;
	}

	public void Retain()
	{
		//wat do?
	}

	public string Info()
	{
	   return "Something to click on.";
	}

	public bool IsInteractionValid(GameObject interactor)
	{
		return true;
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public bool IsInteractionValid(Interactor interactor)
	{
		throw new NotImplementedException();
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
