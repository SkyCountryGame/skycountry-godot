using Godot;
using System;

public partial class TimerRandomInterval : Timer
{
	[Export] public int baseWaitTime = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timeout += () => {
			WaitTime = new Random().NextDouble() * 5 + baseWaitTime;
			Start();
		};
	}

	public void Reset(float waitTime = -1){ 
		Stop();
		if (waitTime != 1){
			WaitTime = waitTime;
		} else {
			WaitTime = new Random().NextDouble() * 5 + baseWaitTime;
		}
		Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
