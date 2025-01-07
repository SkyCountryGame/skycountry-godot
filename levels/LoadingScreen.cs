using Godot;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class LoadingScreen : Node
{
	[Export(PropertyHint.File, "All the tscn files that should be preloaded")] private Godot.Collections.Array<string> tscnFilepathsToLoad;
	[Export] private string nextScene; //the scene to load after the timer is up

	Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (tscnFilepathsToLoad == null){
			return;
		}
		Task.Run( () => {
			PrefabManager.LoadPrefabs(tscnFilepathsToLoad.ToArray());
			CallDeferred("StartGame");
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartGame(){
		GetTree().ChangeSceneToPacked(PrefabManager.Get(nextScene));
	}
}
