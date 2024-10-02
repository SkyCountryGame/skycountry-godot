using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
		//TODO how to have player "interact" when click on this thing
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

    public int GetHashCode()
    {
        throw new NotImplementedException();
    }

}
