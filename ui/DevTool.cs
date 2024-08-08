using Godot;
using System;
using System.Collections.Generic;

public partial class DevTool : Node2D
{

	private PanelContainer devPanel; //UI panel that has some info and controls for dev and debug
	private VSlider slider1; //accel
	private Label label1; //label for what the slider sets
	private Label labelValue1; //label for the value of the slider
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		devPanel = GetNode<PanelContainer>("DevPanel");
		devPanel.Hide();
		slider1 = GetNode<VSlider>("DevPanel/VBox1/VSlider");
		slider1.MaxValue = 200;
		slider1.MinValue = 0;
		labelValue1 = GetNode<Label>("DevPanel/VBox1/LabelValue");
		slider1.DragEnded += OnSlider1Changed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("devmode")){
			GD.Print("devmode");
			ToggleDevPanel();
		
		}
    }

	private void ToggleDevPanel()
	{
		devPanel.Visible = !devPanel.Visible;
	}

	private void OnSlider1Changed(bool valChanged)
	{
		Global.PlayerNode.accelScalar = (float)slider1.Value;
		slider1.TooltipText = slider1.Value.ToString();
		labelValue1.Text = slider1.Value.ToString();
	}
}
