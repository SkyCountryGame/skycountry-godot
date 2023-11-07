using Godot;
using System;
using System.Collections.Generic;
using SkyCountry;

public partial class InputManager : Node3D
{
	private Dictionary<InputEventAction, InputEvent> actionKeyMap;

	private List<InputActionListener> actionListeners;

	private Dictionary<string, List<InputActionListener>> actionListenerMap; //which actionlisteners want to know about what actions
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(InputMap.GetActions());
		foreach (StringName a in InputMap.GetActions())
		{
			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _Input(InputEvent @event){
		GD.Print(@event.AsText());
	}

	/**
	 * external class calls this and registers for input actions, passing a list of actions it cares about
	 */
	public void addActionListener(InputActionListener l, List<string> actionsOfConcern)
	{
		actionListeners.Add(l);
		foreach (string a in actionsOfConcern)
		{
			if (!actionListenerMap.ContainsKey(a))
			{
				actionListenerMap[a] = new List<InputActionListener>();
			}
			actionListenerMap[a].Add(l);
		}
		
	}
}
