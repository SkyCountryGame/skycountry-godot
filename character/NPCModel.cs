using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class NPCModel : Resource{
	[Export] public string name;
	[Export] public string description; //what is known of this one? maybe should be stored in player's journal/knowledge
	
	//inventory, quests, tribe, ? 
	// 
	[Export] public Inventory inv;

	[Export] public int disposition = 0; //level of hostility/friendliness. more negative is more hostile, positive friendly, 0 neutral
	
	public enum State { 
        IDLE, //standing still
        ALERT, //something nearby happened, like somebody entered awareness zone, maybe watching or pursuing
        TALKING, 
        ROAMING, //same as idle except moving around 
        ATTACKING, //attacking target
        SLEEPING, 
        ACTION, //performing an action which may also be triggering an event
        DEAD 
    } //cycles through these states, changes based on environment and TBD functions
    
	public enum Emotion { HAPPY, SAD, ANGRY, SCARED, SURPRISED, DISGUSTED, NEUTRAL }
	public enum HomeostaticPressure {NONE, HUNGER, SOCIAL, RESTORE, REDPRODUCE, SLEEP }; //dictates object of pursual

	[Export] public State state;
	[Export] public Emotion emotion;
	//[Export] public List<Quest> quests; //
	[Export] public State defaultState = State.IDLE; //this is what the npc will begin its exist with
	//public Dictionary<TimeRange, State> dailyRoutine; //NOT should probably actually be "actions" or something
    
    [Export(PropertyHint.None,"average duration (s) to remain in each state unless interrupted")] 
    public int stateTransitionInterval;

	//
	public Dictionary<State, Animation> mapStateAnimation; //which animation for which state 

    //should never have to call default constructor because always loaded from '.tres' file
    public NPCModel(){
        name = "Bob";
        description = "A friendly guy";
        inv = new Inventory();
        state = defaultState;
        stateTransitionInterval = 10;
    }

	public NPCModel(string resourcePath){
		
	}

	public bool UpdateState(State s){
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
		state = s; //TODO
        return true;
	}

	public void GetCurrentActivity(double time){
		//use the time duration activity map to get the jcurrent activity.
		//then we can set antimation etc. accordingly 
	}
}
