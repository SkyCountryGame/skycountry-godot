using System.Net.NetworkInformation;
using Godot;

public partial class HUDManager : Node {

    public Node eventLog;

    public override void _Ready(){
        eventLog = GetNode("EventLog");
    }

    /**
     * add a message to the event log on the HUD
     */
    public void LogEvent(string message){
        eventLog.GetNode<Label>("EventLog/Label").Text = message;
    }
}