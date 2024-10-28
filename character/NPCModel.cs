using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class NPCModel : Resource{
	[Export] public string name;
	[Export] public string description; //what is known of this one? maybe should be stored in player's journal/knowledge
	
	//inventory, quests, tribe, ? 
	// 
	[Export] Inventory inv;

	[Export] public int disposition = 0; //level of hostility/friendliness. more negative is more hostile, positive friendly, 0 neutral
	
	public enum State { IDLE, TALKING, ROAMING, ATTACKING, SLEEPING, ACTION, DEAD } //cycles through these states, changes based on environment and TBD functions
	public enum Emotion { HAPPY, SAD, ANGRY, SCARED, SURPRISED, DISGUSTED, NEUTRAL }
	public enum HomeostaticPressure {NONE, HUNGER, SOCIAL, RESTORE, REDPRODUCE, SLEEP }; //dictates object of pursual

	[Export] public State state;
	[Export] public Emotion emotion;
	//[Export] public List<Quest> quests; //
	public State defaultState; //this is what the npc will begin its exist with
	//public Dictionary<TimeRange, State> dailyRoutine; //NOT should probably actually be "actions" or something

	public Dictionary<State, Animation> mapStateAnimation; //which animation for which state 

    public NPCModel(){

    }

	public NPCModel(string name, string description){
		this.name = name;
		this.description = description;
		//quests = new List<Quest>();
	}

	public void UpdateState(State s){
		switch (s){
			case State.IDLE:
				break;
			case State.TALKING:
				break;
			case State.ROAMING:
				break;
			case State.ATTACKING:
				break;
			case State.SLEEPING:
				break;
			case State.ACTION:
				break;
			case State.DEAD:
				break;
		}
	}

	public void GetCurrentActivity(double time){
		//use the time duration activity map to get the jcurrent activity.
		//then we can set antimation etc. accordingly 
	}
}
