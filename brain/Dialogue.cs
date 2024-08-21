using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
public partial class Dialogue
{
    //TODO add functionaliy for removing/adding/altering responses etc. after something already said or other event

    [Export]
    private string filepath {get; set;} //the file that contains the dialogue tree
    public Dictionary<int, StatementNode> statements; //map statement id to statement
    public StatementNode currentStatement;

    public struct StatementNode { //NOTE dont know if should also store id
        public string statement; //NOTE should be "statementText"?
        public List<ResponseNode> responses;
        public int nextStatementID = -1; //when there's no responses but a continuation to another statement. -1 means to exit dialogue
        public EventType eventType; //when the next thing that happens is some in game action
        public StatementNode(string statement){
            this.statement = statement;
            responses = new List<ResponseNode>();
            eventType = EventType.None;
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

    //parse file contents as json and populate this object. local variables here are named with 'j' suffix if a json thing. 'd' obviously stands for dialogue. 
    //return false if any of the json values do not follow the dialogue format spec
    private bool ParseDialogue(string dstr){
        JsonDocument dj = JsonDocument.Parse(dstr); //dialogue as json
        if (dj.RootElement.ValueKind != JsonValueKind.Array){
            return false;
        }
        foreach (JsonElement sj in dj.RootElement.EnumerateArray()){ //iterate through each statement
            if (sj.TryGetProperty("text", out JsonElement tj)){ //statement text
                StatementNode sn;
                if (tj.ValueKind == JsonValueKind.String){
                    sn = new StatementNode(tj.GetString());
                } else if (tj.ValueKind == JsonValueKind.Array){
                    //TODO handle multiple statements in some way
                    string tmp = ""; //temporary
                    foreach (JsonElement stj in tj.EnumerateArray()){
                            tmp += stj.GetString();
                    }
                    sn = new StatementNode(tmp);
                } else { return false; }
                
                if (sj.TryGetProperty("id", out JsonElement ij) && ij.ValueKind == JsonValueKind.Number){ //id of statement
                    if (sj.TryGetProperty("event", out JsonElement ej)) { //this statement triggers an in game event
                        if (ej.ValueKind == JsonValueKind.String){ //for now action is an int, to be mapped to an enum probably. might end up being a string
                            sn.eventType = Enum.Parse<EventType>(ej.GetString()); //not worrying about payload for dialogue-triggered events
                        } else { return false; } //event json not string
                    }
                    if ( sj.TryGetProperty("responses", out JsonElement rj)){ //responses
                        if (rj.ValueKind == JsonValueKind.Array){
                            foreach (JsonElement r in rj.EnumerateArray()){
                                if (r.TryGetProperty("text", out JsonElement rtj) && r.TryGetProperty("next", out JsonElement nidj) && nidj.ValueKind == JsonValueKind.Number){
                                    sn.responses.Add(new ResponseNode(rtj.GetString(), nidj.GetInt32()));
                                } else { return false; }
                            }
                        } else { return false; }
                    } else if (sj.TryGetProperty("next", out JsonElement nj) && nj.ValueKind == JsonValueKind.Number){ //goes to another statement before player can respond
                        sn.nextStatementID = nj.GetInt32();
                    }
                    statements[ij.GetInt32()] = sn;
                     
                } else { return false; }
            } else { return false; }
        }
        return true;
    }

    //increments the dialogue to the next statement and return that statement, unless the next statement is -1 (end of dialogue), and this function is only intended to be called when we think there's a next statement
    public StatementNode Next()
    {
        if (currentStatement.nextStatementID != -1){
            currentStatement = statements[currentStatement.nextStatementID];
        }
        return currentStatement;
    }

    //increments the dialogue to the next statement given the player response choice (index of the response) and returns that StatementNode
    public StatementNode Next(int idx)
    {
        if (idx >= currentStatement.responses.Count){
            return currentStatement; //this shouldn't have been called with incorrect index
        }
        currentStatement = statements[currentStatement.responses[idx].nextStatementID];
        return currentStatement;
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

