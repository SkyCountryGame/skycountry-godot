using System;
using Godot;

/**
    this is an explicit implementation of an interactable lamppost
*/
public partial class LampPost : Node, Interactable {


    public InteractionMethod interactionMethod => InteractionMethod.Use;

  	//[Export(PropertyHint.Enum, "interactionType")]
    public InteractionType interactionType => InteractionType.General;
    private OmniLight3D lamplight;
    private int n = 0;

    public override void _Ready()
	{
        //Global.interactables.Add(this);
        Global.RegisterGameObject(this, GameObjectType.Interactable);
        lamplight = GetNode<OmniLight3D>("CollisionShape3D/StaticBody3D/OmniLight3D");
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public string Info()
    {
        return "Does one seek light?";
    }

    public dynamic Interact()
    {
        if (n > 4){
            //NOTE this is obviously not how we will be changing levels in the end. it is just proof of concept
            PackedScene levelscene = ResourceLoader.Load<PackedScene>("res://level2.tscn");
            //Node nextlevel = levelscene.Instantiate();
            //PackedScene player = ResourceLoader.Load<PackedScene>("res://player.tscn");
            //nextlevel.AddChild(Global._P.playerNode);
            //TODO need to think about level switching logic. there will be 2 different types of scene switch: enter/exit rooms, and switch "big levels". in first case, want to keep things in memory, in second case, want to free old scene from mem.
            //GetTree().Root.RemoveChild(GetParent());
            //GetTree().Root.AddChild(nextlevel);
            GetTree().ChangeSceneToPacked(levelscene);
        }
        //toggle lamp light
        lamplight.LightEnergy = lamplight.LightEnergy == 0 ? 1 : 0;
        n += 1;
        return true;
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        throw new NotImplementedException();
    }

    public void Retain()
    {
        throw new NotImplementedException();
    }

}
