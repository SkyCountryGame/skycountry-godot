using System;
using Godot;
[GlobalClass]
public partial class ItemProperties : Resource {
    [Export]
    public bool usableWhileMoving;

    public ItemProperties() {
        usableWhileMoving = true;
    }
}