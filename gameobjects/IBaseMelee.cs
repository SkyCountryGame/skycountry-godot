using Godot;
using System;

public interface IBaseMelee {

    string name {get;}
    string equipPath {get;}
    string swingAnimation {get;}
    int durability {get;}
    int damage{get;}
    int arcLength {get;}
    int range {get;}
    int reswingSpeed {get;}
    Animation Swing(AnimationTree animationTree);
}