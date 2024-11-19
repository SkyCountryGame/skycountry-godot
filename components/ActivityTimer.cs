using Godot;
using System;
using Godot.Collections;

/*
* the purpose of this component is to automatically cycle through some states of a statemanager 
*/

[GlobalClass]
public partial class ActivityTimer : Timer {
	private Array<int> activities;
	private int cur = 0; //current activity
	[Export] private StateManager stateManager;

	public override void _Ready(){
		base._Ready();
		activities = new Array<int>(new int[stateManager.states.Count]);
		Timeout += ()=>{
			cur++;
			if (cur > activities.Count-1){
				cur = 0;
			}
			stateManager.SetStateByIndex(cur);
		};
	}
}
