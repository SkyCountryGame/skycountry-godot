using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Schema;
/*
 * represents a conversation between the player and another character. keeps track of progression through the dialogue. 
    - typewriting text, hold button to skip, 
	- talker has emotional-expression associated with point in sentence
	- choices after each statement to start actions (like barter UI)
  - how it works:
    - parses a file that contains the spec for a tree of dialogue
    - turns that into a dialogue which is then used by the game
 **/
public partial class Dialogue : Resource
{
    //TODO add functionaliy for removing/adding/altering responses etc. after something already said or other event

    [Export]
    private string filepath {get; set;} //the file that contains the dialogue tree
    private Dictionary<int, StatementNode> statements; //map statement id to statement
    private struct StatementNode {
        public string statement;
        public List<ResponseNode> responses;
        public StatementNode(string statement){
            this.statement = statement;
            responses = new List<ResponseNode>();
        }
    }
    private struct ResponseNode {
        public string response;
        public int nextStatementID;
        public ResponseNode(string response, int nextStatementID){
            this.response = response;
            this.nextStatementID = nextStatementID;
        }
    }
    public Dialogue()
    {
        statements = new Dictionary<int, StatementNode>();
        string dialogueStr = FileAccess.GetFileAsString(filepath);
        ParseDialogue(dialogueStr);
    }

    public Dialogue(string start)
    {
        DialogueNode dn = new DialogueNode(1, start);
        dn.responses.Add(new DialogueNode(0, "hello"));
    }

    //see doc for formatting
    private void ParseDialogue(string dstr){ //TODO add error handling
        int pid = -1; //parent dialoguenode id
        foreach (string l in dstr.Split('\n')){
            string[] parts = l.Split(':', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            if (int.TryParse(parts[0], out int id)){ //this is said by npc
                //there can by multiple statements that can be selected at random. get each string that's between quotes
                //TODO read through and get in between quotes
                if (parts.Length > 2){ //there are colons in the statement
                    string statementsStr = string.Join(":", parts[1..^0]);
                    //TODO handle multiple statements. separated by commas
                    statements.Add(id, new StatementNode(statementsStr));
                } else {
                    statements.Add(id, new StatementNode(parts[1]));
                }
            } else if (int.TryParse(parts[^0], out id)){ //this is a player response
                //TODO read through and get in between quotes
                if (parts.Length > 2){
                    string responseStr = string.Join(":", parts[0..^1]);
                    //TODO handle multiple responses. separated by commas
                    statements[pid].responses.Add(new ResponseNode(responseStr, id));
                } else {
                    statements[pid].responses.Add(new ResponseNode(parts[0], id));
                }
            }
        }
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
    //TODO remove DialogueNode. don't think need it
    partial class DialogueNode : Resource {
        [Export]
        public int who; //0 = player, 1 = npc, 2 = other npc, etc. NOTE this might end up being like an NPC ID
        [Export]
        public string text; //what "who" says
        public int textSpeed;
        //public emotion
        //event
        public List<DialogueNode> responses;
        public List<DialogueNode> next; //most cases will only lead to one, but we might want to randomly pick one

        public DialogueNode(){}
        public DialogueNode(int who, string text){
            this.who = who;
            this.text = text;
            responses = new List<DialogueNode>();
            next = new List<DialogueNode>();
            textSpeed = 1;
        }
    }
}

