using System;
using Godot;

public partial class Drugz : StaticBody3D, Interactable {
    public InteractionType interactionType => InteractionType.Function;

    public InteractionMethod interactionMethod => InteractionMethod.Contact;

    private Func<Player, dynamic> f;

    public Drugz()
    {
        f = (Player p) => {
            GD.Print("drgz func");
            return null;
        };
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SceneManager.RegisterGameObject(this, GameObjectType.Interactable);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public string Info()
    {
        return "Drug";
    }

    //PAYLOAD 
    public dynamic Interact()
    {
        return f;
    }

    public void Retain()
    {
        throw new System.NotImplementedException();
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }
}