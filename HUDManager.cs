using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public partial class HUDManager : Node {

    public Node eventLog;
    public ConcurrentQueue<string> messages; //the messages currently displayed
    private bool needsUpdate = false;

    public override void _Ready(){
        eventLog = GetNode("EventLog");
        messages = new ConcurrentQueue<string>();
    }

    public override void _Process(double dt){
        if (needsUpdate){
            UpdateEventLog();
            needsUpdate = false;
        }
    }

    /**
     * update the event log on the HUD with the list of messages
     */
    public void UpdateEventLog(){
        string eventLogText = "";
        foreach (string s in messages){
            eventLogText += s + "\n";
        }
        eventLog.GetNode<Label>("Label").Text = eventLogText;
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