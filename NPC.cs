using System;
using System.Collections.Generic;
using Godot;

public class NPC {
    public string name;
    public string description; //what is known of this one? maybe should be stored in player's journal/knowledge
    
    //inventory, quests, tribe, ? 
    // 
    public int disposition = 0; //level of hostility/friendliness. more negative is more hostile, positive friendly, 0 neutral
    
    public enum State { IDLE, TALKING, ROAMING, ATTACKING, SLEEPING, ACTION, DEAD } //cycles through these states, changes based on environment and TBD functions
    public enum Emotion { HAPPY, SAD, ANGRY, SCARED, SURPRISED, DISGUSTED, NEUTRAL }
    public Tuple<State, Emotion> state;
    public Dictionary<State, Animation> mapEmotionAnimation; //which animation for which emotion 

    public NPC(string name, string description){
        this.name = name;
        this.description = description;
    }
}