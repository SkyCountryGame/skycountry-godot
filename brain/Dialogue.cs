using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
/*
 * represents a conversation between the player and another character. keeps track of progression through the dialogue. 
    - typewriting text, hold button to skip, 
	- talker has emotional-expression associated with point in sentence
	- choices after each statement to start actions (like barter UI)
 **/
public partial class Dialogue : Resource
{
    //dialogue is a tree of nodes containing all the relevant information for a conversation
    public struct DialogueNode {
        public int who; //0 = player, 1 = npc, 2 = other npc, etc. NOTE this might end up being like an NPC ID
        public string text; //what "who" says
        public int textSpeed;
        //public emotion
        //event
        public List<DialogueNode> responses;
        public List<DialogueNode> next; //most cases will only lead to one, but we might want to randomly pick one
        public DialogueNode(int who, string text){
            this.who = who;
            this.text = text;
            responses = new List<DialogueNode>();
            next = new List<DialogueNode>();
            textSpeed = 1;
        }
    }

    public DialogueNode start; // the beginning of the dialogue
    public DialogueNode cur; // the current point in the dialogue

    [JsonInclude]
    public List<string> statements { get; set; }
    // List<StatementNode> statements
    // StatementNode contains: string statement, List<ChoiceNode> choices
    // ChoiceNode contains: List<string> responseChoices, List<StatementNode> nextStatements


    [JsonInclude]
    public string testStr { get; set; }

    public Dialogue()
    {
        //TODO construct from the spec file
    }

    public Dialogue(string start)
    {
        DialogueNode dn = new DialogueNode(1, start);
        dn.responses.Add(new DialogueNode(0, "hello"));


    }

    //return next thing to say when player says nothing
    public string Next()
    { 
        return null;
    }

    //return next thing to say based on what player says
    public string Next(int i)
    {
        return null;
    }

}
