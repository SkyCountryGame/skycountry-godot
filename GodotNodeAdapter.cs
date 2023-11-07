using Godot;
using SkyCountry.Controller;

namespace SkyCountry;

public abstract partial class GodotNodeAdapter : Node
{
    private AbstractController controller;

    public override void _Process(double delta)
    {
        controller.update(delta);
    }
}