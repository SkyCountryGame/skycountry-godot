using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public partial class HUDManager : Node {

    public Node eventLog;
    public PanelContainer dialoguePanel;
    private RichTextLabel dialogueText;

    public ConcurrentQueue<string> messages; //the messages currently displayed
    private bool needsUpdate = false;

    enum State {
        DEFAULT,
        DIALOGUE
    }
    private State state = State.DEFAULT;

    public override void _Ready(){
        eventLog = GetNode("EventLog");
        dialoguePanel = GetNode<PanelContainer>("DialoguePanel");
        dialogueText = dialoguePanel.GetNode<RichTextLabel>("MessageLabel"); //this is the text node that is the current message of dialogue
        messages = new ConcurrentQueue<string>();
        dialoguePanel.Visible = false;
    }

    public override void _Process(double dt){
        if (needsUpdate){
            UpdateEventLog();
            needsUpdate = false;
        }
    }

    private void UpdateState(State s){
        switch (s){
            case State.DEFAULT:
                dialoguePanel.Visible = false;
                state = s;
                break;
            case State.DIALOGUE:
                dialoguePanel.Visible = true;
                state = s;
                break;
        }
    }

    /**
     * update the event log on the HUD with the list of messages
     */
    private void UpdateEventLog(){
        string eventLogText = "";
        foreach (string s in messages){
            eventLogText += s + "\n";
        }
        eventLog.GetNode<Label>("Label").Text = eventLogText;
    }

    public void ShowDialogue(string msg){
        //show the dialogue without text disappearing
        UpdateState(State.DIALOGUE);
        dialogueText.Text = msg;
    }

    public void HideDialogue(){
        UpdateState(State.DEFAULT);
    }

    /**
     * add a message to the event log on the HUD
     */
    public void LogEvent(string message){
        messages.Enqueue(message);
        UpdateEventLog();
        Task removeMessage = new Task(() => {
            GD.Print("start rmv msg task");
            Thread.Sleep(2000);
            while (!messages.TryDequeue(out string _)){
                Thread.Sleep(100);
                GD.Print("failed to remove message");
            }
            needsUpdate = true;
            //UpdateEventLog();
        });
        removeMessage.Start();
        
    }
}