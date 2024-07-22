using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
/*
 * represents a conversation between the player and another character. keeps track of progression through the dialogue. 
 **/
public partial class Dialogue : Resource
{
    //dialogue is a tree. root is always from the NPC, though they may say nothing.
    public Graph<string> dialogueTree { get; set; }
    public Graph<string>.Node dialogueCurrentPoint { get; set; }

    [JsonInclude]
    public List<string> statements { get; set; }

    [JsonInclude]
    public string testStr { get; set; }

    
    public int testExp; 


    public Dialogue()
    {
        dialogueTree = new Graph<string>();
    }

    public Dialogue(string start)
    {
        dialogueTree = new Graph<string>(start);
    }

    //return next thing to say when player says nothing
    public string Next()
    { 
        if (dialogueCurrentPoint == null)
        {
            dialogueCurrentPoint = dialogueTree.root;
        } else if (statements != null && statements.Count > 0)
        {
            Random rand = new Random();
            return statements[rand.Next(0, statements.Count)];
        }

        return dialogueCurrentPoint.value;
    }

    //return next thing to say based on what player says
    public string Next(int i)
    {
        //TODO
        string res;
        if (dialogueCurrentPoint == null)
        {
            dialogueCurrentPoint = dialogueTree.root;
            res = dialogueCurrentPoint.value;
        } else
        {
            res = dialogueTree.adjacencyList[dialogueCurrentPoint][i].value; 
            dialogueCurrentPoint = dialogueTree.adjacencyList[dialogueCurrentPoint][i];
        }
        return res;
    }

}