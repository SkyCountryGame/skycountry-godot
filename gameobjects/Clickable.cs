using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

//a node that can clicked on. TODO this is custom for a lockon thing currently. 
public partial class Clickable : RigidBody3D, Interactable, Collideable {
	public InteractionType interactionType { get => InteractionType.Function; }
	public InteractionMethod interactionMethod { get => InteractionMethod.ClickRight; }

	private Task movementTask;
	private Random rand = new Random();

	public override void _Ready(){
		Global.RegisterGameObject(this, GameObjectType.Interactable);
		InputEvent += OnInputEvent;
		
		Task.Run(()=>{ 
			while (true){
				Thread.Sleep(800);
				rand.NextDouble();
				CallDeferred("set_linear_velocity", new Vector3((float)rand.NextDouble()*10f-5,(float)rand.NextDouble()*.3f,(float)rand.NextDouble()*6f-3)); 
			}
		});

	}

	public void Move(){
		Position = new Vector3( Position.X + .5f, 0, 0);
	}

	private void OnInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right)
		{
			if (mouseEvent.Pressed){
				Global.cam.LockOn(this);
			}
		}
	}

	//start dialogue when player interacts
	public dynamic Interact()
	{
		Global.cam.LockOn(this);
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
