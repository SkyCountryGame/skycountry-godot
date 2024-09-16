using Godot;

//purpose of this class is to associate a pickupable object with its PackedScene
public partial class InteractableNode : Node, Interactable
{
    [Export]
    public PackedScene packedScene;

    [Export]
    private InteractionType _interactionType = InteractionType.Pickup;
    [Export]
    private InteractionMethod _interactionMethod = InteractionMethod.Use;
    public InteractionType interactionType => _interactionType;
    public InteractionMethod interactionMethod => _interactionMethod;

    public InteractableNode(){}

    public override void _Ready()
    {
        Global.RegisterGameObject(this, Name, GameObjectType.Interactable);
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    //PAYLOAD: 
    public dynamic Interact()
    {
        return null;
    }

    public bool IsInteractionValid(Interactor interactor)
    {
        return true;
    }

    public void Retain()
    {
        throw new System.NotImplementedException();
    }

    public string Info(){
        throw new System.NotImplementedException();
    }

}