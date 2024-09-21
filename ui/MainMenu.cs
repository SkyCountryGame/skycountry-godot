using Godot;
using System;

public partial class MainMenu : Control
{
	[Export]
	public TextureButton buttonStart;
	[Export]
	public TextureButton buttonLoad;
	[Export]
	public TextureButton buttonQuit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buttonStart.Pressed += OnButtonStartPressed;
		buttonLoad.Pressed += OnButtonLoadPressed;
		buttonQuit.Pressed += OnButtonQuitPressed;
		// = GetTree().CurrentScene; //tell the level manager what scene we are in
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonStartPressed()
	{
		Global.saveSlot = 0; //NOTE do we want to instead resume from last played game?
		PackedScene level0 = ResourceLoader.Load<PackedScene>("res://levels/level0.tscn"); //TODO
		GetTree().ChangeSceneToPacked(level0);
	}
	private void OnButtonLoadPressed()
	{
		GD.Print("TODO!!");
	}
	private void OnButtonQuitPressed()
	{
		GetTree().Quit();
	}
}
