using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<int, StatementNode> statements; //map statement id to statement
    public StatementNode currentStatement;
    public struct StatementNode {
        public string statement; //NOTE should be "statementText"?
        public List<ResponseNode> responses;
        public StatementNode(string statement){
            this.statement = statement;
            responses = new List<ResponseNode>();
        }
    }
    public struct ResponseNode {
        public string response; //NOTE should be "responseText"?
        public int nextStatementID;
        public ResponseNode(string response, int nextStatementID){
            this.response = response;
            this.nextStatementID = nextStatementID;
        }
    }
    //---

    public Dialogue(string filepath = null)
    {
        if (filepath != null){ //might already be set from editor
            this.filepath = filepath;
        }
        statements = new Dictionary<int, StatementNode>();
        string dialogueStr = FileAccess.GetFileAsString(filepath);
        if (ParseDialogue(dialogueStr)){
            currentStatement = statements.First().Value; //grab the first entry, because we don't know the id of the first statement
        } else {
            GD.Print("Failed to parse dialogue file. TODO what do?");
        }
    }

    //see doc for formatting
    private bool ParseDialogue(string dstr){ //TODO add error handling
        int pid = -1; //parent dialoguenode id. "parent" of any player responses in subsequent lines
        foreach (string l in dstr.Split('\n')){
            if (l.StartsWith("###")){ //end
                break;
            }
            if (l.StartsWith("#") || l.Trim() == ""){ //comment or blank
                continue;
            }
            string[] parts = l.Split(':', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            if (int.TryParse(parts[0], out int id)){ //this is said by npc
                pid = id; //set parent id for the subsequent player responses
                //there can by multiple statements that can be selected at random. get each string that's between quotes
                //TODO read through and get in between quotes
                if (parts.Length > 2){ //there happen to be colons in the text of the statement
                    string statementsStr = string.Join(":", parts[1..^1]);
                    //TODO handle multiple statements. separated by commas https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0
                    statements.Add(id, new StatementNode(statementsStr));
                } else {
                    statements.Add(id, new StatementNode(parts[1]));
                }
            } else if (int.TryParse(parts[^1], out id)){ //this is a player response, and the id points to the NPC statement that would follow after this response
                //TODO read through and get in between quotes
                if (parts.Length > 2){ //there happen to be colons in the text of the response
                    string responseStr = string.Join(":", parts[0..^2]);
                    //TODO handle multiple responses. separated by commas
                    statements[pid].responses.Add(new ResponseNode(responseStr, id));
                } else {
                    statements[pid].responses.Add(new ResponseNode(parts[0], id));
                }
            } else {
                return false;
            }
        }
        return true;
    }

    //return next thing to say when player says nothing
    public string Next()
    { 
        return currentStatement.statement;
    }

    //return next thing to say based on what player says
    public string Next(int i)
    {
        if (i >= currentStatement.responses.Count){
            return ""; //this shouldn't have been called with incorrect index anyway
        }
        currentStatement = statements[currentStatement.responses[i].nextStatementID];
        return currentStatement.statement;
    }

    public List<string> GetResponsesAsString(){
        List<string> responses = new List<string>();
        foreach (ResponseNode r in currentStatement.responses){
            responses.Add(r.response);
        }
        return responses;
    }
    public List<ResponseNode> GetResponses(){
        return currentStatement.responses;
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

