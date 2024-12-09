using Godot.Collections;
using Godot;
using State = StateManager.State;
using System.Numerics;
using System;
using System.Collections.Generic;

//defines the animation to set for each state
[GlobalClass]
public partial class AnimationController : Node
{
    [Export] private Godot.Collections.Dictionary<string, string> mapStateStringAnim = new Godot.Collections.Dictionary<string, string>();
    [Export] private AnimationPlayer animPlayer;
    private System.Collections.Generic.Dictionary<State, string> mapStateAnim = new System.Collections.Generic.Dictionary<State, string>();
    public override void _Ready()
    {
        base._Ready();
        if (animPlayer == null)
        {
            animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }
        foreach (KeyValuePair<string, string> entry in mapStateStringAnim)
        {
            State s = (State)Enum.Parse(typeof(State), entry.Key);
            mapStateAnim[s] = entry.Value;
            animPlayer.GetAnimation(entry.Value).LoopMode = Animation.LoopModeEnum.Linear;
        }
    }

    public void PlayAnimation(string animName)
    {
        animPlayer.Play(animName, -1, 1, true);
        
    }

    public void HandleStateChange(State state){
        GD.Print($"animctlr HandleStateChange {mapStateAnim[state]}");
        if (mapStateAnim.ContainsKey(state)){
            PlayAnimation(mapStateAnim[state]);
        }
    }
}