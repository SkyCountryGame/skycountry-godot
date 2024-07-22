using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class Talker : Node, Interactable {
	public InteractionType interactionType { get => InteractionType.Dialogue; }
    public InteractionMethod interactionMethod { get => InteractionMethod.Use; }

    //[Export(PropertyHint.None, "dialogue")]
    [Export(PropertyHint.File, "dialogue-for-dude.json")]
    public String dialogueFilename = "assets/dialogue/0.json";
    public Dialogue dialogue;

    public override void _Ready(){
        ResourceManager.RegisterGameObject(this, GameObjectType.Interactable);
        try
        {
            string jsonString = File.ReadAllText(dialogueFilename);
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            dialogue = JsonSerializer.Deserialize<Dialogue>(jsonString);
        }
        catch (Exception e)
        {
            GD.Print("Failed to load dialogue from JSON file: " + e.Message);
        }
        //dialogue = new Dialogue(); //TODO load from config file that specifies what npc says what
    }

    //start dialogue when player interacts
    public dynamic Interact()
    {
        return dialogue.Next();
    }

    public void Retain()
    {
        //wat do?
    }

    public string Info()
    {
       return "A talking thing. ";
    }

    public bool IsInteractionValid(GameObject interactor)
    {
        return true;
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

}
