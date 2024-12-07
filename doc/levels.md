currently, Level base class holds a Map<string, PackedScene> of levels, loaded from filenames specified via editor. these are loaded from disk upon level scene _Ready(). 
i think this should be moved to Global, so that levels aren't reloaded when player enters another level.

idk if it would be useful to eventually have a level graph that uses the actual Level objects, like so: 
//which levels are accessible from each level. this includes "sublevels"
//public static Dictionary<Level, List<Level>> levelGraph = new Dictionary<Level, List<Level>>();