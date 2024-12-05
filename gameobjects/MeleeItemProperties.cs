using Godot;
using System;
[GlobalClass]
public partial class MeleeItemProperties : ItemProperties {
    [Export]
    public string swingAnimation;
    [Export]
    public int durability;
    [Export]
    public int damage;
    [Export]
    public int miningDamage;
    [Export]
    public int arcLength;
    [Export]
    public int range;
    [Export]
    public int reswingSpeed;
}