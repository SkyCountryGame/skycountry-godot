using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * represents a conversation between the player and another character. keeps track of progression through the dialogue. 
 **/
public class Dialogue
{
    //dialogue is a tree. root is always from the NPC, though they may say nothing.
    public Graph<string> dialogueTree;
    public Graph<string>.Node dialogueCurrentPoint;

    public List<string> responsesToSilence;

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
        } else if (responsesToSilence != null && responsesToSilence.Count > 0)
        {
            return responsesToSilence[Random.Range(0, responsesToSilence.Count)];
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