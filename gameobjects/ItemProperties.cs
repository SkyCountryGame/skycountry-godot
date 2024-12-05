using System;
using Godot;
[GlobalClass]
public partial class ItemProperties : Resource {
    [Export]
    public string scenePath;
    [Export]
    public bool usableWhileMoving;
}