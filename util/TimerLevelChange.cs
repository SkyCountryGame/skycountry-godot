using Godot;
using System;

public partial class TimerLevelChange : Timer
{
	[Export] public int time = 7;
	[Export] private string level;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (level == null){
			GD.PushWarning("level not set for timer");
			return;
		}
		Timeout += () => {
			GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(level));
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
