using Godot;
using System;
using System.Collections.Generic;

public partial class DevTool : Node2D
{

	private PanelContainer devPanel; //UI panel that has some info and controls for dev and debug
	private VSlider slider1; //accel
	private Label label1; //label for what the slider sets
	private Label labelValue1; //label for the value of the slider
	private VSlider slider2; //max vel
	private Label labelValue2; 
	private VSlider slider3; //jump vel
	private Label labelValue3; 
	//private VSlider slider4; //gravity accel
	private TextEdit textEdit4; //gravity accel
	private Label labelValue4; 

	private Button button1; //save resource test
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		devPanel = GetNode<PanelContainer>("DevPanel");
		devPanel.Hide();
		slider1 = GetNode<VSlider>("DevPanel/HBoxContainer/VBox1/VSlider"); //accel
		slider1.MaxValue = 200;
		slider1.MinValue = 0;
		labelValue1 = GetNode<Label>("DevPanel/HBoxContainer/VBox1/LabelValue");
		slider2 = GetNode<VSlider>("DevPanel/HBoxContainer/VBox2/VSlider"); //vel
		slider2.MinValue = 1;
		slider2.MaxValue = 256;
		labelValue2 = GetNode<Label>("DevPanel/HBoxContainer/VBox2/LabelValue");
		slider3 = GetNode<VSlider>("DevPanel/HBoxContainer/VBox3/VSlider"); //jump
		slider3.MinValue = 0;
		slider3.MaxValue = 228;
		labelValue3 = GetNode<Label>("DevPanel/HBoxContainer/VBox4/LabelValue");
		//slider4 = GetNode<VSlider>("DevPanel/HBoxContainer/VBox4/VSlider"); //gravity
		textEdit4 = GetNode<TextEdit>("DevPanel/HBoxContainer/VBox4/TextEdit"); //gravity
		labelValue4 = GetNode<Label>("DevPanel/HBoxContainer/VBox4/LabelValue");

		slider1.DragEnded += OnSlider1Changed;
		slider2.DragEnded += OnSlider2Changed;
		slider3.DragEnded += OnSlider3Changed;
		//slider4.DragEnded += OnSlider4Changed;
		//textEdit4.TextChanged += OnTextEdit4Changed;
		/*labelValue1.Text = Global.playerNode.accelScalar.ToString();
		labelValue2.Text = Global.playerNode.velMagnitudeMax.ToString();
		labelValue3.Text = Global.playerNode.JumpVelocity.ToString();
		labelValue4.Text = Global.playerNode.gravity.ToString();
		textEdit4.Text = Global.playerNode.gravity.ToString();
		slider1.Value = Global.playerNode.accelScalar;
		slider2.Value = Global.playerNode.velMagnitudeMax;
		slider3.Value = Global.playerNode.JumpVelocity;
		//slider4.Value = Global.playerNode.gravity;
		*/
		button1 = GetNode<Button>("DevPanel/HBoxContainer/Button1");
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
		Global.playerNode.accelScalar = (float)slider1.Value;
		slider1.TooltipText = slider1.Value.ToString();
		labelValue1.Text = slider1.Value.ToString();
	}
	private void OnSlider2Changed(bool valChanged){
		Global.playerNode.velMagnitudeMax = (float)slider2.Value;
		slider2.TooltipText = slider2.Value.ToString();
		labelValue2.Text = slider2.Value.ToString();
	}
	private void OnSlider3Changed(bool valChanged){
		Global.playerNode.JumpVelocity = (float)slider3.Value;
		slider3.TooltipText = slider3.Value.ToString();
		labelValue3.Text = slider3.Value.ToString();
	}
	/*private void OnSlider4Changed(bool valChanged){
		Global.playerNode.gravity = (float)slider4.Value;
		slider4.TooltipText = slider4.Value.ToString();
		labelValue4.Text = slider4.Value.ToString();//Global.playerNode.gravity.ToString();
	}*/
	private void OnTextEdit4Changed(){
		double val = Global.playerNode.gravity;
		try {
			double grav = float.Parse(textEdit4.Text);
			Global.playerNode.gravity = grav;
			labelValue4.Text = grav.ToString();
		} catch (FormatException e){
			GD.Print("gravity must be a number");
		}
	}

	private void OnButton1Pressed()
	{
		ResourceSaver.Save(Global.playerModel, "res://player/PlayerModel.tres");
	}
}
