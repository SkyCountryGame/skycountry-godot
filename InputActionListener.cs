using Godot;

namespace SkyCountry;

public interface InputActionListener
{
    void HandleActionEnable(InputEventAction type, bool en);

    //currently unused. will it every be used?
    void HandleContinuousAction(InputEventAction a);
}