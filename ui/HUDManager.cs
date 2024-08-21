using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public partial class HUDManager : Node {

    public Node eventLog;

    public Label actionLabel; //pops up when there is an interaction available or other action

    //dialogue stuff. TODO move into DialogueUI class cause this is getting a little messy
    public VBoxContainer dialoguePanel;
    private RichTextLabel dialogueText;
    private ItemList dialogueChoices;
    private Label dialogueTitle; //title of dialogue. probably NPC name
    private Button buttonContinue; //continue dialogue button
    private Button buttonExit; //exit dialogue
    
    private Dialogue currentDialogue; //currently active dialogue, if any
    private Event dialogueEvent = null; //event to be triggered when current statement node is done

    //inventory stuff    
    private ItemList inventoryMenu;

    private Label equippedLabel;
    
    private Label hpLabel;

    public ConcurrentQueue<string> messages; //the messages currently displayed
    private bool needsUpdate = false;
    

    enum State { //TODO hud might not need its own state because can just look at player
        DEFAULT,
        DIALOGUE,
        INVENTORY,
        DANGER,
        PAUSE,
        CUTSCENE
    }
    private State state = State.DEFAULT;

    public override void _Ready(){
        Global.HUD = this;
        eventLog = GetNode("MarginContainer/EventLog");
        dialoguePanel = GetNode<VBoxContainer>("DialoguePanel");
        dialogueText = dialoguePanel.GetNode<RichTextLabel>("PanelContainerMessage/MessageLabel"); //this is the text node that is the current message of dialogue
        dialogueChoices = dialoguePanel.GetNode<ItemList>("ResponsesList");
        buttonContinue = dialoguePanel.GetNode<Button>("PanelContainerTop/InfoPanel/ButtonContinue");
        buttonContinue.Visible = false;
        messages = new ConcurrentQueue<string>();
        dialoguePanel.Visible = false;
        inventoryMenu = GetNode<ItemList>("InventoryMenu");
        inventoryMenu.Visible = false;
        actionLabel = GetNode<Label>("LabelAction");
        actionLabel.Visible = false;
        equippedLabel = GetNode<Label>("LabelEquip");
    }

    public override void _Process(double dt){
        if (needsUpdate){
            UpdateEventLog();
            needsUpdate = false;
        }
        //TODO show equipped item
    }

    //NOTE does hud actually need a state system?
    private void UpdateState(State s, dynamic payload = null){
        switch (s){
            case State.DEFAULT:
                dialoguePanel.Visible = false;
                state = s;
                break;
            case State.DIALOGUE:
                if (payload != null && payload is Dialogue){
                    currentDialogue = (Dialogue)payload;
                }
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

    public void ShowDialogue(Dialogue d){
        //show the dialogue without text disappearing
        //first get the current state of dialogue with the thing to which you are talking 
        UpdateState(State.DIALOGUE, d);
        //if (state == State.DIALOGUE){ //we're already in dialogue, so continue
        if (Global.PlayerModel.activityState == PlayerModel.State.DIALOGUE){
            UpdateDialoguePanel(currentDialogue.currentStatement); //not necessarily the first statement
            return;
        } else {

        }
    }
    //NOTE might move dialogue stuff into DialogueManager
    public void HideDialogue(){
        UpdateState(State.DEFAULT);
    }
    public void ContinueDialogue(){

    }
    public void OnDialogueResponseItemClicked(int index, Vector2 pos, int mouseButton){
        if (mouseButton == 1){ //left click
            int id = (int) dialogueChoices.GetItemMetadata(index); //get the id of the next statement
            if (dialogueEvent != null){ //invoke event if one is associated with the statement
                dialogueEvent.Invoke();
                dialogueEvent = null;
            }
            if (id == -1){
                HideDialogue();
                Global.PlayerModel.UpdateState(PlayerModel.State.DEFAULT);
                return;
            }
            Dialogue.StatementNode sn = currentDialogue.statements[id];
            currentDialogue.currentStatement = sn;
            UpdateDialoguePanel(sn);
        }
    }
    public void OnButtonContinuePressed(){ //yes it does feel wrong to be messing with internal structure of dialogue object from the hud
        //continue button is only visible when there is a direct continuation to another statement
        Dialogue.StatementNode sn = currentDialogue.Next(); //.statements[currentDialogue.currentStatement.nextStatementID];
        //TODO just call currentDialogue.Next()
        //currentDialogue.currentStatement = sn;
        UpdateDialoguePanel(sn);
    }
    //update the dialogue panel. display a statement and its responses
    private void UpdateDialoguePanel(Dialogue.StatementNode sn){
        //dialogueText.Clear();
        dialogueChoices.Clear();
        dialogueText.Text = sn.statement;
        int idx = -1; //index of response just added to list
        if (sn.responses.Count > 0){
            foreach (Dialogue.ResponseNode r in sn.responses){
                idx = dialogueChoices.AddItem(r.response);
                dialogueChoices.SetItemMetadata(idx, r.nextStatementID);
            }
            idx = dialogueChoices.AddItem("Exit");
            dialogueChoices.SetItemMetadata(idx, -1);
            buttonContinue.Visible = false;
        } else {
            if (sn.nextStatementID == -1){
                idx = dialogueChoices.AddItem("Exit");
                buttonContinue.Visible = false;
            } else {
                idx = dialogueChoices.AddItem("Continue");
                buttonContinue.Visible = true;
            }
            dialogueChoices.SetItemMetadata(idx, sn.nextStatementID);
        } 
        
        if (sn.eventType != EventType.None){
            dialogueEvent = new Event(sn.eventType);
        }
    }
    
    public void Back(){
        switch (state){

        }
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

    //also used to update the inventory list if it's already visible
    public void ShowInventory(){ //TODO reactive fix update: we hold a reference to inventory, and ItemList automatically updates on change
        inventoryMenu.Visible = true;
    }
    public void HideInventory(){
        inventoryMenu.Visible = false;   
    }
    public void UpdateInventoryMenu(Inventory inv){
        inventoryMenu.Clear();
        //int idx = inventoryMenu.AddItem("Inventory", null, false);
        //inventoryMenu.SetItemDisabled(idx, true);
        //inventoryMenu.SetItemCustomBgColor(idx, new Color(0.5f, 0.5f, 0.5f, 0.5f));
        foreach (InventoryItem item in inv.GetItems()){
            int idx = -1;
            if (item == Global.PlayerModel.equipped){
                idx = inventoryMenu.AddItem($" - {item.name} - ");
            } else {
                idx = inventoryMenu.AddItem(item.name);
            }
            inventoryMenu.SetItemMetadata(idx, item.id); //assoc the inventory item
        }
    }

    public void ToggleInventory(Inventory inv){
        UpdateInventoryMenu(inv);
        if (inventoryMenu.Visible){
            HideInventory();
        } else {
            ShowInventory();
        }
    }
    public void ShowEquipped(string label = null){ 
        if (label == null){
            equippedLabel.Visible = false;
        } else {
            equippedLabel.Visible = true;
            equippedLabel.Text = label;
        }
    }

    public void ShowAction(string text){
        actionLabel.Visible = true;
        actionLabel.Text = "interact: " + text; //TODO this will have a sprite for key instead of 'interact'
    }
    public void HideAction(){
        actionLabel.Visible = false;
    }

    public void OnInventoryMenuItemClicked(int index, Vector2 pos, int mouseButton){
        GD.Print($"clicked {index}");
        InventoryItem item = Global.PlayerModel.inv.GetItemByIndex(index); //GetItemMetadata shouldn't be null because we always set it when adding the menu items
        if (item != null){
            if (mouseButton == 1){ //left click
                if (Global.PlayerModel.EquipItem(item)){
                    inventoryMenu.SetItemText(index, $" - {item.name} - "); //TODO bad. will be fixed after reactive ui update
                    ShowEquipped(item.name);
                }
            } else if (mouseButton == 2){
                if (Global.PlayerModel.DropItem(item)){
                    inventoryMenu.RemoveItem(index);
                }
            }
        }       
    }

    /* might have to use this later when HUD elements are focused (taking input priority)
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }*/
}