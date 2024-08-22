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
		Global.sceneManager.SetActiveLevelScene(GetTree().CurrentScene); //tell the level manager what scene we are in
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonStartPressed()
	{
		Global.sceneManager.ChangeLevel("l0");
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
