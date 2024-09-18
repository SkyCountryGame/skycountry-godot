using Godot;
using System;

public partial class PauseMenu : Control
{
	[Export]
	public Button buttonResume;
	[Export]
	public Button buttonLoad;
	[Export]
	public Button buttonSave;
	[Export]
	public Button buttonQuit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buttonResume.Pressed += OnButtonResumePressed;
		buttonSave.Pressed += OnButtonSavePressed;
		buttonLoad.Pressed += OnButtonLoadPressed;
		buttonQuit.Pressed += OnButtonQuitPressed;
		// = GetTree().CurrentScene; //tell the level manager what scene we are in
		Global.pauseMenu = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonResumePressed()
	{
		Global.ResumeGame();
	}
	private void OnButtonLoadPressed()
	{
		PackedScene level0 = ResourceLoader.Load<PackedScene>("res://levels/level0.tscn"); //TODO
		Resource playerModel = ResourceLoader.Load<Resource>($"res://saves/{Global.saveSlot}.playerModel.tres");
		GetTree().ChangeSceneToPacked(level0);
		Global.playerModel = playerModel as PlayerModel;
		Global.playerNode.SetPlayerModel(Global.playerModel);
	}

	private void OnButtonSavePressed()
	{
		GD.Print("TODO!!");
		ResourceSaver.Save(Global.playerModel, $"res://saves/{Global.saveSlot}.playerModel.tres");
		
	}
	private void OnButtonQuitPressed()
	{
		//TODO confirm dialogue
		GetTree().Quit();
	}
}
