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
public partial class Dialogue : Resource
{
    //TODO add functionaliy for removing/adding/altering responses etc. after something already said or other event

    [Export]
    private string filepath {get; set;} //the file that contains the dialogue tree
    public Dictionary<int, StatementNode> statements; //map statement id to statement
    public StatementNode currentStatement;
    public string title = "";

    public struct StatementNode { //NOTE dont know if should also store id
        public string statement; //NOTE should be "statementText"?
        public List<ResponseNode> responses;
        public int nextStatementID = -1; //when there's no responses but a continuation to another statement. -1 means to exit dialogue
        public EventType eventType; //when the next thing that happens is some in game action
        public string startFunction;
        public StatementNode(string statement){
            this.statement = statement;
            responses = new List<ResponseNode>();
            eventType = EventType.None;
            startFunction = null;
        }
    }
    public struct ResponseNode {
        public string response; //NOTE should be "responseText"?
        public int failureNextStatementID;
        public List<Object> args;
        public string methodName;

        public int nextStatementID;
        public ResponseNode(string response, int nextStatementID){
            this.response = response;
            this.nextStatementID = nextStatementID;
            failureNextStatementID = nextStatementID; //figure no matter what we want to proceed to next
            args = null;
            methodName = null;
        }
        public ResponseNode(string response, string methodName, int nextStatementID, int failureNextStatementID, List<Object> args){
            this.response = response;
            this.nextStatementID = nextStatementID;
            this.failureNextStatementID = failureNextStatementID;
            this.args = args;
            this.methodName = methodName;
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
        JsonDocument jsonDocument = JsonDocument.Parse(dstr); //dialogue as json
        if (jsonDocument.RootElement.ValueKind != JsonValueKind.Array){
            return false;
        }
        foreach (JsonElement jsonElement in jsonDocument.RootElement.EnumerateArray()){ //iterate through each statement. sj = statementjson
            if (jsonElement.TryGetProperty("text", out JsonElement textJsonElement)){ //statement text. tj = text json
                StatementNode statementNode;
                if (textJsonElement.ValueKind == JsonValueKind.String){ 
                    statementNode = new StatementNode(textJsonElement.GetString());
                } else { return false; } //NOTE possible future: use array of strings and construct a statement for each one, generating the appropriate ids
                
                if (jsonElement.TryGetProperty("id", out JsonElement idJsonElement) && idJsonElement.ValueKind == JsonValueKind.Number){ //id of statement. ij = id json
                    if (jsonElement.TryGetProperty("event", out JsonElement eventJsonElement)) { //this statement triggers an in game event. ej = event json
                        if (eventJsonElement.ValueKind == JsonValueKind.String){ //for now action is an int, to be mapped to an enum probably. might end up being a string
                            statementNode.eventType = Enum.Parse<EventType>(eventJsonElement.GetString()); //not worrying about payload for dialogue-triggered events
                        } else { return false; } //event json not string
                    }
                    if (jsonElement.TryGetProperty("exec", out JsonElement dialogueStartExec)){
                        if(dialogueStartExec.TryGetProperty("name", out JsonElement functionName)){
                            statementNode.startFunction = functionName.ToString();
                        }
                    }
                    if ( jsonElement.TryGetProperty("responses", out JsonElement responsesJsonElement)){ //responses. rj = responses json
                        if (responsesJsonElement.ValueKind == JsonValueKind.Array){
                            foreach (JsonElement response in responsesJsonElement.EnumerateArray()){
                                if (response.TryGetProperty("text", out JsonElement responseTextJsonElement)){
                                    if(response.TryGetProperty("next", out JsonElement nextIdJsonElement) && nextIdJsonElement.ValueKind == JsonValueKind.Number){
                                        //rtj = response text json, nij = next id json
                                        statementNode.responses.Add(new ResponseNode(responseTextJsonElement.GetString(), nextIdJsonElement.GetInt32()));   
                                    }
                                    else if (response.TryGetProperty("exec", out JsonElement dialogueEndExec)) {
                                        if(dialogueEndExec.TryGetProperty("success", out JsonElement successJsonElement) && dialogueEndExec.TryGetProperty("failure", out JsonElement failureJsonElement) && dialogueEndExec.TryGetProperty("name", out JsonElement methodNameJsonElement)){
                                            if(dialogueEndExec.TryGetProperty("args", out JsonElement argsJsonElement) && argsJsonElement.ValueKind == JsonValueKind.Array){
                                                statementNode.responses.Add(new ResponseNode(responseTextJsonElement.GetString(), methodNameJsonElement.GetString(), successJsonElement.GetInt32(), failureJsonElement.GetInt32(), argsJsonElement.EnumerateArray().Select(x => x.ToString() as object).ToList()));
                                            } else {
                                                statementNode.responses.Add(new ResponseNode(responseTextJsonElement.GetString(), methodNameJsonElement.GetString(), successJsonElement.GetInt32(), failureJsonElement.GetInt32(), null));
                                            }
                                            
                                        }
                                    }
                                } else { return false; }
                                
                            }
                        } else { return false; }
                    } else if (jsonElement.TryGetProperty("next", out JsonElement nextJsonElement) && nextJsonElement.ValueKind == JsonValueKind.Number){ //goes to another statement before player can respond
                        //nj = next id json
                        statementNode.nextStatementID = nextJsonElement.GetInt32();
                    }
                    statements[idJsonElement.GetInt32()] = statementNode;
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

    //restarts dialogue to beginning and returns that statement node. in the future probably will have checkpoints in a dialogue
    public StatementNode Start(){
        currentStatement = statements.First().Value;
        return currentStatement;
    }
    //does the current statement have a next default statement?
    public bool HasNext(){
        return currentStatement.nextStatementID != -1;
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

