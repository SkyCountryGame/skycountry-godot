using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public partial class HUDManager : Node {

    public Node eventLog;

    public Label actionLabel; //pops up when there is an interaction available or other action

    //dialogue stuff
    public PanelContainer dialoguePanel;
    private RichTextLabel dialogueText;
    private ItemList dialogueChoices;

    //inventory stuff    
    private ItemList inventoryMenu;

    private Label equippedLabel;
    
    private Label hpLabel;


    public ConcurrentQueue<string> messages; //the messages currently displayed
    private bool needsUpdate = false;

    enum State {
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
        dialoguePanel = GetNode<PanelContainer>("DialoguePanel");
        dialogueText = dialoguePanel.GetNode<RichTextLabel>("MessageLabel"); //this is the text node that is the current message of dialogue
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

    public void ShowInventory(Inventory inv){ //TODO reactive fix update: we hold a reference to inventory, and ItemList automatically updates on change
        inventoryMenu.Visible = true;
        //int idx = inventoryMenu.AddItem("Inventory", null, false);
        //inventoryMenu.SetItemDisabled(idx, true);
        //inventoryMenu.SetItemCustomBgColor(idx, new Color(0.5f, 0.5f, 0.5f, 0.5f));
        foreach (InventoryItem item in inv.GetItems()){
            int idx = -1;
            if (item == Global._P.equipped){
                idx = inventoryMenu.AddItem($" - {item.name} - ");
            } else {
                idx = inventoryMenu.AddItem(item.name);
            }
            inventoryMenu.SetItemMetadata(idx, item.id); //assoc the inventory item
        }
    }
    public void HideInventory(){
        inventoryMenu.Clear();
        inventoryMenu.Visible = false;
        
    }
    public void ToggleInventory(Inventory inv){
        if (inventoryMenu.Visible){
            HideInventory();
        } else {
            ShowInventory(inv);
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
        InventoryItem item = Global._P.inv.GetItemByIndex(index); //GetItemMetadata shouldn't be null because we always set it when adding the menu items
        if (item != null){
            if (mouseButton == 1){ //left click
                if (Global._P.EquipItem(item)){
                    inventoryMenu.SetItemText(index, $" - {item.name} - "); //TODO bad. will be fixed after reactive ui update
                    ShowEquipped(item.name);
                }
            } else if (mouseButton == 2){
                if (Global._P.DropItem(item)){
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