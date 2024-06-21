using Godot;
using SkyCountry.Controller;

namespace SkyCountry;

//NOTE: delete if not used
public abstract partial class GodotNodeAdapter : Node
{
    //private AbstractController controller;

    public override void _Process(double delta)
    {
        //controller.update(delta);
    }
}