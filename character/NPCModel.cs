using System;
using System.Collections.Generic;
using Godot;
using State = StateManager.State;

[GlobalClass]
public partial class NPCModel : Resource{
	[Export] public string name;
	[Export] public string description; //what is known of this one? maybe should be stored in player's journal/knowledge
	
	//inventory, quests, tribe, ? 
	// 
	[Export] public Inventory inv;

	[Export] public int disposition = 0; //level of hostility/friendliness. more negative is more hostile, positive friendly, 0 neutral
	
    [Export] public Dialogue dialogue; //NPCs can have dialogue, often many, each associated with a level or other conditions
    
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

	public void GetCurrentActivity(double time){
		//use the time duration activity map to get the jcurrent activity.
		//then we can set antimation etc. accordingly 
	}
}
